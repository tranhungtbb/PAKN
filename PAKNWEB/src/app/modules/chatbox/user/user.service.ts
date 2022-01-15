import { EventEmitter, Injectable, OnInit } from '@angular/core'
import { Subject } from 'rxjs'
import { CanActivate, Router } from '@angular/router'
import { QBHelper } from '../helper/qbHelper'
import { QBconfig } from '../QBconfig'
import { LoginModel } from '../user/login/loginModel'
import { UserService } from 'src/app/services/user.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
declare var QB: any

@Injectable({
  providedIn: 'root'
})
export class UserServiceChatBox implements CanActivate {
  public errorSubject = new Subject<any>();
  public successSubject = new Subject<boolean>();
  public user: any;
  public _usersCache = [];
  usersCacheEvent: EventEmitter<any> = new EventEmitter();
  public loginModel = new LoginModel()

  // params = {
  // 	login: this.login(this.loginModel),
  // 	password: "someSecret",
  // 	full_name: "QuickBlox Test"
  // };

  constructor(private qbHelper: QBHelper, private router: Router, private userService: UserService, private stogateService: UserInfoStorageService,) {
    //this.LoginQL();
  }
  // ngOnInit(){}

  // public LoginQL() {			
  // 	this.createUser(this.params);
  //   this.login(this.loginModel);

  // }	

  canActivate(): boolean {
    const self = this;
    this.user = JSON.parse(localStorage.getItem('loggedinUser'));
    const sessionResponse = JSON.parse(localStorage.getItem('sessionResponse'));
    if (this.qbHelper.getSession() && this.user && sessionResponse) {
      return true;
    } else if (sessionResponse && this.user) {
      self.login({
        userLogin: this.user.login,
        userName: this.user.full_name,
        id: self.stogateService.getUserId()
      }).then(() => {
        this.router.navigate(['quan-tri/chatbox']);
      });
    } else {
      self.qbHelper.qbLogout();
    }
  }

  public addToCache(user: any) {
    const id = user.idQB;
    if (!this._usersCache[id]) {
      this._usersCache[id] = {
        id: id,
        color: Math.floor(Math.random() * (10 - 1 + 1)) + 1
      };
    }
    this._usersCache[id].last_request_at = user.last_request_at;
    this._usersCache[id].name = user.full_name || user.login || 'Unknown user (' + id + ')';
    this.usersCacheEvent.emit(this._usersCache);
    return this._usersCache[id];
  }

  public createUserCB(user) {
    if (QB.users.create(user)) {
      console.log('User Creation successfull ');
    };
  }

  // create User
  public createUser(user): Promise<any> {
    return new Promise((resolve, reject) => {
      QB.users.create(user, function (createErr, createRes) {
        if (createErr) {
          console.log('User creation Error : ', createErr);
          reject(createErr);
        } else {
          console.log('User Creation successfull ');
          resolve(createRes);
        }
      });
    });
  }

  // update User
  public updateUser(userId, params): Promise<any> {
    const self = this;
    return new Promise((resolve, reject) => {
      QB.users.update(userId, params, function (updateError, updateUser) {
        if (updateError) {
          console.log('User update Error : ', updateError);
          reject(updateError);
        } else {
          self.addToCache(updateUser);
          console.log('User update successfull ', updateUser);
          resolve(updateUser);
        }
      });
    });
  }

  // delete user


  deleteUser(user: any): Promise<any> {
    return new Promise((resolve, reject) => {
      QB.init(QBconfig.credentials.appId, QBconfig.credentials.authKey, QBconfig.credentials.authSecret, QBconfig.credentials.accountKey, QBconfig.appConfig);
      this.qbHelper.qbCreateConnection(user)
        .then((loginRes) => {
          QB.users.delete(loginRes.id, function (err, result) {
            if (err) {
              reject(err);
            } else {
              resolve(result)
            }
          });
        })
        .catch((loginErr) => {

        });
    });
  }

  // get Users List
  public getUserList(args): Promise<any> {
    if (typeof args !== 'object') {
      args = {};
    }
    const
      self = this,
      params = {
        filter: {
          field: args.field || 'full_name',
          param: 'in',
          value: args.value || [args.full_name || '']
        },
        order: args.order || {
          field: 'updated_at',
          sort: 'desc'
        },
        page: args.page || 1,
        per_page: args.per_page || 100
      };
    return new Promise(function (resolve, reject) {
      QB.users.listUsers(params, function (userErr: any, usersRes: any) {
        if (userErr) {
          reject(userErr);
        } else {
          console.log('User List === ', usersRes);
          const users = usersRes.items.map((userObj: any) => {
            self.addToCache(userObj.user);
            return userObj.user;
          });
          resolve(users);
        }
      });
    });
  }

  public getUserListForChat(args): Promise<any> {
    const self = this;
    return new Promise(function (resolve, reject) {
      self.userService.getAllPagedListForChat(args).subscribe(res => {
        if (res.success == RESPONSE_STATUS.success) {
          resolve(res)
        }
        else {
          reject(res.message)
        }
      })
    });
  }

  public getUserListForDelete(args): Promise<any> {
    const self = this;
    return new Promise(function (resolve, reject) {
      self.userService.getAllByListIdQb(args).subscribe(res => {
        if (res.success == RESPONSE_STATUS.success) {
          resolve(res)
        }
        else {
          reject(res.message)
        }
      })
    });
  }



  public setUser(User) {
    this.user = User;
    localStorage.setItem('loggedinUser', JSON.stringify(User));
  }

  public login(loginPayload) {
    const self = this;
    return new Promise((resolve, reject) => {
      QB.init(QBconfig.credentials.appId, QBconfig.credentials.authKey, QBconfig.credentials.authSecret, QBconfig.credentials.accountKey, QBconfig.appConfig);
      const user = {
        login: loginPayload.userLogin,
        password: 'quickblox',
        full_name: loginPayload.userName,
        custom_data: JSON.stringify(loginPayload.id)
      };
      const loginSuccess = (loginRes) => {
        console.log('login Response :', loginRes);
        this.setUser(loginRes);
        console.log('chat connection :', loginRes.id, user.password);
        // Establish chat connection
        this.qbHelper.qbChatConnection(loginRes.id, user.password).then(chatRes => {
          this.successSubject.next(true);
          resolve(1);
        },
          chatErr => {
            console.log('chat connection Error :', chatErr);
          });
      };

      this.qbHelper.qbCreateConnection(user)
        .then((loginRes) => {
          /** Update info */
          loginSuccess(loginRes)
        })
        .catch((loginErr) => {
          if (loginErr.status === undefined || loginErr.status !== 401) {
            QB.users.create(user, function (createErr, createRes) {
              if (createErr) {
                console.log('User creation Error : ', createErr);
                reject(createErr);
              } else {
                console.log('User Creation successfull ');
                self.userService.updateBQId({ Id: loginPayload.id, IdQB: createRes.id }).subscribe(res => { })
                loginSuccess(createRes)
                resolve(createRes);
              }
            });
          }
        });
    });
  }



  async createUserForApp(user, files = null, isLogin: boolean = false) {
    const self = this;
    return new Promise((resolve, reject) => {
      QB.init(QBconfig.credentials.appId, QBconfig.credentials.authKey, QBconfig.credentials.authSecret, QBconfig.credentials.accountKey, QBconfig.appConfig);
      this.qbHelper.qbCreateConnection(user)
        .then((loginRes) => {
          delete user['password']
          if (files) {
            QB.content.createAndUpload(files, function (error, result) {
              if (error) {
              } else {
                user.blob_id = result.id
                QB.users.update(loginRes.id, user, function (error, users) {
                  if (error) {
                    reject(error)
                  } else {
                    self.userService.updateBQId({ Id: JSON.parse(user.custom_data).id, IdQB: users.id }).subscribe(res => { })
                    resolve(users)
                  }
                });
              }
            });
          }
          else {
            if (!isLogin) {
              QB.users.update(loginRes.id, user, function (error, users) {
                if (error) {
                  reject(error)
                } else {
                  self.userService.updateBQId({ Id: JSON.parse(user.custom_data).id, IdQB: users.id }).subscribe(res => { })
                  resolve(users)
                }
              });
            }
          }
        })
        .catch((loginErr) => {
          if (loginErr.status === undefined || loginErr.status !== 401) {

            QB.users.create(user, function (createErr, createRes) {
              if (createErr) {
                console.log('User creation Error : ', createErr);
                reject(createErr);
              } else {
                console.log('User Creation successfull ');
                self.userService.updateBQId({ Id: JSON.parse(user.custom_data).id, IdQB: createRes.id }).subscribe(res => { })
                resolve(createRes);
              }
            });
          }
        });

      // this.qbHelper.qbGetUserByLogin(user).then(res =>{console.log(res)}).catch(err=>{console.log(err)})
    });
  }

  public getRecipientUserId(users) {
    const self = this;
    if (users.length === 2) {
      return users.filter(function (user) {
        if (user !== self.user.id) {
          return user;
        }
      })[0];
    }
  }
}

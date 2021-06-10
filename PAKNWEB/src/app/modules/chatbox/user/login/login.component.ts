import { Component, OnInit, AfterViewInit } from '@angular/core'
import { Router } from '@angular/router'
import { LoginModel } from './loginModel'
import { UserServiceChatBox } from '../user.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { UserService } from 'src/app/services/user.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

@Component({
	selector: 'app-login-chatbox',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css'],
})
export class LoginChatBoxComponent implements OnInit, AfterViewInit {
	public loginModel = new LoginModel()
	public userNameRegExString = '^[a-zA-Z]{1}[\\w]{2,19}$'
	public userLoginRegExString = '^[a-zA-Z]{1}[\\w]{2,19}$'
	userNameFocused = true
	userLoginFocused = true
	private successUnSubscribe$
	userNameinfo: string
	userLoginId: number = this.storeageService.getUserId()

	constructor(private userServiceCB: UserServiceChatBox, private storeageService: UserInfoStorageService, private router: Router, private userService: UserService) {}
	ngOnInit() {
		this.successUnSubscribe$ = this.userServiceCB.successSubject.subscribe((success) => {
			if (success) {
				this.router.navigate(['quan-tri/chatbox'])
			}
		})
	}
	ngAfterViewInit() {
		this.userService.getById({ id: this.userLoginId }).subscribe((res) => {
			console.log(res)
			if (res.success == RESPONSE_STATUS.success) {
				this.userNameinfo = res.result.SYUserGetByID[0].userName
				this.onSubmit()
			}
		})
	}

	params = {
		login: this.loginModel.userLogin,
		password: '12345678',
		full_name: 'TL_CN',
	}

	onSubmit() {
		if (this.userNameFocused && this.userLoginFocused) {
			console.log('Login form passed validation and ready to submit.')

			this.params.login = this.loginModel.userLogin = this.userNameinfo
			this.loginModel.userName = this.userNameinfo
			this.userServiceCB.createUser(this.params)
			this.userServiceCB.login(this.loginModel)
		} else {
			console.log('Login form failed validation')
		}
	}
	onChange() {
		this.userNameFocused = new RegExp(this.userNameRegExString).test(this.loginModel.userName)
		this.userLoginFocused = new RegExp(this.userLoginRegExString).test(this.loginModel.userLogin)
	}
}

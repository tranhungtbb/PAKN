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
	user: any 
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
				this.user = res.result.SYUserGetByID[0]
				console.log(this.user)
				this.onSubmit()
			}
		})
	}

	

	onSubmit() {
		let params= {
			userLogin : this.user.userName,
			userName : this.user.fullName,
			id : this.user.id
		}
		this.userServiceCB.login(params)
	}
}

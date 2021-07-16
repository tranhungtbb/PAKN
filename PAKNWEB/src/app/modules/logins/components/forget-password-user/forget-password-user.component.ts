import { Component, OnInit } from '@angular/core'
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { AuthenticationService } from '../../../../services/authentication.service'
import { ForgetPasswordUserObject } from '../../../../models/forgetPasswordUserObject'
import { ToastrService } from 'ngx-toastr'

@Component({
	selector: 'app-forget-password-user',
	templateUrl: './forget-password-user.component.html',
	styleUrls: ['./forget-password-user.component.css'],
})
export class ForgetPasswordUserComponent implements OnInit {
	submitted: boolean = false
	user: ForgetPasswordUserObject = {
		Email: '',
	}
	forgetPasswordForm: FormGroup
	constructor(
		private authenService: AuthenticationService,
		private _avRoute: ActivatedRoute,
		private _router: Router,
		private toastr: ToastrService,
		private formBuilder: FormBuilder
	) {}

	ngOnInit() {
		this.forgetPasswordForm = this.formBuilder.group({
			email: new FormControl(this.user.Email, [Validators.required]),
		})
	}

	forgetPassword(): void {
		this.submitted = true
		if (this.forgetPasswordForm.invalid) return

		this.authenService.forgetpassword(this.user).subscribe(
			(data) => {
				if (data.result > 0) {
					this.toastr.info("Vui lòng đăng nhập với mật khẩu mới trong email của bạn" ,'',{timeOut:300000})
					// this.toastr.info(
					// 	'message',
					// 	'title',
					// 	{positionClass:'inline',
					// timeOut:500000},
					// );
					// this._router.navigate(['/dang-nhap'])
				} else  {
					this.toastr.error(data.message)
				}
			},
			(error) => {
				console.error(error)
			}
		)
	}
	login(): void {
		this._router.navigate(['/dang-nhap'])
	}
	get f() {
		return this.forgetPasswordForm.controls
	}
}

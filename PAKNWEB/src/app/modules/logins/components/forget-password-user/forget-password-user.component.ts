import { Component, OnInit, Type } from '@angular/core'
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { AuthenticationService } from '../../../../services/authentication.service'
import { ForgetPasswordObject } from '../../../../models/forgetPasswordUserObject'
import { ToastrService } from 'ngx-toastr'
import { TYPE_CONFIG } from 'src/app/constants/CONSTANTS'

@Component({
	selector: 'app-forget-password-user',
	templateUrl: './forget-password-user.component.html',
	styleUrls: ['./forget-password-user.component.css'],
})
export class ForgetPasswordUserComponent implements OnInit {
	submitted: boolean = false
	user: any = new ForgetPasswordObject()
	forgetPasswordForm: FormGroup
	constructor(
		private authenService: AuthenticationService,
		private _router: Router,
		private toastr: ToastrService,
		private formBuilder: FormBuilder
	) { }

	ngOnInit() {
		this.user.isSystem = true
		this.forgetPasswordForm = this.formBuilder.group({
			email: new FormControl(this.user.email, [Validators.required, Validators.email]),
		})
	}

	forgetPassword(): void {
		this.submitted = true
		if (this.forgetPasswordForm.invalid) return

		this.authenService.forgetpassword(this.user).subscribe(
			(data) => {
				if (data.result > 0) {
					this.toastr.info("Vui lòng đăng nhập với mật khẩu mới trong email của bạn")
					setTimeout(() => {
						this._router.navigate(['/dang-nhap'])
					}, 3000);

				} else {
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

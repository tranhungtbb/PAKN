import { Component, OnInit } from '@angular/core'
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { AuthenticationService } from '../../../../services/authentication.service'
import { ForgetPasswordObject } from '../../../../models/forgetPasswordUserObject'
import { ToastrService } from 'ngx-toastr'
declare var $: any
@Component({
	selector: 'app-forget-password',
	templateUrl: './forget-password.component.html',
	styleUrls: ['./forget-password.component.css'],
})
export class ForgetPasswordComponent implements OnInit {
	submitted: boolean = false
	user: ForgetPasswordObject = {
		phone: '',
	}
	phoneHide: any = ''
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
			phone: new FormControl(this.user.phone, [Validators.required, Validators.pattern(/^(84|0[3|5|7|8|9])+([0-9]{8})$/g)]),
		})
	}

	forgetPassword(): void {
		this.submitted = true
		if (this.forgetPasswordForm.invalid) return

		this.authenService.forgetpassword(this.user).subscribe(
			(data) => {
				if (data.status === 1) {
					this._router.navigate(['/dang-nhap'])
				} else if (data.status !== 2) {
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
	//get email() { return this.forgetPasswordForm.get('email'); }
	get f() {
		return this.forgetPasswordForm.controls
	}

	preShowOPT() {
		this.user.phone = this.user.phone.trim()
		if (this.forgetPasswordForm.invalid) {
			this.toastr.error('Dữ liệu không hợp lệ')
			return
		}
		this.phoneHide = this.user.phone
			.split('')
			.map((item, index) => {
				if (index > 3 && index < 7) {
					return '*'
				}
				return item
			})
			.join('')
		$('#modal-otp').modal('show')
	}
}

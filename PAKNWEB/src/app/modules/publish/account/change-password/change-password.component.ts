import { Component, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { FormBuilder, FormControl, FormGroup, Validator, Validators } from '@angular/forms'
import { Router } from '@angular/router'
import { UserInfoStorageService } from '../../../../commons/user-info-storage.service'

import { AccountService } from 'src/app/services/account.service'
import { ChangePwdObject } from 'src/app/models/UserObject'

@Component({
	selector: 'app-change-password',
	templateUrl: './change-password.component.html',
	styleUrls: ['./change-password.component.css'],
})
export class ChangePasswordComponent implements OnInit {
	constructor(private formBuider: FormBuilder, private toast: ToastrService, private accountService: AccountService, private router: Router, private storageService : UserInfoStorageService) {}

	formChangePwd: FormGroup
	model: ChangePwdObject = new ChangePwdObject()

	get f() {
		return this.formChangePwd.controls
	}
	ngOnInit() {
		this.formChangePwd = this.formBuider.group(
			{
				oldPassword: [this.model.oldPassword, [Validators.required]],
				newPassword: [this.model.newPassword, [Validators.required]],
				rePassword: [this.model.rePassword, [Validators.required]],
			},
			{ validator: MustMatch('newPassword', 'rePassword') }
		)
	}

	viewPassword: any = {
		new: false,
		reNew: false,
	}

	submited = false
	onSave(event) {
		this.submited = true
		if (this.formChangePwd.invalid) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}

		this.accountService.changePassword(this.model).subscribe((res) => {
			if (res.success != 'OK') {
				this.toast.error(res.message)
				return
			}
			this.storageService.clear()
			this.toast.success('Đổi mật khẩu thành công')
			this.router.navigate(['/dang-nhap'])
		})
	}
}
function MustMatch(controlName: string, matchingControlName: string) {
	return (formGroup: FormGroup) => {
		const control = formGroup.controls[controlName]
		const matchingControl = formGroup.controls[matchingControlName]

		if (matchingControl.errors && !matchingControl.errors.mustMatch) {
			// return if another validator has already found an error on the matchingControl
			return
		}

		// set error on matchingControl if validation fails
		if (control.value !== matchingControl.value) {
			matchingControl.setErrors({ mustMatch: true })
		} else {
			matchingControl.setErrors(null)
		}
	}
}

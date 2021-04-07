import { Component, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'

import { COMMONS } from 'src/app/commons/commons'
import { IndividualObject } from 'src/app/models/individualObject'

declare var $: any
@Component({
	selector: 'app-individual',
	templateUrl: './individual.component.html',
	styleUrls: ['./individual.component.css'],
})
export class IndividualComponent implements OnInit {
	constructor(private toast: ToastrService, private formBuilder: FormBuilder) {}

	formLogin: FormGroup
	formInfo: FormGroup
	model: IndividualObject = new IndividualObject()

	ngOnInit() {
		this.loadFormBuilder()
	}

	fLoginSubmitted = false
	fInfoSubmitted = false

	get fLogin() {
		return this.formLogin.controls
	}

	get fInfo() {
		return this.formInfo.controls
	}

	public loadFormBuilder() {
		//form thông tin đăng nhập
		this.formLogin = this.formBuilder.group({
			phone: [this.model.phone, [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			password: [this.model.password, [Validators.required, Validators.minLength(6)]],
			rePassword: [this.model.rePassword, [Validators.required]],
		})

		//form thông tin
		this.formInfo = this.formBuilder.group({
			fullName: [this.model.fullName, [Validators.required]],
			nation: [this.model.nation, [Validators.required]],
			province: [this.model.province, [Validators.required]],
			district: [this.model.district, [Validators.required]],
			village: [this.model.village, [Validators.required]],
			gender: [this.model.gender, [Validators.required]],
			email: [this.model.email, [Validators.required]],
			address: [this.model.address, [Validators.required]],
			identity: [this.model.identity, [Validators.required]],
			placeIssue: [this.model.placeIssue, [Validators.required]],
			dateIssue: [this.model.dateIssue, [Validators.required]],
		})
	}
}

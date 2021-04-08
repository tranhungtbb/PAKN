import { Component, OnInit } from '@angular/core'

import { ToastrService } from 'ngx-toastr'
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms'

import { COMMONS } from 'src/app/commons/commons'
declare var $: any

@Component({
	selector: 'app-organization',
	templateUrl: './organization.component.html',
	styleUrls: ['./organization.component.css'],
})
export class OrganizationComponent implements OnInit {
	constructor(
		private toast :ToastrService,
		private formBuilder: FormBuilder) {}

	formLogin: FormGroup
	formInfo: FormGroup

	model:any

	ngOnInit() {}

	listNation:any[]=[
		{id:1,name:"Việt Nam"},
		{id:2,name:"Lào"},
		{id:3,name:"Thái Lan"},
		{id:4,name:"Campuchia"}
	]
	listProvince:any[]=[]
	listDistrict:any[]=[]
	listVillage:any[]=[]

	listGender:any[]=[
		{value:true,text:'Nam'},
		{value:false,text:'Nữ'}
	]

	onSave(){
		this.fLoginSubmitted = true;
		this.fInfoSubmitted = true;
		if(this.formLogin.invalid || this.formInfo.invalid){
			this.toast.error('Dữ liệu không hợp lệ');
			return;
		}

		// req to server

	}


	fLoginSubmitted = false
	fInfoSubmitted = false

	get fLogin() {
		return this.formLogin.controls
	}

	get fInfo() {
		return this.formInfo.controls
	}

	private loadFormBuilder() {
		//form thông tin đăng nhập
		this.formLogin = this.formBuilder.group({
			phone: [this.model.phone, [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			password: [this.model.password, [Validators.required, Validators.minLength(6)]],
			rePassword: [this.model.rePassword, [Validators.required]],
		},{validator: MustMatch('password', 'rePassword')})

		//form thông tin
		this.formInfo = this.formBuilder.group({
			fullName: [this.model.fullName, [Validators.required]],
			gender: [this.model.gender, [Validators.required]],
			odb:[this.model.odb,[Validators.required]],
			nation: [this.model.nation, [Validators.required]],
			province: [this.model.province, [Validators.required]],
			district: [this.model.district, [Validators.required]],
			village: [this.model.village, [Validators.required]],

			email: [this.model.email, [Validators.required]],
			address: [this.model.address, [Validators.required]],
			identity: [this.model.identity, [Validators.required]],
			placeIssue: [this.model.placeIssue, [Validators.required]],
			dateIssue: [this.model.dateIssue, [Validators.required]],
		})
	}
}
function MustMatch(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
        const control = formGroup.controls[controlName];
        const matchingControl = formGroup.controls[matchingControlName];

        if (matchingControl.errors && !matchingControl.errors.mustMatch) {
            // return if another validator has already found an error on the matchingControl
            return;
        }

        // set error on matchingControl if validation fails
        if (control.value !== matchingControl.value) {
            matchingControl.setErrors({ mustMatch: true });
        } else {
            matchingControl.setErrors(null);
        }
    }
}


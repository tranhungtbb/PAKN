import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { viLocale } from 'ngx-bootstrap/locale'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'
import { OrgFormAddressComponent } from './org-form-address/org-form-address.component'
import { OrgRepreFormComponent } from './org-repre-form/org-repre-form.component'
import { RegisterService } from 'src/app/services/register.service'
import { BusinessIndividualService } from 'src/app/services/business-individual.service'
import { COMMONS } from 'src/app/commons/commons'
import { OrganizationObject } from 'src/app/models/businessIndividualObject'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { ActivatedRoute, Router } from '@angular/router'

declare var $: any
@Component({
	selector: 'app-create-upd-business',
	templateUrl: './create-upd-business.component.html',
	styleUrls: ['./create-upd-business.component.css'],
})
export class CreateUpdBusinessComponent implements OnInit {
	@ViewChild(OrgRepreFormComponent, { static: true })
	private child_OrgRepreForm: OrgRepreFormComponent
	@ViewChild(OrgFormAddressComponent, { static: true })
	private child_OrgAddressForm: OrgFormAddressComponent

	constructor(
		private localeService: BsLocaleService,
		private toast: ToastrService,
		private formBuilder: FormBuilder,
		private registerService: RegisterService,
		private businessIndividualService: BusinessIndividualService,
		private router: Router,
		private storeageService: UserInfoStorageService,
		private activatedRoute: ActivatedRoute
	) {
		defineLocale('vi', viLocale)
	}

	dateNow: Date = new Date()

	// formLogin: FormGroup
	formOrgInfo: FormGroup
	listNation: any[] = [{ id: 'Việt Nam', name: 'Việt Nam' }]
	model: OrganizationObject = new OrganizationObject()
	nation_enable_type = false
	userLoginId: number = this.storeageService.getUserId()
	title: string = 'Tạo mới doanh nghiệp'

	ngOnInit() {
		this.localeService.use('vi')

		// set
		this.activatedRoute.params.subscribe((params) => {
			this.model.id = +params['id']
			if (this.model.id != 0) {
				this.getData()
				this.title = 'Cập nhật thông tin'
			} else {
				this.title = 'Tạo mới doanh nghiệp'
			}
		})
		//
		this.child_OrgAddressForm.model = this.model
		this.child_OrgRepreForm.model = this.model
		this.loadFormBuilder()
	}

	serverMsg = {}

	onReset() {
		this.formOrgInfo.reset()
		this.child_OrgRepreForm.formInfo.reset()
		this.child_OrgAddressForm.formOrgAddress.reset()

		this.child_OrgRepreForm.fInfoSubmitted = false
		this.fOrgInfoSubmitted = false
		this.child_OrgAddressForm.fOrgAddressSubmitted = false
		this.model = new OrganizationObject()
		this.model._RepresentativeBirthDay = ''
		this.model._DateOfIssue = ''
		this.model.RepresentativeGender = true
	}

	getData() {
		let request = {
			Id: this.model.id,
		}
		this.businessIndividualService.businessGetByID(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.model = response.result.BusinessGetById[0]
			} else {
				this.toast.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}

	onSave() {
		this.child_OrgRepreForm.fInfoSubmitted = true
		this.fOrgInfoSubmitted = true
		this.child_OrgAddressForm.fOrgAddressSubmitted = true
		let fDob: any = document.querySelector('#_dob')
		let fIsDate: any = document.querySelector('#_IsDate')
		this.model._RepresentativeBirthDay = fDob.value
		this.model._DateOfIssue = fIsDate.value
		this.model.userId = this.userLoginId
		console.log('this.model', this.model)

		if (
			this.checkExists['Phone'] ||
			this.checkExists['BusinessRegistration'] ||
			this.checkExists['DecisionOfEstablishing'] ||
			this.checkExists['Tax'] ||
			this.child_OrgAddressForm.checkExists['OrgEmail'] ||
			this.child_OrgAddressForm.checkExists['OrgPhone'] ||
			this.child_OrgRepreForm.checkExists['Email'] ||
			this.child_OrgRepreForm.checkExists['IDCard']
		) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}

		console.log('this.formOrgInfo', this.formOrgInfo)
		console.log('this.child_OrgRepreForm.formInfo', this.child_OrgRepreForm.formInfo)
		console.log('this.child_OrgAddressForm.formOrgAddress', this.child_OrgAddressForm.formOrgAddress)
		if (this.formOrgInfo.invalid || this.child_OrgRepreForm.formInfo.invalid || this.child_OrgAddressForm.formOrgAddress.invalid) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}

		if (this.model.id != null && this.model.id > 0) {
			this.businessIndividualService.businessUpdate(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					let errorMsg = res.message
					this.toast.error(res.message)
					return
				}
				this.toast.success(COMMONS.UPDATE_SUCCESS)
				this.router.navigate(['/quan-tri/ca-nhan-doanh-nghiep/doanh-nghiep'])
			})
		} else {
			this.businessIndividualService.businessRegister(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					let msg = res.message
					if (msg.includes(`UNIQUE KEY constraint 'UC_SY_User_Email'`)) {
						this.toast.error('Email Người đại diện đã tồn tại')
						return
					}
					if (msg.includes(`UNIQUE KEY constraint 'UK_BI_Business_OrgEmail'`)) {
						this.toast.error('Email Văn phòng đại diện đã tồn tại')
						return
					}
					if (msg.includes(`UNIQUE KEY constraint 'UK_BI_Business_Email'`)) {
						this.toast.error('Email Người đại diện đã tồn tại')
						return
					}
					this.toast.error(msg)
					return
				}
				this.toast.success('Đăng ký doanh nghiệp thành công')
				this.router.navigate(['/quan-tri/ca-nhan-doanh-nghiep/doanh-nghiep'])
			})
		}
	}

	fOrgInfoSubmitted = false
	get fOrgInfo() {
		return this.formOrgInfo.controls
	}

	private loadFormBuilder() {
		this.formOrgInfo = this.formBuilder.group({
			//---thông tin doanh nghiệp
			Business: [this.model.Business, [Validators.required, Validators.maxLength(200)]], // tên tổ chức
			RegistrationNum: [this.model.BusinessRegistration, [Validators.maxLength(20)]], //Số ĐKKD
			DecisionFoundation: [this.model.DecisionOfEstablishing, [Validators.maxLength(20)]], //Quyết định thành lập
			DateIssue: [this.model._DateOfIssue, []], //Ngày cấp/thành lập
			Tax: [this.model.Tax, [Validators.required, Validators.maxLength(13)]], //Mã số thuế
		})
	}
	//kiểm tra dữ liệu đã tồn tại
	checkExists = {
		Phone: false,
		BusinessRegistration: false,
		DecisionOfEstablishing: false,
	}
	onCheckExist(field: string, value: string) {
		if (value == null || value == '') {
			this.checkExists[field] = false
			return
		}
		this.registerService
			.businessCheckExists({
				field,
				value,
				id: 0,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					// if (field == 'Phone') this.phone_exists = res.result.BIBusinessCheckExists[0].exists
					// else if (field == 'BusinessRegistration') this.busiRegis_exists = res.result.BIBusinessCheckExists[0].exists
					// else if (field == 'DecisionOfEstablishing') this.busiDeci_exists = res.result.BIBusinessCheckExists[0].exists
					this.checkExists[field] = res.result.BIBusinessCheckExists[0].exists
				}
			})
	}
}

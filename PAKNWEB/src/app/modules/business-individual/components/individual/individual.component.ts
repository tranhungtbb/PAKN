import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { BusinessIndividualService } from 'src/app/services/business-individual.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
import { IndividualObject } from 'src/app/models/businessIndividualObject'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'
import { viLocale } from 'ngx-bootstrap/locale'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { DiadanhService } from 'src/app/services/diadanh.service'
import { RegisterService } from 'src/app/services/register.service'

declare var $: any
@Component({
	selector: 'app-individual',
	templateUrl: './individual.component.html',
	styleUrls: ['./individual.component.css'],
})
export class IndividualComponent implements OnInit {
	constructor(
		private _service: BusinessIndividualService,
		private storeageService: UserInfoStorageService,
		private _fb: FormBuilder,
		private _toastr: ToastrService,
		private _router: Router,
		private _shareData: DataService,
		private localeService: BsLocaleService,
		private diadanhService: DiadanhService,
		private registerService: RegisterService
	) {
		defineLocale('vi', viLocale)
	}

	dateNow: Date = new Date()

	listNation: any[] = [{ id: 'Việt Nam', name: 'Việt Nam' }]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []

	userLoginId: number = this.storeageService.getUserId()
	unitLoginId: number = this.storeageService.getUnitId()
	listData = new Array<IndividualObject>()
	listStatus: any = [
		{ value: '', text: 'Chọn trạng thái' },
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]

	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	form: FormGroup
	model: any = new IndividualObject()
	submitted: boolean = false
	isActived: boolean
	title: string = ''
	fullName: string = ''
	address: string = ''
	phone: string = ''
	email: string = ''
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0

	//sort
	individualSortDir = 'DESC'
	individualSortField = 'ID'

	inSortDir = 'DESC'
	inSortField = 'ID'

	nation_enable_type = false

	ngOnInit() {
		this.buildForm()
		this.getList()
		this.localeService.use('vi')
		this.loadFormBuilder()
		this.onChangeNation()
	}

	//event
	onChangeNation() {
		this.listProvince = []
		this.listDistrict = []
		this.listVillage = []

		this.model.provinceId = ''

		if (this.model.nation == 'Việt Nam') {
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll
					this.model.provinceId = 37
				}
			})
		} else {
			if (this.model.nation == '#') {
				this.nation_enable_type = true
				this.model.nation = ''
			}
		}
	}

	onChangeProvince() {
		this.listDistrict = []
		this.listVillage = []

		this.model.districtId = ''
		this.model.wardsId = ''
		if (this.model.provinceId != null && this.model.provinceId != '') {
			this.diadanhService.getAllDistrict(this.model.provinceId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listDistrict = res.result.CADistrictGetAll
				}
			})
		} else {
		}
	}

	onChangeDistrict() {
		this.listVillage = []

		this.model.wardsId = ''
		if (this.model.districtId != null && this.model.districtId != '') {
			this.diadanhService.getAllVillage(this.model.provinceId, this.model.districtId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listVillage = res.result.CAVillageGetAll
				}
			})
		} else {
		}
	}

	onReset() {
		this.form.reset()
		this.submitted = false
		this.model = new IndividualObject()

		this.model._birthDay = ''
		this.model._dateOfIssue = ''
		this.model.fullName = ''
		this.model.gender = true
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	buildForm() {
		this.form = this._fb.group({
			fullName: [this.model.fullName, Validators.required],
			address: [this.model.address, Validators.required],
			phone: [this.model.phone, Validators.required],
			email: [this.model.email, Validators.required],
			isActived: [this.model.isActived, Validators.required],
		})
	}

	rebuilForm() {
		this.form.reset({
			fullName: this.model.fullName,
			address: this.model.address,
			phone: this.model.phone,
			email: this.model.email,
			isActived: this.model.isActived,
		})

		this.submitted = false
		this.model = new IndividualObject()
		this.model._birthDay = ''
		this.model._dateOfIssue = ''
		this.model.fullName = ''
		this.model.email = ''
		this.model.gender = true
	}
	onSortUser(fieldName: string) {
		this.inSortDir = fieldName
		this.inSortField = this.inSortField == 'DESC' ? 'ASC' : 'DESC'
		this.getList()
	}

	getList() {
		this.fullName = this.fullName.trim()
		this.address = this.address.trim()
		this.phone = this.phone.trim()
		this.email = this.email.trim()

		let request = {
			FullName: this.fullName,
			Address: this.address,
			Phone: this.phone,
			Email: this.email,
			isActived: this.isActived != null ? this.isActived : '',
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
			sortDir: this.inSortDir,
			sortField: this.inSortField,
		}
		this._service.individualGetList(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.IndividualGetAllOnPage
					this.totalRecords = response.result.IndividualGetAllOnPage.length != 0 ? response.result.IndividualGetAllOnPage[0].rowNumber : 0
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	dataUpdate: any
	preUpdateStatus(data) {
		this.dataUpdate = data
		$('#modalConfirmUpdateStatus').modal('show')
	}

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}

	dataStateChange() {
		this.pageIndex = 1
		this.table.first = 0
		this.getList()
	}

	changeState(event: any) {
		if (event) {
			if (event.target.value == 'null') {
				this.isActived = null
			} else {
				this.isActived = event.target.value
			}
			this.pageIndex = 1
			this.pageSize = 20
			this.getList()
		}
	}

	changeType(event: any) {
		if (event) {
			this.pageIndex = 1
			this.pageSize = 20
			this.getList()
		}
	}

	preCreate() {
		this.model = new IndividualObject()
		this.rebuilForm()
		this.submitted = false
		this.title = 'Thêm mới cá nhân'
		$('#modal').modal('show')
	}

	get f() {
		return this.form.controls
	}

	private loadFormBuilder() {
		//form createIndividualForm
		this.form = this._fb.group({
			fullName: [this.model.fullName, [Validators.required, Validators.maxLength(100)]],
			gender: [this.model.gender, [Validators.required]],
			dob: [this.model._birthDay, [Validators.required]],
			nation: [this.model.nation, [Validators.required]],
			province: [this.model.provinceId, []],
			district: [this.model.districtId, []],
			village: [this.model.wardsId, []],
			phone: [this.model.phone, [Validators.required, Validators.pattern(/^(84|0[3|5|7|8|9])+([0-9]{8})$/g)]],

			email: [this.model.email, [Validators.email]],
			address: [this.model.address, [Validators.required]],
			iDCard: [this.model.iDCard, [Validators.required]], //, Validators.pattern(/^([0-9]){8,12}$/g)
			issuedPlace: [this.model.issuedPlace, []],
			dateIssue: [this.model._dateOfIssue, []],
			isActived: [this.model.isActived],
		})
	}

	onSave() {
		this.submitted = true

		let fDob: any = document.querySelector('#_dob')
		let fDateIssue: any = document.querySelector('#_dateIssue')
		this.model._birthDay = fDob.value
		this.model._dateOfIssue = fDateIssue.value
		if (!this.model.email) this.model.email = ''

		if (this.email_exists || this.phone_exists || this.idCard_exists) {
			this._toastr.error('Dữ liệu không hợp lệ')
			return
		}

		// console.log('form', this.form.invalid)
		// if (this.form.invalid) {
		// 	console.log('invalid 1')
		// 	this._toastr.error('Dữ liệu không hợp lệ')
		// 	return
		// }

		//check ngày cấp < ngày sinh
		let dateIssue = new Date(this.model._dateOfIssue)
		let dateOfBirth = new Date(this.model._birthDay)

		if (dateIssue < dateOfBirth) {
			this._toastr.error('Ngày cấp phải lớn hơn ngày sinh')
			return
		}

		// req to server
		this.registerService.registerIndividual(this.model).subscribe((res) => {
			if (res.success != 'OK') {
				let msg = res.message
				if (msg.includes(`UNIQUE KEY constraint 'UC_SY_User_Email'`)) {
					this._toastr.error('Email đã tồn tại')
				}

				this._toastr.error(msg)
				return
			}
			this._toastr.success('Đăng ký cá nhân thành công')
		})
	}

	// server exists
	phone_exists: boolean = false
	email_exists: boolean = false
	idCard_exists: boolean = false
	onCheckExist(field: string, value: string) {
		this.registerService
			.individualCheckExists({
				field,
				value,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					if (field == 'Phone') this.phone_exists = res.result.BIIndividualCheckExists[0].exists
					else if (field == 'Email') this.email_exists = res.result.BIIndividualCheckExists[0].exists
					else if (field == 'IDCard') this.idCard_exists = res.result.BIIndividualCheckExists[0].exists
				}
			})
	}
}

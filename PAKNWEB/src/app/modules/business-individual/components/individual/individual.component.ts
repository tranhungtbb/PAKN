import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { BusinessIndividualService } from 'src/app/services/business-individual.service'
import { DataService } from 'src/app/services/sharedata.service'
import { RESPONSE_STATUS, FILETYPE, CONSTANTS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
import { IndividualObject, IndividualExportObject } from 'src/app/models/businessIndividualObject'
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
	data: [][]
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
	allowExcelExtend = ['xlsx', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet']
	dateNow: Date = new Date()

	listNation: any[] = [{ id: 'Việt Nam', name: 'Việt Nam' }]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []

	userLoginId: number = this.storeageService.getUserId()
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
	fileAccept = '.xls, .xlsx'
	files: any[] = []
	listInvPaged: any[] = []

	form: FormGroup
	model: any = new IndividualObject()
	submitted: boolean = false
	isActived: boolean = false
	title: string = ''
	fullName: string = ''
	address: string = ''
	phone: string = ''
	email: string = ''
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	dataSearch: IndividualExportObject = new IndividualExportObject()

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
		this.dataSearch.fullName = this.dataSearch.fullName.trim()
		this.dataSearch.address = this.dataSearch.address.trim()
		this.dataSearch.phone = this.dataSearch.phone.trim()
		this.dataSearch.email = this.dataSearch.email.trim()

		let request = {
			FullName: this.dataSearch.fullName,
			Address: this.dataSearch.address,
			Phone: this.dataSearch.phone,
			Email: this.dataSearch.email,
			isActived: this.isActived != null ? this.isActived : true,
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
					this.listInvPaged = response.result.IndividualGetAllOnPage
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

	/*start - chức năng xác nhận hành động xóa*/
	modalConfirm_type = 'isActived'
	modelConfirm_itemId: number = 0
	onOpenConfirmModal(id: any, type = 'isActived') {
		$('#modal-confirm').modal('show')
		this.modalConfirm_type = type
		this.modelConfirm_itemId = id
	}
	acceptConfirm() {
		if (this.modalConfirm_type == 'isActived') {
			this.onChangeIndividualStatus(this.modelConfirm_itemId)
		} else if (this.modalConfirm_type == 'individual') {
			this.onDeleteIndividual(this.modelConfirm_itemId)
		}

		$('#modal-confirm').modal('hide')
	}
	onChangeIndividualStatus(id: number) {
		let item = this.listInvPaged.find((c) => c.id == id)
		item.isActived = !item.isActived
		this._service.individualChangeStatus(item).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(COMMONS.UPDATE_FAILED)
				item.isActived = !item.isActived
				return
			}
			this._toastr.success(COMMONS.UPDATE_SUCCESS)
			this.getList()
			this.model = new IndividualObject()
		})
	}
	/*end - chức năng xác nhận hành động xóa*/
	onDeleteIndividual(id) {
		let item = this.listInvPaged.find((c) => c.id == id)
		if (!item) item = this.model
		this._service.individualDelete(item).subscribe((res) => {
			if (res.success != 'OK') {
				if (res.message.includes(`REFERENCE constraint "PK_BI_Individual"`)) {
					this._toastr.error(COMMONS.DELETE_FAILED + ', Người dùng đã được xóa')
					return
				}
				this.getList()
				this._toastr.error(res.message)
				return
			}
			this._toastr.success(COMMONS.DELETE_SUCCESS)

			if (this.model.id == id) {
				this.getList()
			}
		})
	}
	/*end - chức năng xác nhận hành động xóa*/

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
		//form model
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
		this.model.userId = this.userLoginId
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

		if (this.model.id != null && this.model.id > 0) {
			this._service.invididualUpdate(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					let errorMsg = res.message
					this._toastr.error(res.message)
					return
				}
				this._toastr.success(COMMONS.UPDATE_SUCCESS)
				this.model = new IndividualObject()
				$('#modal').modal('hide')
				this.getList()
			})
		} else {
			// individualRegister
			this._service.individualRegister(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					let msg = res.message
					if (msg.includes(`UNIQUE KEY constraint 'UC_SY_User_Email'`)) {
						this._toastr.error('Email đã tồn tại')
					}
					this._toastr.error(msg)
					return
				}
				this._toastr.success(COMMONS.ADD_SUCCESS)
				this.model = new IndividualObject()
				$('#modal').modal('hide')
				this.getList()
			})
		}
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

	preUpdate(data) {
		let request = {
			Id: data.id,
			Type: 1,
		}
		this._service.individualById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.rebuilForm()
				this.title = 'Chỉnh sửa cá nhân'
				this.model = response.result.InvididualGetByID[0]
				$('#modal').modal('show')
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
				alert(error)
			}
	}

	onExport() {
		let passingObj: any = {}
		passingObj = this.dataSearch
		passingObj.TitleReport = 'DANH SÁCH CÁ NHÂN'
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'BI_Individual_List?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}

	onExcelfileChange(event: any) {
		let obj: any = {}
		var file = event.target.files[0]
		if (!this.allowExcelExtend.includes(file.type)) {
			this._toastr.error('Chỉ chọn tệp excel')
			return
		}

		let formData = new FormData()
		formData.append('file', file, file.name)

		this._service.invididualImportFile(formData).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error('Xảy ra lỗi trong quá trình xử lý')
				return
			}
			this.model.imagePath = res.result.path
		})
	}
	onChangeFileExcel() {
		$('#excel-file').click()
	}
}

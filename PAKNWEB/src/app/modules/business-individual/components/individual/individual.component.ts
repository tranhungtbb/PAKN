import { Component, DebugElement, OnInit, ViewChild, ElementRef } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { saveAs as importedSaveAs } from 'file-saver'

import { UploadFileService } from 'src/app/services/uploadfiles.service'
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
import { PathSampleFiles } from 'src/app/constants/CONSTANTS'

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
		private _filesService: UploadFileService,
		private diadanhService: DiadanhService // private registerService: RegisterService
	) {
		defineLocale('vi', viLocale)
	}

	@ViewChild('file', { static: false }) public file: ElementRef
	allowExcelExtend = ['xlsx', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet']
	dateNow: Date = new Date()

	listNation: any[] = [{ id: 'Việt Nam', name: 'Việt Nam' }]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []
	listAllDiaDanh: any[] = []
	titleAccess : any = ''
	userLoginId: number = this.storeageService.getUserId()
	listData = new Array<IndividualObject>()
	listStatus: any = [
		{ value: 1, text: 'Hiệu lực' },
		{ value: 0, text: 'Hết hiệu lực' },
	]

	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	fileAccept = '.xls, .xlsx'
	listInvPaged: any[] = []

	form: FormGroup
	model: IndividualObject = new IndividualObject()
	modelDetail: IndividualObject = new IndividualObject()
	submitted: boolean = false
	title: string = ''
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	dataSearch: IndividualExportObject = new IndividualExportObject()

	//sort
	inSortDir = 'DESC'
	inSortField = 'ID'

	isOtherNation = false

	ngOnInit() {
		// this.buildForm()
		this.getList()
		this.localeService.use('vi')
		this.loadFormBuilder()
		this.onChangeNation()

		let dataRecommendation = this.storeageService.getRecommentdationObjectRemember()
		if(dataRecommendation){
			$('#modal').modal('show')
		}
	}

	//event
	backToSelectBox() {
		this.isOtherNation = false
		this.model.nation = 'Việt Nam'
		this.onChangeNation()

		this.model.provinceId = null
		this.model.districtId = null
		this.model.wardsId = null
	}
	onChangeNation() {
		this.listProvince = []
		this.listDistrict = []
		this.listVillage = []

		if (this.model.nation == 'Việt Nam') {
			this.isOtherNation = false
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll
					this.model.provinceId = 37
					this.onChangeProvince()
				}
			})
		} else {
			if (this.model.nation == '#') {
				this.isOtherNation = true
				this.model.nation = ''

				this.model.provinceId = 0
				this.model.districtId = 0
				this.model.wardsId = 0

				// this.form.get('nation').setErrors(null)
			}
		}
	}
	onResetNationValue(event: any) {
		console.log(event)
		if (event.target.value == 'Nhập...') {
			event.target.value = ''
		}
	}

	onChangeCapTinh() {
		this.listDistrict = []
		this.listVillage = []
		this.model.districtId = null
		this.model.wardsId = null
		if (this.model.provinceId != null && this.model.provinceId != '') {
			this.diadanhService.getAllByProvinceId(this.model.provinceId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listAllDiaDanh = res.result.ListData
				}
			})
		}
	}

	onChangeProvince(tryLoad = false) {
		this.listDistrict = []
		this.listVillage = []
		this.model.districtId = null
		this.model.wardsId = null
		if (tryLoad || (this.model.provinceId != null && this.model.provinceId != '')) {
			this.diadanhService.getAllDistrict(this.model.provinceId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listDistrict = res.result.CADistrictGetAll
				}
			})
		}
	}

	onChangeDistrict(tryLoad = false) {
		this.listVillage = []
		this.model.wardsId = null
		if (tryLoad || (this.model.districtId != null && this.model.districtId != '')) {
			this.diadanhService.getAllVillage(this.model.provinceId, this.model.districtId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listVillage = res.result.CAVillageGetAll
				}
			})
		}
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
		// $('#modal').on('hidden.bs.modal', function () {
			
		// });
	}

	close =()=>{
		let dataRecommendation = this.storeageService.getRecommentdationObjectRemember()
				if(dataRecommendation){
					this._router.navigate(['/quan-tri/kien-nghi/them-moi/0/1'])
					return
				}
				return
	}

	onSortIndividual(fieldName: string) {
		this.inSortDir = this.inSortDir == 'DESC' ? 'ASC' : 'DESC'
		this.inSortField = fieldName
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
			Status: this.dataSearch.status != null ? this.dataSearch.status : '',
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
	modalConfirm_type = 'Status'
	modelConfirm_itemId: number = 0
	onOpenConfirmModal(id: any, type = 'Status') {
		if(type == 'Status'){
			this.titleAccess = 'Anh/Chị có chắc chắn muốn thay đổi trạng thái của người dân này?'
		}
		else{
			this.titleAccess = 'Anh/Chị có chắc chắn muốn xóa người dân này?'
		}
		this.modalConfirm_type = type
		this.modelConfirm_itemId = id
		$('#modal-confirm').modal('show')
	}
	acceptConfirm() {
		if (this.modalConfirm_type == 'Status') {
			this.onChangeIndividualStatus(this.modelConfirm_itemId)
		} else if (this.modalConfirm_type == 'Individual') {
			this.onDeleteIndividual(this.modelConfirm_itemId)
		}

		$('#modal-confirm').modal('hide')
	}
	onChangeIndividualStatus(id: number) {
		let item = this.listInvPaged.find((c) => c.id == id)
		item.status = item.status ? 0 : 1
		this._service.individualChangeStatus(item).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(COMMONS.UPDATE_FAILED)
				item.status = item.status ? 0 : 1
				return
			}
			this._toastr.success(COMMONS.UPDATE_SUCCESS)
			this.getList()
			this.model = new IndividualObject()
		})
	}
	/*end - chức năng xác nhận hành động xóa*/
	onDeleteIndividual(id) {
		this._service.individualDelete({ Id: id }).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				if (res.message.includes(`REFERENCE constraint "PK_BI_Individual"`)) {
					this._toastr.error(COMMONS.DELETE_FAILED)
					return
				}
				this.getList()
				this._toastr.error(res.message)
				return
			} else {
				if (res.result > 0) {
					this._toastr.success(COMMONS.DELETE_SUCCESS)
					this.getList()
				} else {
					this._toastr.error('Không thể xóa cá nhân đã trong 1 quy trình')
					// this.getList()
				}
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
		this.model.nation = 'Việt Nam'
		this.model.provinceId = 37 // Tỉnh Khánh Hòa
		this.model.gender = true // Giới tính Nam
		this.model.status = 1 // Hiệu lực
		this.submitted = false
		this.rebuidForm()

		if (this.isOtherNation) {
			this.onChangeNation()
			this.isOtherNation = false
		}

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
			birthDate: [this.model.birthDate, [Validators.required]],
			nation: [this.model.nation, [Validators.required]],
			province: [this.model.provinceId, [Validators.required]],
			district: [this.model.districtId, [Validators.required]],
			village: [this.model.wardsId, [Validators.required]],
			phone: [this.model.phone, [Validators.required]],

			email: [this.model.email, [Validators.email]],
			address: [this.model.address, [Validators.required]],
			iDCard: [this.model.iDCard, [Validators.required]], //, Validators.pattern(/^([0-9]){8,12}$/g)
			placeIssue: [this.model.issuedPlace],
			dateIssue: [this.model.dateOfIssue],
			status: [this.model.status, [Validators.required]],
		})
	}

	rebuidForm() {
		this.form.reset({
			fullName: this.model.fullName,
			gender: this.model.gender,
			birthDate: this.model.birthDate,
			nation: this.model.nation,
			province: this.model.provinceId,
			district: this.model.districtId,
			village: this.model.wardsId,
			phone: this.model.phone,
			email: this.model.email,
			address: this.model.address,
			iDCard: this.model.iDCard,
			placeIssue: this.model.issuedPlace,
			dateIssue: this.model.dateOfIssue,
			status: this.model.status,
		})
	}

	onSave() {
		this.submitted = true

		let fDob: any = document.querySelector('#_dob')
		let fDateIssue: any = document.querySelector('#_dateIssue')
		if (this.model.nation == 'Nhập...') {
			this.model.nation = ''
		}
		// if (this.isOtherNation) {
		// 	this.model.provinceId = 0
		// 	this.model.districtId = 0
		// 	this.model.wardsId = 0
		// }

		// this.model.birthDate = fDob.value
		// this.model.dateOfIssue = fDateIssue.value
		this.model._birthDay = fDob.value
		this.model._dateOfIssue = fDateIssue.value
		this.model.userId = this.userLoginId
		if(!this.model.issuedPlace) this.model.issuedPlace = ''	
		this.model.issuedPlace = this.model.issuedPlace.trim()
		if (!this.model.email) this.model.email = ''

		if (this.email_exists || this.phone_exists || this.idCard_exists) {
			//this._toastr.error('Dữ liệu không hợp lệ')
			return
		}

		if (this.form.invalid) {
			//this._toastr.error('Dữ liệu không hợp lệ')
			return
		}

		//check ngày cấp < ngày sinh
		let dateIssue = new Date(this.model.dateOfIssue)
		let dateOfBirth = new Date(this.model.birthDate)

		if (this.model.dateOfIssue && dateIssue < dateOfBirth) {
			this._toastr.error('Ngày cấp phải lớn hơn ngày sinh')
			return
		}

		// if (this.isOtherNation) {
		// 	this.model.provinceId = ''
		// 	this.model.districtId = ''
		// 	this.model.wardsId = ''
		// }

		if (this.model.id != null && this.model.id > 0) {
			this._service.invididualUpdate(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					this._toastr.error(res.message)

					return
				}
				this._toastr.success(COMMONS.UPDATE_SUCCESS)
				// this.model = new IndividualObject()
				// this.rebuidForm()
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
				// this.model = new IndividualObject()
				$('#modal').modal('hide')
				let dataRecommendation = this.storeageService.getRecommentdationObjectRemember()
				if(dataRecommendation){
					this._router.navigate(['/quan-tri/kien-nghi/them-moi/0'])
				}else{
					this.getList()
				}
			})
		}
	}

	// server exists
	phone_exists: boolean = false
	email_exists: boolean = false
	idCard_exists: boolean = false
	onCheckExist(field: string, value: string) {
		let id = this.model.id != null || undefined ? this.model.id : 0
		this._service
			.individualCheckExists({
				field,
				value,
				id,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					if (field == 'Phone') this.phone_exists = res.result.BIInvididualCheckExists[0].exists
					else if (field == 'Email') this.email_exists = res.result.BIInvididualCheckExists[0].exists
					else if (field == 'IDCard') this.idCard_exists = res.result.BIInvididualCheckExists[0].exists
				}
			})
	}

	preView(id: any) {
		this._service.individualById({ Id: id }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.InvididualGetByID.length > 0) {
					this.modelDetail = res.result.InvididualGetByID[0]
					this.modelDetail.iDCard = res.result.InvididualGetByID[0].idCard
					// console.log(res.result.InvididualGetByID[0])
					$('#modalDetail').modal('show')
				}
			}
		})
	}

	preUpdate(data) {
		let request = {
			Id: data.id,
			Type: 1,
		}
		// this.phone_exists = false
		// this.email_exists = false
		// this.idCard_exists = false
		this.submitted = false
		this.isOtherNation = false
		this._service.individualById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.title = 'Chỉnh sửa cá nhân'
				this.model = response.result.InvididualGetByID[0]
				if (this.model.nation != this.listNation[0].id) {
					this.isOtherNation = true
					this.model.provinceId = 0
					this.model.districtId = 0
					this.model.wardsId = 0
				}

				this.model.iDCard = response.result.InvididualGetByID[0].idCard
				this.model.birthDate = new Date(response.result.InvididualGetByID[0].birthDate)
				if (this.model.dateOfIssue != null) {
					this.model.dateOfIssue = new Date(response.result.InvididualGetByID[0].dateOfIssue)
				}
				this.getProvince()
				this.getDistrict(response.result.InvididualGetByID[0].provinceId)
				this.getVillage(response.result.InvididualGetByID[0].provinceId, response.result.InvididualGetByID[0].districtId)
				this.rebuidForm()
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

	getProvince() {
		this.diadanhService.getAllProvince().subscribe((res) => {
			if (res.success == 'OK') {
				this.listProvince = res.result.CAProvinceGetAll
			}
		})
	}

	getDistrict(provinceId) {
		if (provinceId != null && provinceId != '') {
			this.diadanhService.getAllDistrict(provinceId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listDistrict = res.result.CADistrictGetAll
				}
			})
		}
	}

	getVillage(provinceId, districtId) {
		if (districtId != null && districtId != '') {
			this.diadanhService.getAllVillage(provinceId, districtId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listVillage = res.result.CAVillageGetAll
				}
			})
		}
	}

	onExport() {
		let passingObj: any = {}
		passingObj = this.dataSearch
		passingObj.TitleReport = 'DANH SÁCH CÁ NHÂN'
		passingObj.UserProcessId = this.storeageService.getUserId()
		passingObj.UserProcessName = this.storeageService.getFullName()
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
			if (res.success != RESPONSE_STATUS.success) {
				this._toastr.error('Xảy ra lỗi trong quá trình xử lý')
				return
			} else {
				if (res.result.CountSuccess > 0) {
					this._toastr.success('Thêm thành công ' + res.result.CountSuccess + ' người dùng')
				}

				if (res.result.CountError > 0) {
					setTimeout(() => {
						this._toastr.error('Thêm thất bại ' + res.result.CountError + ' người dùng')
					}, 1000)
				}
				this.getList()
			}
			this.file.nativeElement.value = ''
		})
	}
	onChangeFileExcel() {
		$('#excel-file').click()
	}
	onDownFileExcel() {
		$('#sampleFilesIndividual').attr('src', PathSampleFiles.PathSampleFilesIndividual)
	}
}
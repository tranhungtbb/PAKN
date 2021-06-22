import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { BusinessIndividualService } from 'src/app/services/business-individual.service'
import { DataService } from 'src/app/services/sharedata.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { COMMONS } from 'src/app/commons/commons'
import { Router } from '@angular/router'
import { BusinessionObject, BusinessExportObject } from 'src/app/models/businessIndividualObject'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'
import { viLocale } from 'ngx-bootstrap/locale'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { DiadanhService } from 'src/app/services/diadanh.service'
import { PathSampleFiles } from 'src/app/constants/CONSTANTS'

declare var $: any
@Component({
	selector: 'app-business',
	templateUrl: './business.component.html',
	styleUrls: ['./business.component.css'],
})
export class BusinessComponent implements OnInit {
	constructor(
		private _service: BusinessIndividualService,
		private storeageService: UserInfoStorageService,
		private _fb: FormBuilder,
		private _toastr: ToastrService,
		private _router: Router,
		private _shareData: DataService,
		private localeService: BsLocaleService,
		private diadanhService: DiadanhService
	) {
		defineLocale('vi', viLocale)
		this.modelDetail = {}
	}

	dateNow: Date = new Date()
	@ViewChild('file', { static: false }) public file: ElementRef

	listNation: any[] = [{ id: 'Việt Nam', name: 'Việt Nam' }]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []
	titleAccess : any = ''
	userLoginId: number = this.storeageService.getUserId()
	unitLoginId: number = this.storeageService.getUnitId()
	listData = new Array<BusinessionObject>()
	listStatus: any = [
		{ value: 0, text: 'Hết hiệu lực' },
		{ value: 1, text: 'Hiệu lực' },
	]

	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	listInvPaged: any[] = []

	form: FormGroup
	model: any = new BusinessionObject()
	modelDetail: any
	submitted: boolean = false
	isActived: boolean
	title: string = ''
	representativeName: string = ''
	address: string = ''
	phone: string = ''
	email: string = ''
	pageIndex: number = 1
	pageSize: number = 20
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	dataSearch: BusinessExportObject = new BusinessExportObject()
	allowExcelExtend = ['xlsx', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet']

	//sort
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

	buildForm() {
		this.form = this._fb.group({
			representativeName: [this.model.representativeName, Validators.required],
			address: [this.model.address, Validators.required],
			phone: [this.model.phone, Validators.required],
			email: [this.model.email, Validators.required],
			isActived: [this.model.isActived, Validators.required],
		})
	}
	onSortBusiness(fieldName: string) {
		this.inSortDir = this.inSortDir == 'DESC' ? 'ASC' : 'DESC'
		this.inSortField = fieldName
		this.getList()
	}
	getList() {
		this.dataSearch.representativeName = this.dataSearch.representativeName.trim()
		this.dataSearch.address = this.dataSearch.address.trim()
		this.dataSearch.phone = this.dataSearch.phone.trim()
		this.dataSearch.email = this.dataSearch.email.trim()

		let request = {
			RepresentativeName: this.dataSearch.representativeName,
			Address: this.dataSearch.address,
			Phone: this.dataSearch.phone,
			Email: this.dataSearch.email,
			Status: this.dataSearch.status != null ? this.dataSearch.status : '',
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
			sortDir: this.inSortDir,
			sortField: this.inSortField,
		}
		this._service.businessGetList(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.BusinessGetAllOnPageBase
					this.listInvPaged = response.result.BusinessGetAllOnPageBase
					this.totalRecords = response.result.BusinessGetAllOnPageBase.length != 0 ? response.result.BusinessGetAllOnPageBase[0].rowNumber : 0
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

	private loadFormBuilder() {
		//form createIndividualForm
		this.form = this._fb.group({
			representativeName: [this.model.representativeName, [Validators.required, Validators.maxLength(100)]],
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

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
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

	/*start - chức năng xác nhận hành động xóa*/
	modalConfirm_type = 'Status'
	modelConfirm_itemId: number = 0
	onOpenConfirmModal(id: any, type = 'Status') {
		if(type == 'Status'){
			this.titleAccess = 'Anh/Chị có chắc chắn muốn thay đổi trạng thái của doanh nghiệp này?'
		}
		else{
			this.titleAccess = 'Anh/Chị có chắc chắn muốn xóa doanh nghiệp này?'
		}
		$('#modal-confirm').modal('show')
		this.modalConfirm_type = type
		this.modelConfirm_itemId = id
	}
	acceptConfirm() {
		if (this.modalConfirm_type == 'Status') {
			this.onChangeBusinessChangeStatus(this.modelConfirm_itemId)
		} else if (this.modalConfirm_type == 'Individual') {
			this.onDeleteBusiness(this.modelConfirm_itemId)
		}

		$('#modal-confirm').modal('hide')
	}
	onChangeBusinessChangeStatus(id: number) {
		let item = this.listInvPaged.find((c) => c.id == id)
		item.status = item.status ? 0 : 1
		this._service.businessChangeStatus(item).subscribe((res) => {
			if (res.success != 'OK') {
				this._toastr.error(COMMONS.UPDATE_FAILED)
				item.status = item.status ? 0 : 1
				return
			}
			this._toastr.success(COMMONS.UPDATE_SUCCESS)
			this.getList()
			this.model = new BusinessionObject()
		})
	}

	preView(id: any) {
		this._service.businessGetByID({ Id: id }).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.modelDetail = response.result.BusinessGetById[0]
				// console.log(this.modelDetail)
				$('#modalDetail').modal('show')
			}
		})
	}

	/*start - chức năng xác nhận hành động xóa*/
	onDeleteBusiness(id) {
		let item = this.listInvPaged.find((c) => c.id == id)
		if (!item) item = this.model
		this._service.businessDelete(item).subscribe((res) => {
			if (res.success != 'OK') {
				if (res.message.includes(`REFERENCE constraint "PK_BI_Business"`)) {
					this._toastr.error(COMMONS.DELETE_FAILED)
					return
				}
				this._toastr.error(res.message)
				return
			}
			if (res.result > 0) {
				this._toastr.success(COMMONS.DELETE_SUCCESS)
				this.getList()
			} else {
				this._toastr.error('Không thể xóa doanh nghiệp đã trong 1 quy trình')
				// this.getList()
			}
		})
	}
	/*end - chức năng xác nhận hành động xóa*/

	rebuilForm() {
		this.form.reset({
			fullName: this.model.fullName,
			address: this.model.address,
			phone: this.model.phone,
			email: this.model.email,
			isActived: this.model.isActived,
		})
	}

	dataStateChange() {
		this.pageIndex = 1
		this.table.first = 0
		this.getList()
	}

	onExport() {
		let passingObj: any = {}
		passingObj = this.dataSearch
		passingObj.TitleReport = 'DANH SÁCH DOANH NGHIỆP'
		this._shareData.setobjectsearch(passingObj)
		this._shareData.sendReportUrl = 'BI_Business_List?' + JSON.stringify(passingObj)
		this._router.navigate(['quan-tri/xuat-file'])
	}

	onExcelfileChange(event: any) {
		var file = event.target.files[0]
		if (!this.allowExcelExtend.includes(file.type)) {
			this._toastr.error('Chỉ chọn tệp excel')
			return
		}

		let formData = new FormData()
		formData.append('file', file, file.name)

		this._service.businessImportFile(formData).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				this._toastr.error('Xảy ra lỗi trong quá trình xử lý')
				return
			} else {
				if (res.result.CountSuccess > 0) {
					this._toastr.success('Thêm thành công ' + res.result.CountSuccess + ' doanh nghiệp')
				}

				if (res.result.CountError > 0) {
					setTimeout(() => {
						this._toastr.error('Thêm không thành công ' + res.result.CountError + ' doanh nghiệp')
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
		$('#sampleFilesBusiness').attr('src', PathSampleFiles.PathSampleFilesBusiness)
	}
}

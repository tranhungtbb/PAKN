import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { FieldObject } from 'src/app/models/fieldObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any
@Component({
	selector: 'app-group-word',
	templateUrl: './group-word.component.html',
	styleUrls: ['./group-word.component.css'],
})
export class GroupWordComponent implements OnInit {
	constructor(private _service: CatalogService, private _toastr: ToastrService, private _fb: FormBuilder, private _shareData: DataService) {}

	listData = new Array<FieldObject>()
	listWordByGroupId : any 
	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	form: FormGroup
	model: any = new FieldObject()
	submitted: boolean = false
	title: string = ''
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	ngOnInit() {
		this.buildForm()
		this.getList()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
		// $('#modal').on('keypress', function (e) {
		// 	if (e.which == 13) e.preventDefault()
		// })
	}

	get f() {
		return this.form.controls
	}
	dataUpdate: any
	preUpdateStatus(data) {
		this.dataUpdate = data
		$('#modalConfirmUpdateStatus').modal('show')
	}
	buildForm() {
		this.form = this._fb.group({
			name: [this.model.name, Validators.required],
			description: [this.model.description],
			isActived: [this.model.isActived, Validators.required],
		})
	}

	rebuilForm() {
		this.form.reset({
			name: this.model.name,
			isActived: this.model.isActived,
			description: this.model.description,
		})
	}

	getList() {
		this._service.groupWordGetList({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.CAGroupWordGetAllOnPage
					this.totalRecords = response.result.TotalCount
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

	preCreate() {
		this.model = new FieldObject()
		this.rebuilForm()
		this.submitted = false
		this.title = 'Thêm mới nhóm thư viện từ ngữ'
		$('#modal').modal('show')
	}

	onSave() {
		this.submitted = true
		if (this.form.invalid) {
			return
		}
		if (this.model.id == 0 || this.model.id == null) {
			this._service.groupWordInsert(this.model).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						$('#target').focus()
						return
					} else {
						$('#modal').modal('hide')
						this._toastr.success(MESSAGE_COMMON.ADD_SUCCESS)
						this.getList()
					}
				} else {
					this._toastr.error(response.message)
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		} else {
			this._service.groupWordUpdate(this.model).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						$('#target').focus()
						return
					} else {
						$('#modal').modal('hide')
						this._toastr.success(MESSAGE_COMMON.UPDATE_SUCCESS)
						this.getList()
					}
				} else {
					this._toastr.error(response.message)
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		}
	}

	preUpdate(data) {
		let request = {
			Id: data.id,
			Type: 1,
		}
		this._service.groupWordGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.rebuilForm()
				this.title = 'Chỉnh sửa nhóm thư viện từ ngữ'
				this.model = response.result.CAGroupWordGetByID[0]
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
	preDelete(id: number) {
		this.idDelete = id
		$('#modalConfirmDelete').modal('show')
	}

	onDelete(id: number) {
		let request = {
			Id: id,
		}
		this._service.groupWordDelete(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result > 0) {
					this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				} else {
					this._toastr.error('Nhóm thư viện từ ngữ này đang được sử dụng!')
				}
				$('#modalConfirmDelete').modal('hide')
				this.getList()
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
			}
	}

	onUpdateStatus(data) {
		var isActived = data.isActived
		let request = {
			Type: 1,
			Id: data.id,
		}
		data.isActived = !data.isActived
		this._service.groupWordUpdateStatus(data).subscribe((res) => {
			$('#modalConfirmUpdateStatus').modal('hide')
			if (res.success == 'OK') {
				if (data.isActived == true) {
					this._toastr.success(MESSAGE_COMMON.UNLOCK_SUCCESS)
				} else {
					this._toastr.success(MESSAGE_COMMON.LOCK_SUCCESS)
				}
			} else {
				this._toastr.error(res.message)
			}
		}),
			(error) => {
				console.error(error)
			}
	}

	preView(data) {
		this.model = data
		$('#modalDetail').modal('show')
	}

	getListWordLibraryByGroupId(groupId : any){
		this._service.wordGetListByGroupId({GroupId : groupId}).subscribe(res =>{
			if(res.success == RESPONSE_STATUS.success){
				this.listWordByGroupId = res.result.CAWordGetAllOnPageByGroupId
				$('#modalLstWordByGroupId').modal('show')
			}
			else{
				this.listWordByGroupId = []
			}
		}),(error)=>{
			console.log(error)
		}
	}
}

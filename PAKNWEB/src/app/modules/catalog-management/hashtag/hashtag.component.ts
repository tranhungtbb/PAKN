import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms'
import { HttpEventType, HttpClient, HttpParams } from '@angular/common/http'
import { ToastrService } from 'ngx-toastr'
import { HashtagObject } from 'src/app/models/hashtagObject'
import { HashtagService } from 'src/app/services/hashtag.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { of } from 'rxjs'
import { stringify } from '@angular/compiler/src/util'

declare var $: any

@Component({
	selector: 'app-hashtag',
	templateUrl: './hashtag.component.html',
	styleUrls: ['./hashtag.component.css'],
})
export class HashtagComponent implements OnInit {
	// khai báo

	public listHastag = new Array<HashtagObject>()

	cols: any[]

	public listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]

	public hashtag = new HashtagObject()
	public PageIndex
	public PageSize
	public totalRecord
	public nameHash = ''
	public IsActived
	public fForm: FormGroup
	public Title: string
	public IdDelete: number
	submitted: boolean = false

	@ViewChild('table', { static: false }) table: any

	constructor(private service: HashtagService, private _toastr: ToastrService, private formBuilder: FormBuilder, private _shareData: DataService) {
		this.PageIndex = 1
		this.PageSize = 20
	}

	ngOnInit() {
		// form validate

		this.buildForm()
		this.GetListHashtag()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	buildForm() {
		this.fForm = this.formBuilder.group({
			name: [this.hashtag.name.trim(), Validators.required],
			isActived: [this.hashtag.isActived, Validators.required],
		})
	}

	rebuilForm() {
		this.fForm.reset({
			name: this.hashtag.name,
			isActived: this.hashtag.isActived,
		})
	}

	get f() {
		return this.fForm.controls
	}

	GetListHashtag() {
		this.nameHash = this.nameHash.trim()
		debugger
		var obj = {
			PageSize: this.PageSize,
			PageIndex: this.PageIndex,
			Name: this.nameHash,
		}
		if (typeof this.IsActived !== 'undefined' && this.IsActived != null) {
			obj['IsActived'] = this.IsActived
		}
		this.service.getAllPagedList(obj).subscribe((res) => {
			if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
				if (res.result) {
					this.listHastag = res.result.CAHashtag
					this.totalRecord = res.result.TotalCount
				}
			} else {
				this._toastr.error(res.message)
			}
		})
	}

	onPageChange(event: any) {
		this.PageSize = event.rows
		this.PageIndex = event.first / event.rows + 1
		this.GetListHashtag()
	}

	dataStateChange() {
		this.table.first = 0
		this.GetListHashtag()
	}

	preCreate() {
		this.hashtag = new HashtagObject()
		this.Title = 'Thêm mới'
		this.submitted = false
		this.rebuilForm()
		$('#modal').modal('show')
	}

	preUpdate(obj: any) {
		this.hashtag = Object.assign(new HashtagObject(), obj)
		this.Title = 'Chỉnh sửa'
		$('#modal').modal('show')
	}

	confirmChangeStatus(data) {
		this.hashtag = { ...data }
		this.hashtag.isActived = !data.isActived
		$('#modalConfirmChangeStatus').modal('show')
	}

	UpdateIsActived() {
		this.onSave()
	}

	preDelete(id: any) {
		this.IdDelete = id
		$('#modalConfirmDelete').modal('show')
	}

	onSave() {
		this.submitted = true
		if (this.fForm.invalid) {
			return
		}

		this.hashtag.name = this.hashtag.name.trim()

		if (this.hashtag.name == '') {
			this.nameHash = ''
			return
		}
		// create
		if (this.hashtag.id == 0) {
			this.service.create(this.hashtag).subscribe((res) => {
				$('#modal').modal('hide')
				if (res != 'undefined') {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result == -1) {
							this._toastr.info(MESSAGE_COMMON.EXISTED_NAME)
						} else {
							this.GetListHashtag()
							this._toastr.success(MESSAGE_COMMON.ADD_SUCCESS)
						}
					}
				} else {
					this._toastr.error(MESSAGE_COMMON.ADD_FAILED)
				}
			})
		}
		// update
		else {
			this.service.update(this.hashtag).subscribe((res) => {
				$('#modalConfirmChangeStatus').modal('hide')
				debugger
				if (res != 'undefined') {
					if (res.success == RESPONSE_STATUS.success) {
						$('#modal').modal('hide')
						this.rebuilForm()
						this.GetListHashtag()
						this._toastr.success(MESSAGE_COMMON.UPDATE_SUCCESS)
					} else if (res.success == RESPONSE_STATUS.orror && res.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
					} else {
						$('#modal').modal('hide')
						this._toastr.error(MESSAGE_COMMON.UPDATE_FAILED)
					}
				} else {
					$('#modal').modal('hide')
					this._toastr.error(MESSAGE_COMMON.UPDATE_FAILED)
				}
			})
		}
	}

	// delete
	onDelete() {
		this.service.delete({ id: this.IdDelete }).subscribe((res) => {
			$('#modalConfirmDelete').modal('hide')
			if (res != 'undefined') {
				if (res.success == RESPONSE_STATUS.success) {
					this.GetListHashtag()
					this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				} else {
					this._toastr.error('Hashtag này đang được sử dụng')
				}
			} else {
				this._toastr.error(MESSAGE_COMMON.DELETE_FAILED)
			}
		})
	}
}

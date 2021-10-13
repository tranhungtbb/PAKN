import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms'
import { HttpEventType, HttpClient, HttpParams } from '@angular/common/http'
import { ToastrService } from 'ngx-toastr'
import { HashtagObject } from 'src/app/models/hashtagObject'
import { HashtagService } from 'src/app/services/hashtag.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { Table } from 'primeng/components/table/table'

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
	public totalRecord
	public fForm: FormGroup
	public Title: string
	public IdDelete: number
	submitted: boolean = false
	listData: any[]
	recommendationsGetByHashtag: any[]

	@ViewChild('table', { static: false }) table: Table
	@ViewChild('tableMRR', { static: false }) tableMRR: any

	listMrrStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Chờ xử lý' },
		{ value: 3, text: 'Từ chối xử lý' },
		{ value: 4, text: 'Đã tiếp nhận' },
		{ value: 5, text: 'Chờ giải quyết' },
		{ value: 6, text: 'Từ chối giải quyết' },
		{ value: 7, text: 'Đang giải quyết' },
		{ value: 8, text: 'Chờ phê duyệt' },
		{ value: 9, text: 'Từ chối phê duyệt' },
		{ value: 10, text: 'Đã giải quyết' },
	]

	mrrCode: any
	mrrSendName: any
	mrrTitle: any
	mrrContent: any
	mrrStatus: any
	mrrPageSize: Number
	mrrPageIndex: Number
	mrrHashtagId: Number

	totalRecord2: number = 0

	constructor(
		private service: HashtagService,
		private _toastr: ToastrService,
		private formBuilder: FormBuilder,
		private _shareData: DataService,
		private recommendationService: RecommendationService
	) {
		this.mrrPageSize = 10
		this.mrrPageIndex = 1
	}

	// const FilterUtils = require('primeng/components/utils/filterutils').FilterUtils;

	ngOnInit() {
		// form validate

		this.buildForm()
		this.GetListHashtag()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
		// $('#modal').on('keypress', function (e) {
		// 	if (e.which == 13) e.preventDefault()
		// })
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
		this.service.getAllPagedList({}).subscribe((res) => {
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

	listRecommendationByHashtag(id: any) {
		if (id == undefined) return
		this.mrrHashtagId = id
		var obj = {
			Code: this.mrrCode != null ? this.mrrCode : '',
			SendName: this.mrrSendName != null ? this.mrrSendName : '',
			Title: this.mrrTitle != null ? this.mrrTitle : '',
			Content: this.mrrContent != null ? this.mrrContent : '',
			Status: this.mrrStatus != null ? this.mrrStatus : '',
			UnitId: Number(localStorage.getItem('unitId')),
			HashtagId: id,
			PageSize: this.mrrPageSize,
			PageIndex: this.mrrPageIndex,
		}

		this.service.recommendationGetByHashtagAllOnPage(obj).subscribe((res) => {
			if ((res.success = RESPONSE_STATUS.success)) {
				this.recommendationsGetByHashtag = res.result.MRRecommendationGetByHashtagAllOnPage
				this.totalRecord2 = this.recommendationsGetByHashtag.length > 0 ? this.recommendationsGetByHashtag[0].rowNumber : 0
				$('#modalRecommendationGetByHashtag').modal('show')
			}
			return
		})
	}

	onPageChange2(event: any) {
		this.mrrPageSize = event.rows
		this.mrrPageIndex = event.first / event.rows + 1
		this.listRecommendationByHashtag(this.mrrHashtagId)
	}

	dataStateChange2() {
		this.table.first = 0
		this.mrrPageIndex = 1
		this.mrrPageSize = 10
		this.listRecommendationByHashtag(this.mrrHashtagId)
	}

	preCreate() {
		this.hashtag = new HashtagObject()
		this.Title = 'Thêm mới hashtag'
		this.submitted = false
		this.rebuilForm()
		$('#modal').modal('show')
	}

	preUpdate(obj: any) {
		this.hashtag = Object.assign(new HashtagObject(), obj)
		this.Title = 'Chỉnh sửa hashtag'
		$('#modal').modal('show')
	}

	confirmChangeStatus(data) {
		this.hashtag = { ...data }
		this.hashtag.isActived = !data.isActived
		$('#modalConfirmChangeStatus').modal('show')
	}

	UpdateIsActived() {
		$('#modalConfirmChangeStatus').modal('hide')
		this.service.changeStatus(this.hashtag).subscribe((res) => {
			if (res != 'undefined') {
				if (res.success == RESPONSE_STATUS.success) {
					this.rebuilForm()
					this.GetListHashtag()
					this._toastr.success(MESSAGE_COMMON.UPDATE_SUCCESS)
				} else if (res.success == RESPONSE_STATUS.orror && res.result == -1) {
					this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
				} else {
					this._toastr.error(MESSAGE_COMMON.UPDATE_FAILED)
				}
			} else {
				this._toastr.error(MESSAGE_COMMON.UPDATE_FAILED)
			}
		})
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
			return
		}
		// create
		if (this.hashtag.id == 0) {
			this.service.create(this.hashtag).subscribe((res) => {
				if (res != 'undefined') {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result == -1) {
							this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
						} else {
							$('#modal').modal('hide')
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
		let obj = this.listHastag.find((x) => x.id == this.IdDelete)
		this.service.delete({ id: this.IdDelete }, obj.name).subscribe((res) => {
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

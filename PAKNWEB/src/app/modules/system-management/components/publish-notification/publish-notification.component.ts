import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { PublishNotificationObject } from 'src/app/models/fieldObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { DataService } from 'src/app/services/sharedata.service'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'

// import { RemindComponent } from 'src/app/modules/recommendation/remind/remind.component'

declare var $: any
@Component({
	selector: 'app-publish-notification',
	templateUrl: './publish-notification.component.html',
	styleUrls: ['./publish-notification.component.css'],
})
export class PublishNotificationComponent implements OnInit, AfterViewInit {
	constructor(
		private _service: CatalogService,
		private _toastr: ToastrService,
		private _fb: FormBuilder,
		private _shareData: DataService,
		private userStorage: UserInfoStorageService,
		private _serviceR: RecommendationService
	) {}

	// child

	// @ViewChild(RemindComponent, { static: false }) remindComponent: RemindComponent

	listData = new Array<PublishNotificationObject>()
	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Công bố' },
	]
	form: FormGroup
	model: PublishNotificationObject = new PublishNotificationObject()
	submitted: boolean = false
	title: string = ''
	content: string = ''

	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	idDelete: number = 0
	ngOnInit() {
		this.buildForm()
		this.getList()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	get f() {
		return this.form.controls
	}

	buildForm() {
		this.form = this._fb.group({
			title: [this.model.title, Validators.required],
			status: [this.model.status, Validators.required],
			content: [this.model.content],
		})
	}

	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			status: this.model.status,
			content: this.model.content,
		})
	}

	getList() {
		this._service.publishNotificationGetList({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.data
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
		this.model = new PublishNotificationObject()
		this.rebuilForm()
		this.submitted = false
		$('#modal').modal('show')
		setTimeout(() => {
			$('#target').focus()
		}, 400)
	}

	onSave() {
		this.submitted = true
		this.model.title = this.model.title.trim()
		if (this.model.title == '') return
		if (this.form.invalid) {
			return
		}
		if (this.model.id == 0 || this.model.id == null) {
			this.model.createdBy = this.userStorage.getUserId()
			this._service.publishNotificationInsert(this.model).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
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
			this._service.publishNotificationUpdate(this.model).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
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
		this._service.publishNotificationGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.rebuilForm()
				this.model = response.result.data[0]
				$('#modal').modal('show')
				setTimeout(() => {
					$('#target').focus()
				}, 400)
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
		let obj = this.listData.find((x) => x.id == id)
		this._service.publishNotificationDelete(request, obj.title).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
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

	preView(data) {
		this.model = data
		$('#modalDetail').modal('show')
	}
}

import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { CONSTANTS, FILETYPE, RESPONSE_STATUS, MESSAGE_COMMON } from 'src/app/constants/CONSTANTS'
import { ViewRecommendationComponent } from 'src/app/modules/recommendation/view-recommendation/view-recommendation.component'
import { RemindObject } from '../../../models/remindObject'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RemindService } from 'src/app/services/remind.service'
import { from } from 'rxjs'

declare var $: any

@Component({
	selector: 'app-remind',
	templateUrl: './remind.component.html',
	styleUrls: ['./remind.component.css'],
})
export class RemindComponent implements OnInit {
	constructor(
		private toastr: ToastrService,
		private _fb: FormBuilder,
		private fileService: UploadFileService,
		private remindService: RemindService,
		private viewRecommendationComponent: ViewRecommendationComponent
	) {}

	files: any = []
	model = new RemindObject()
	sendOrgId: any
	remindForm: any
	listRemind: any
	@ViewChild('file', { static: false }) public file: ElementRef
	fileAccept = CONSTANTS.FILEACCEPT
	submitted = false

	ngOnInit() {
		this.buildForm()
		this.getListRemind()
	}

	get f() {
		return this.remindForm.controls
	}

	buildForm() {
		this.remindForm = this._fb.group({
			content: [this.model.content, Validators.required],
		})
	}

	rebuilForm() {
		this.remindForm.reset({
			content: this.model.content,
		})
	}

	getListRemind() {
		//this.remindService.remindGetList({RecommendationId : }).subscribe(res=> console.log(res))
	}

	onInsert() {
		this.submitted = true
		debugger

		if (this.remindForm.invalid) {
			return
		}
		this.model.recommendationId = this.viewRecommendationComponent.model.id
		this.sendOrgId = this.viewRecommendationComponent.model.unitId
		var obj = {
			Model: { ...this.model },
			Files: this.files,
			SendOrgId: this.sendOrgId,
		}
		this.remindService.remindInsert(obj).subscribe((res) => {
			debugger
			if (res.success == RESPONSE_STATUS.success) {
				$('#modal2').modal('hide')
				this.model = new RemindObject()
				this.files = []
				this.rebuilForm()
				this.toastr.success(MESSAGE_COMMON.ADD_SUCCESS)
			} else {
				this.toastr.error(MESSAGE_COMMON.ADD_FAILED)
			}
		})
	}

	onUpload(event) {
		if (event.target.files.length == 0) {
			return
		}
		const check = this.fileService.checkFileWasExitsted(event, this.files)
		if (check === 1) {
			for (let item of event.target.files) {
				FILETYPE.forEach((fileType) => {
					if (item.type == fileType.text) {
						let max = this.files.reduce((a, b) => {
							return a.id > b.id ? a.id : b.id
						}, 0)
						item.id = max + 1
						item.fileType = fileType.value
						this.files.push(item)
					}
				})
				if (!item.fileType) {
					this.toastr.error('Định dạng không được hỗ trợ')
				}
			}
		} else if (check === 2) {
			this.toastr.error('Không được phép đẩy trùng tên file lên hệ thống')
		} else {
			this.toastr.error('File tải lên vượt quá dung lượng cho phép 10MB')
		}
		this.file.nativeElement.value = ''
		console.log(this.files)
	}

	onRemoveFile(item: any) {
		debugger
		for (let index = 0; index < this.files.length; index++) {
			if (this.files[index].id == item.id) {
				this.files.splice(index, 1)
				break
			}
		}
	}

	showComponent() {
		this.submitted = false
		$('#modal2').modal('show')
	}
}

import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { CONSTANTS, FILETYPE, RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { FieldComponent } from '../field/field.component'
import { RemindObject } from '../../../models/remindObject'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { from } from 'rxjs'

declare var $: any

@Component({
	selector: 'app-remind',
	templateUrl: './remind.component.html',
	styleUrls: ['./remind.component.css'],
})
export class RemindComponent implements OnInit {
	constructor(private toastr: ToastrService, private _fb: FormBuilder, private filed: FieldComponent, private fileService: UploadFileService) {}

	files: any = []
	model = new RemindObject()
	@ViewChild('file', { static: false }) public file: ElementRef
	fileAccept = CONSTANTS.FILEACCEPT

	ngOnInit() {}

	onUpload(event) {
		if (event.target.files.length == 0) {
			return
		}
		const check = this.fileService.checkFileWasExitsted(event, this.files)
		if (check === 1) {
			for (let item of event.target.files) {
				FILETYPE.forEach((fileType) => {
					if (item.type == fileType.text) {
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
		$('#modal2').modal('show')
	}
}

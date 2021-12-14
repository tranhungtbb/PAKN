import { Component, OnInit, ViewChild } from '@angular/core'
import { Lightbox } from 'ngx-lightbox'
import { ToastrService } from 'ngx-toastr'
import { COMMONS } from 'src/app/commons/commons'
import { FILETYPE, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { SupportGalleryService } from 'src/app/services/support-gallery.service'
import { UploadFileService } from 'src/app/services/uploadfiles.service'

declare var $: any
@Component({
	selector: 'app-support-gallery',
	templateUrl: './support-gallery.component.html',
	styleUrls: ['./support-gallery.component.css'],
})
export class SupportGalleryComponent implements OnInit {
	constructor(private _lightbox: Lightbox, private service: SupportGalleryService, private toastr: ToastrService, private fileService: UploadFileService) {}
	@ViewChild('file', { static: false }) file: any
	files: any[] = []
	galleries: any[] = []
	ngOnInit() {
		this.getList()
	}

	getList = () => {
		this.service.getAllSupportGallery({}).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.galleries = res.result
				} else {
					this.galleries = []
				}
			},
			(error) => {
				console.log(error)
			}
		)
	}

	onChange = (event) => {
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
					return
				}
			}
		} else if (check === 2) {
			this.toastr.error('Không được phép đẩy trùng tên file lên hệ thống')
			return
		} else {
			this.toastr.error('File tải lên vượt quá dung lượng cho phép 10MB')
			return
		}
		this.file.nativeElement.value = ''
		this.service.insertSupportGallery({ files: this.files }).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.toastr.success(COMMONS.ADD_SUCCESS)
					this.getList()
				} else {
					this.toastr.error(res.message)
				}
			},
			(err) => {
				this.toastr.error(err)
			}
		)
		this.files = []
	}
	idDelete: number
	preDelete = (id: number) => {
		this.idDelete = id
		$('#modalConfirmDelete').modal('show')
	}
	onDelete = () => {
		let obj = this.galleries.find((x) => x.id === this.idDelete)
		this.service.deleteSupportGallery({ Id: this.idDelete, filePath: obj.src }).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.toastr.success(COMMONS.DELETE_SUCCESS)
					this.getList()
					$('#modalConfirmDelete').modal('hide')
				} else {
					this.toastr.error(res.message)
				}
			},
			(err) => {
				this.toastr.error(err)
			}
		)
	}
	open(index: number): void {
		this._lightbox.open(this.galleries, index)
	}

	close(): void {
		this._lightbox.close()
	}
}

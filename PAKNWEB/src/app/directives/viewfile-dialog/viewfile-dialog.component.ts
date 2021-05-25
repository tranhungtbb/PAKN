import { Component, OnInit, ViewChild, ChangeDetectorRef, ElementRef } from '@angular/core'
import { MAT_DIALOG_DATA } from '@angular/material'
import { Inject } from '@angular/core'
import { UploadFileService } from '../../services/uploadfiles.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { ToastrService } from 'ngx-toastr'
import { AppSettings } from '../../constants/app-setting'
import { HttpClient } from '@angular/common/http'
declare var $: any

@Component({
	selector: 'app-viewfile-dialog',
	templateUrl: './viewfile-dialog.component.html',
	styleUrls: ['./viewfile-dialog.component.css'],
})
export class ViewFileDialogComponent implements OnInit {
	constructor(
		@Inject(MAT_DIALOG_DATA) public data: any,
		private filesService: UploadFileService,
		private toastr: ToastrService,
		private http: HttpClient,
		private cdRef: ChangeDetectorRef
	) {}
	downloadId: number = 0
	filename: string = ''
	IsPDF: boolean = false
	IsIMG: boolean = false
	IsVideo: boolean = false
	IsShowCreateVersion = false
	files: any
	typeFile: any
	@ViewChild('pdfViewer', { static: false }) pdfViewer
	listImg: any

	ngOnInit() {
		this.typeFile = this.data.type
		if (this.data.link != null) {
			this.IsPDF = false
			this.IsIMG = false
			this.IsVideo = false
			this.cdRef.detectChanges()
			// Check nếu là file ảnh thì mở trực tiếp
			let listsplit = this.data.name
			listsplit = listsplit.split('.')
			let extenfile = listsplit[listsplit.length - 1].toLowerCase()
			if (extenfile == 'png' || extenfile == 'jpg' || extenfile == 'jpeg' || extenfile == 'gif') {
				this.IsIMG = true
				this.LoadViewImage(this.data.link, this.data.name)
			} else if (extenfile == 'pdf') {
				var linkfile = this.data.link
				linkfile = linkfile
				// linkfile = linkfile.replace(AppSettings.API_DOWNLOADFILES, "");
				this.IsPDF = true
				this.IsIMG = false
				this.IsVideo = false
				this.cdRef.detectChanges()
				this.LoadView(linkfile)
			} else if (extenfile == 'doc' || extenfile == 'docx' || extenfile == 'xls' || extenfile == 'xlsx') {
				var linkfile = this.data.link
				linkfile = linkfile
				// linkfile = linkfile.replace(AppSettings.API_DOWNLOADFILES, "");
				$('#viewfile').attr('src', AppSettings.VIEW_FILE + btoa(linkfile))
			} else if (extenfile == 'mp4' || extenfile == 'wmv') {
				this.IsPDF = false
				this.IsIMG = false
				this.IsVideo = true
				this.cdRef.detectChanges()
				this.loadvideo(this.data.link, this.data.name)
			} else {
				$('#viewfile').attr('src', this.data.link)
			}
			var fileurl = this.data.link
			fileurl = fileurl
			// fileurl = fileurl.replace(AppSettings.API_DOWNLOADFILES, "");
			this.filename = fileurl
		}
	}

	setClassFile(item: any) {
		let listsplit = item.name.split('.')
		let extenfile = listsplit[listsplit.length - 1].toLowerCase()

		if (extenfile == 'pdf') {
			return 'fa-file-pdf-o'
		}
		if (extenfile == 'txt') {
			return 'fa-file-text-o'
		}
		if (extenfile == 'doc' || extenfile == 'docx') {
			return 'fa-file-word-o'
		}
		if (extenfile == 'xls' || extenfile == 'xlsx') {
			return 'fa-file-excel-o'
		}
		if (extenfile == 'png' || extenfile == 'jpg' || extenfile == 'jpeg' || extenfile == 'gif') {
			return 'fa-file-image-o'
		} else {
			return 'fa-paperclip'
		}
	}

	DownloadFile() {
		if (this.downloadId != 0) {
			var request = {
				id: this.downloadId,
				type: this.typeFile,
			}
			this.filesService.downloadFilebyId(request).subscribe(
				(response) => {
					var blob = new Blob([response], { type: response.type })
					importedSaveAs(blob, this.filename)
				},
				(error) => {
					this.toastr.error('Không tìm thấy file trên hệ thống')
				}
			)
		} else {
			var request1 = {
				Path: this.data.path,
				Name: this.data.name,
			}
			this.filesService.downloadFile(request1).subscribe(
				(response) => {
					var blob = new Blob([response], { type: response.type })
					importedSaveAs(blob, this.data.name)
				},
				(error) => {
					this.toastr.error('Không tìm thấy file trên hệ thống')
				}
			)
		}
	}

	LoadView(link) {
		var request = {
			Path: link,
			Name: this.data.name,
		}
		this.filesService.downloadFile(request).subscribe(
			(response) => {
				var blob = new Blob([response], { type: response.type })
				this.pdfViewer.pdfSrc = blob
				this.pdfViewer.refresh()
			},
			(error) => {
				this.toastr.error('Không tìm thấy file trên hệ thống')
			}
		)
	}

	LoadViewImage(link, name) {
		var request = {
			Path: link,
			Name: name,
		}
		this.filesService.downloadFile(request).subscribe(
			(response) => {
				var blob = new Blob([response], { type: response.type })
				var blob_url = URL.createObjectURL(blob)
				var reader = new FileReader()
				let that = this
				reader.onload = function () {
					var b64 = reader.result.toString().replace(/^data:.+;base64,/, '')
					that.listImg = []
					that.listImg.push(b64)
				}
				reader.readAsDataURL(blob)
			},
			(error) => {
				this.toastr.error('Không tìm thấy file trên hệ thống')
			}
		)
	}
	// Video
	VideoSource: any = ''

	@ViewChild('videodemo', null) public video: ElementRef

	loadvideo(link, name) {
		var request = {
			Path: link,
			Name: name,
		}

		this.filesService.downloadFile(request).subscribe(
			(response) => {
				var blob = new Blob([response], { type: response.type })
				var srcvideo = window.URL.createObjectURL(blob)
				this.VideoSource = srcvideo
				this.video.nativeElement.crossOrigin = 'anonymous'
				this.video.nativeElement.src = srcvideo
				this.video.nativeElement.load()
				this.video.nativeElement.play()
			},
			(error) => {
				this.toastr.error('Không tìm thấy file trên hệ thống')
			}
		)
	}
}

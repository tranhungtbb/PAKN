import { Component, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { saveAs as importedSaveAs } from 'file-saver'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { AppSettings } from 'src/app/constants/app-setting'

declare var $: any

@Component({
	selector: 'app-support-list',
	templateUrl: './support-list.component.html',
	styleUrls: ['./support-list.component.css'],
})
export class SupportListComponent implements OnInit {
	constructor(private localStorage: UserInfoStorageService, private _filesService: UploadFileService, private toastr: ToastrService) {}
	urlSupperAdmin: Array<string> = []
	urlAdmin: string = ''
	urlUser: string = ''
	ngOnInit() {
		var request = {
			UserId: this.localStorage.getUserId(),
		}
		this._filesService.getFileSupport(request).subscribe((data) => {
			if (data) {
				if (data.length > 0) {
					this.urlSupperAdmin = data
					$('#cv').attr('src', encodeURI(data[0]))
					$('#ldcb').attr('src', encodeURI(data[1]))
					$('#qtht').attr('src', encodeURI(data[2]))
					$('#db').attr('src', encodeURI(data[3]))
				}
			}
		})
	}

	DownloadFile(index) {
		let fileName: string = ''
		var lstsplit = this.urlSupperAdmin[index].split('/')
		fileName = lstsplit[lstsplit.length - 1]
		var request = {
			filePath: this.urlSupperAdmin[index],
			Name: fileName,
		}
		this._filesService.downloadFileSupport(request).subscribe(
			(response) => {
				if (response != undefined) {
					var blob = new Blob([response], { type: response.type })
					importedSaveAs(blob, fileName)
				}
			},
			(error) => {
				this.toastr.error('Không tìm thấy file trên hệ thống')
			}
		)
	}
}

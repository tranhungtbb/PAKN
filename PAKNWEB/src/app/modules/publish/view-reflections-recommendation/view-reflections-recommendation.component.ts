import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { saveAs as importedSaveAs } from 'file-saver'

@Component({
	selector: 'app-view-reflections-recommendations',
	templateUrl: './view-reflections-recommendation.component.html',
	styleUrls: ['./view-reflections-recommendation.component.css'],
})
export class ViewReflectionsRecommendationComponent implements OnInit {
	public id: any = 0
	public model: any
	public lstFiles: any
	public lstConclusion: any
	public lstConclusionFiles: any
	public satisfactions: Array<satisfaction>
	checkSatisfaction: boolean
	isLogin: boolean = this.storeageService.getIsHaveToken()
	typeObject: number = this.storeageService.getTypeObject()
	constructor(
		private service: PuRecommendationService,
		private activatedRoute: ActivatedRoute,
		public router: Router,
		private _toastr: ToastrService,
		private storeageService: UserInfoStorageService,
		private fileService: UploadFileService
	) {
		this.checkSatisfaction = false
	}


	ngOnInit() {
		this.getRecommendationById()
	}

	getRecommendationById() {
		this.activatedRoute.params.subscribe((params) => {
			this.id = +params['id']
			if (this.id != 0) {
				//update count click
				this.service.getById({ id: this.id, status: RECOMMENDATION_STATUS.FINISED }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result.model != null) {
							this.model = { ...res.result.model, shortName: this.getShortName(res.result.model.name) }
							if (this.model.contentConclusion) {
								this.model.contentConclusion = this.model.contentConclusion.split(':').splice(1, this.model.contentConclusion.split(':').length - 1)
							}
							if (this.model.quantityType && this.model.quantityType != 0) {
								this.checkSatisfaction = true
							}
							this.lstFiles = res.result.lstFiles
							this.lstConclusion = res.result.lstConclusion
							this.lstConclusionFiles = res.result.lstConclusionFiles
						}
					}
				})
			}
		})
	}

	getShortName(string) {
		if (!string) {
			return ''
		}
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()
		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}

	changeSatisfaction(status: any) {
		if (this.isLogin) {
			// chưa like hoặc dislike lần nào
			// if (this.checkSatisfaction == false) {
			// call api
			this.service.changeSatisfaction({ RecommendationId: this.id, Satisfaction: status }).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this._toastr.success('Đánh giá thành công!')
					// this.checkSatisfaction = true
					if (this.model.quantityType) {
						switch (this.model.quantityType) {
							case 1:
								this.model.quantityLike = this.model.quantityLike - 1
								break
							case 2:
								this.model.quantityDislike = this.model.quantityDislike - 1
								break
							case 3:
								this.model.quantityAccept = this.model.quantityAccept - 1
								break
						}
					}

					this.model.quantityType = status
					switch (status) {
						case 1:
							this.model.quantityLike = this.model.quantityLike + 1
							break
						case 2:
							this.model.quantityDislike = this.model.quantityDislike + 1
							break
						case 3:
							this.model.quantityAccept = this.model.quantityAccept + 1
							break
					}
				} else {
					this._toastr.error('Bạn đã đánh giá Phản ánh, Kiến nghị này!')
				}
			})
		} else {
			this._toastr.info('Vui lòng đăng nhập để đánh giá Phản ánh, Kiến nghị!')
			return
		}
	}

	DownloadFile(file: any) {
		var request = {
			Path: file.filePath,
			Name: file.name,
		}
		this.fileService.downloadFile(request).subscribe(
			(response) => {
				var blob = new Blob([response], { type: response.type })
				importedSaveAs(blob, file.name)
			},
			(error) => {
				this._toastr.error('Không tìm thấy file trên hệ thống')
			}
		)
	}
}

interface satisfaction {
	recommendationID: number
	satisfaction: boolean
}

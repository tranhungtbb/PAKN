import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { ViewRightComponent } from 'src/app/modules/publish/view-right/view-right.component'
import { ViewChild } from '@angular/core'
import { RecommendationCommentService } from 'src/app/services/recommendation-comment.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RecommnendationCommentObject } from 'src/app/models/recommendationObject'
import { AppSettings } from 'src/app/constants/app-setting'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { saveAs as importedSaveAs } from 'file-saver'

declare var require: any
const FileSaver = require('file-saver')

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
	satisfactionCurrent: boolean
	checkSatisfaction: boolean
	pageSizeComment: any = 20
	IsAllComment: boolean = false
	isLogin: boolean = this.storeageService.getIsHaveToken()
	APIADDRESS: any
	constructor(
		private service: PuRecommendationService,
		private activatedRoute: ActivatedRoute,
		public router: Router,
		private _toastr: ToastrService,
		private commentService: RecommendationCommentService,
		private storeageService: UserInfoStorageService,
		private fileService: UploadFileService
	) {
		this.checkSatisfaction = false
		this.listCommentsPaged = []
		this.APIADDRESS = AppSettings.API_ADDRESS.replace('api/', '')
	}
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
	ngOnInit() {
		this.getRecommendationById()
		this.setSatisfaction()
		this.getCommentPaged()
	}

	getRecommendationById() {
		this.activatedRoute.params.subscribe((params) => {
			this.id = +params['id']
			if (this.id != 0) {
				//update count click
				this.service.countClick({ RecommendationId: this.id }).subscribe()

				// call api getRecommendation by id
				// tạm thời fix status = 3, nhưng thực tế status success = 10
				this.service.getById({ id: this.id, status: RECOMMENDATION_STATUS.FINISED }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result.model != null) {
							this.model = { ...res.result.model, shortName: this.getShortName(res.result.model.name) }
							// console.log(this.model)
							this.lstFiles = res.result.lstFiles
							this.lstConclusion = res.result.lstConclusion
							this.lstConclusionFiles = res.result.lstConclusionFiles
							// console.log(this.lstConclusionFiles)
						}
					}
				})
			}
		})
	}

	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()
		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}
	setSatisfaction() {
		var data = localStorage.getItem('satisfaction')
		if (data == null || data == undefined) {
			this.satisfactions = []
			localStorage.setItem('satisfaction', JSON.stringify(this.satisfactions))
			return
		} else {
			this.satisfactions = JSON.parse(data)
			this.satisfactions.forEach((item) => {
				if (item.recommendationID == this.id) {
					this.satisfactionCurrent = item.satisfaction
					this.checkSatisfaction = true
				}
			})
		}
	}

	changeSatisfaction(status: any) {
		if (this.satisfactionCurrent == null || this.satisfactionCurrent == undefined) {
			// chưa like hoặc dislike lần nào
			if (this.checkSatisfaction == false) {
				this.satisfactionCurrent = status
				// call api
				this.service.changeSatisfaction({ RecommendationId: this.id, Satisfaction: status }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						this._toastr.success('Đánh giá thành công!')
						let check = false
						this.satisfactions.forEach((item) => {
							if (item.recommendationID == this.id) {
								check = true
							}
						})
						if (check == false) {
							let obj = {
								recommendationID: this.id,
								satisfaction: status,
							}
							this.satisfactions.push(obj)
							localStorage.setItem('satisfaction', JSON.stringify(this.satisfactions))
						}
						if (status) {
							this.model.quantityLike = this.model.quantityLike + 1
						} else {
							this.model.quantityDislike = this.model.quantityDislike + 1
						}
					} else {
						this._toastr.error('Đánh giá thất bại!')
					}
				})
			} else {
				this.checkSatisfaction = true
				this._toastr.error('Bạn đã đánh giá Phản ánh, kiến nghị này!')
				return
			}
		} else {
			this._toastr.error('Bạn đã đánh giá Phản ánh, kiến nghị này!')
			return
		}
	}

	//comment
	commentModel: RecommnendationCommentObject = new RecommnendationCommentObject()
	commentQuery: any = {
		pageSize: this.pageSizeComment,
		pageIndex: 1,
		recommendationId: 0,
		isPublish: true,
	}
	listCommentsPaged: any[] = []
	commentFirst = new RecommnendationCommentObject()
	total_Comments = 0

	onSendComment() {
		if (this.isLogin == false) {
			this._toastr.error('Vui lòng đăng nhập để gửi bình luận')
			return
		}
		this.commentModel.userId = this.storeageService.getUserId()
		this.commentModel.fullName = this.storeageService.getFullName()
		this.commentModel.recommendationId = this.model.id
		this.commentModel.contents = this.commentModel.contents.trim()
		this.commentModel.isPublish = true
		if (this.commentModel.contents == null || this.commentModel.contents == '') {
			this._toastr.error('Không bỏ trống nội dung bình luận')
			return
		}

		this.commentService.insert(this.commentModel).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				this._toastr.error('Xảy ra lỗi trong quá trình xử lý')
				return
			}
			this._toastr.success('Thêm bình luận thành công')
			this.commentModel = new RecommnendationCommentObject()
			this.getCommentPaged()
		})
	}

	getCommentPaged() {
		this.commentQuery.pageSize = this.pageSizeComment
		this.commentQuery.recommendationId = this.id
		this.commentService.getAllOnPage(this.commentQuery).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.MRCommnentGetAllOnPage.length > 0) {
					this.commentFirst = res.result.MRCommnentGetAllOnPage.shift()
					debugger
					if (this.listCommentsPaged.length != 0 && this.listCommentsPaged.length == res.result.MRCommnentGetAllOnPage.length && res.result.TotalCount > 20) {
						this.IsAllComment = true
					} else {
						this.listCommentsPaged = res.result.MRCommnentGetAllOnPage
						this.total_Comments = res.result.TotalCount
					}
				} else {
					this.listCommentsPaged = []
					this.commentFirst = null
				}
			}
		})
	}
	loadComment() {
		this.pageSizeComment += 20
		this.getCommentPaged()
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

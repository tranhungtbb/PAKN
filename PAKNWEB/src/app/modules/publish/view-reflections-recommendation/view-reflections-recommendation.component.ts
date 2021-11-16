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
	}

	getRecommendationById() {
		this.activatedRoute.params.subscribe((params) => {
			this.id = +params['id']
			if (this.id != 0) {
				//update count click
				this.getCommentPaged()
				this.service.getById({ id: this.id, status: RECOMMENDATION_STATUS.FINISED }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result.model != null) {
							this.model = { ...res.result.model, shortName: this.getShortName(res.result.model.name) }
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
	// setSatisfaction() {
	// 	var data = localStorage.getItem('satisfaction')
	// 	if (data == null || data == undefined) {
	// 		this.satisfactions = []
	// 		localStorage.setItem('satisfaction', JSON.stringify(this.satisfactions))
	// 		return
	// 	} else {
	// 		this.satisfactions = JSON.parse(data)
	// 		if (this.satisfactions instanceof Array) {
	// 			this.satisfactions.forEach((item) => {
	// 				if (item.recommendationID == this.id) {
	// 					this.satisfactionCurrent = item.satisfaction
	// 					this.checkSatisfaction = true
	// 				}
	// 			})
	// 		}
	// 	}
	// }

	changeSatisfaction(status: any) {
		if (this.isLogin) {
			// chưa like hoặc dislike lần nào
			if (this.checkSatisfaction == false) {
				// call api
				this.service.changeSatisfaction({ RecommendationId: this.id, Satisfaction: status }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						this._toastr.success('Đánh giá thành công!')

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
						this._toastr.error('Đánh giá thất bại!')
					}
				})
			} else {
				this.checkSatisfaction = true
				this._toastr.error('Bạn đã đánh giá Phản ánh, kiến nghị này!')
				return
			}
		} else {
			this._toastr.info('Vui lòng đăng nhập để đánh giá Phản ánh, Kiến nghị!')
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
	// commentFirst = new RecommnendationCommentObject()
	total_Comments = 0

	onSendComment() {
		if (this.isLogin == false) {
			this._toastr.error('Vui lòng đăng nhập để gửi bình luận')
			return
		}
		this.commentModel.recommendationId = this.model.id
		this.commentModel.contents = this.commentModel.contents == null ? '' : this.commentModel.contents.trim()
		this.commentModel.isPublish = true
		if (this.commentModel.contents == null || this.commentModel.contents == '') {
			this._toastr.error('Không bỏ trống nội dung bình luận')
			return
		}

		this.commentService.insert(this.commentModel).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				this._toastr.error(res.message)
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
					this.total_Comments = res.result.TotalCount
					this.listCommentsPaged = res.result.MRCommnentGetAllOnPage
					if (this.total_Comments == res.result.TotalCount && this.commentQuery.pageSize == 20) {
						this.IsAllComment = true
					}
				} else {
					this.listCommentsPaged = []
					this.IsAllComment = true
				}
			}
		})
	}
	loadComment() {
		this.pageSizeComment += 20
		this.getCommentPaged()
	}
	changeStatusComment = (comment) => {
		comment.isView = !comment.isView
		let obj = {
			Id: comment.id,
			IsView: comment.isView,
		}
		this.commentService.updateStatus(obj).subscribe(
			(res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this._toastr.success(comment.isView == true ? 'Công bố bình luận thành công' : 'Thu hồi bình luận thành công')
				} else {
					this._toastr.error(res.message)
				}
			},
			(err) => {
				console.log(err)
			}
		)
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

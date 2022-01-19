import { Component, Input, OnInit, AfterViewInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { ViewChild } from '@angular/core'
import { RecommendationCommentService } from 'src/app/services/recommendation-comment.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RecommnendationCommentObject } from 'src/app/models/recommendationObject'

declare var $: any

@Component({
	selector: 'app-comment-recommendation',
	templateUrl: './comment-recommendation.component.html',
	styleUrls: ['./comment-recommendation.component.css'],
})
export class CommentComponent implements OnInit {

	@Input() RecommentId: number
	@Input() IsPagePublic: boolean

	isLogin: boolean = this.storeageService.getIsHaveToken()
	typeObject: number = this.storeageService.getTypeObject()
	constructor(
		private commentService: RecommendationCommentService, private _toastr: ToastrService, private storeageService: UserInfoStorageService
	) { }

	ngOnInit() {

	}

	ngAfterViewInit() {
		this.getCommentPaged()
	}

	commentModel: RecommnendationCommentObject = new RecommnendationCommentObject()
	commentQuery: any = {
		pageSize: 5,
		pageIndex: 1,
		recommendationId: 0,
		isPublish: false,
	}
	listCommentsPaged: any[] = []
	total_Comments = 0
	total_CommentCombineChild = 0
	pageIndexComment: number = 1
	IsAllComment: boolean = false

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

	onSendComment() {
		if (this.isLogin == false) {
			this._toastr.error('Vui lòng đăng nhập để gửi bình luận')
			return
		}
		this.commentModel.recommendationId = this.RecommentId
		this.commentModel.contents = this.commentModel.contents == null ? '' : this.commentModel.contents.trim()
		this.commentModel.isPublish = false
		if (this.commentModel.contents == null || this.commentModel.contents == '') {
			this._toastr.error('Không bỏ trống nội dung bình luận')
			return
		}

		this.commentService.insert(this.commentModel).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				this._toastr.error(res.message)
				return
			}
			this._toastr.success('Bình luận của bạn sẽ được chuyển đến quản trị viên để phê duyệt')
			this.commentModel = new RecommnendationCommentObject()
			// this.getCommentPaged()
		})
	}

	getCommentPaged() {
		this.commentQuery.pageIndex = this.pageIndexComment
		this.commentQuery.recommendationId = this.RecommentId
		this.commentService.getAllOnPage(this.commentQuery).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.MRCommnentGetAllOnPage.length > 0) {
					this.total_Comments = res.result.TotalCount
					this.total_CommentCombineChild = res.result.Total
					this.listCommentsPaged = this.listCommentsPaged != null ? this.listCommentsPaged.concat(res.result.MRCommnentGetAllOnPage) : res.result.MRCommnentGetAllOnPage
					if (this.listCommentsPaged.length == res.result.TotalCount) {
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
		this.pageIndexComment += 1
		this.getCommentPaged()
	}

	loadCommentChild(item: any) {
		item.pageIndexChild = item.pageIndexChild == null ? 1 : item.pageIndexChild
		this.commentService.getPageByParentId({ ParentId: item.id, PageIndex: item.pageIndexChild }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				item.listChild = item.listChild == null ? res.result : item.listChild.concat(res.result)
				item.pageIndexChild += 1
				this.listCommentsPaged = [...this.listCommentsPaged]
			}
		}, (err) => {
			console.log(err)
		})
	}

	preCreateCommentChild(item) {
		if (item.listChild) {
			var element = document.getElementById("demo_" + (item.listChild.length - 1))
			const elementRect = document.getElementById("demo_" + (item.listChild.length - 1)).getBoundingClientRect()
			const absoluteElementTop = elementRect.top;
			const middle = absoluteElementTop - (elementRect.height / 2);
			window.scrollTo(0, absoluteElementTop);
		}

		item.IsShowChild = !item.IsShowChild
	}
}
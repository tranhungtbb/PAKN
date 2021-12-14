import { Component, Input, OnInit, AfterViewInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { RESPONSE_STATUS, RECOMMENDATION_STATUS } from 'src/app/constants/CONSTANTS'
import { ViewChild } from '@angular/core'
import { RecommendationCommentService } from 'src/app/services/recommendation-comment.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RecommnendationCommentObject, RecommnendationInfomationExchange } from 'src/app/models/recommendationObject'

declare var $: any

@Component({
	selector: 'app-infomation-exchange',
	templateUrl: './infomation-exchange.component.html',
	styleUrls: ['./infomation-exchange.component.css'],
})
export class InfomationExchangeComponent implements OnInit {

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
		this.getInfomationExchangePaged()
	}



	model: RecommnendationInfomationExchange = new RecommnendationInfomationExchange()
	commentQuery: any = {
		pageSize: 5,
		pageIndex: 1,
		recommendationId: 0,
	}
	listData: any[] = []
	total_Comments = 0
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

	onSendInfomationExchange() {
		// fullName: string
		// createdDate: Date
		this.model.fullName = this.storeageService.getFullName()
		this.model.createdDate = new Date()
		this.model.recommendationId = this.RecommentId
		this.model.contents = this.model.contents == null ? '' : this.model.contents.trim()
		this.model.isPublish = false
		if (this.model.contents == null || this.model.contents == '') {
			this._toastr.error('Không bỏ trống nội dung trao đổi')
			return
		}

		this.commentService.insertInfomationExchange(this.model).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				this._toastr.error(res.message)
				return
			}
			this._toastr.success('Thêm trao đổi thành công')
			this.listData.push(this.model)
			this.total_Comments += 1
			this.model = new RecommnendationInfomationExchange()
			// this.getInfomationExchangePaged()
		})
	}

	getInfomationExchangePaged() {
		this.commentQuery.pageIndex = this.pageIndexComment
		this.commentQuery.recommendationId = this.RecommentId
		this.commentService.getAllInfomationChangeOnPage(this.commentQuery).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.MRInfomationExchangeAllOnPage.length > 0) {
					this.total_Comments = res.result.TotalCount
					this.listData = this.listData != null ? this.listData.concat(res.result.MRInfomationExchangeAllOnPage) : res.result.MRInfomationExchangeAllOnPage
					if (this.listData.length == res.result.TotalCount) {
						this.IsAllComment = true
					}
				} else {
					this.listData = []
					this.IsAllComment = true
				}
			}
		})
	}
	loadComment() {
		this.pageIndexComment += 1
		this.getInfomationExchangePaged()
	}

	loadInfomationExchangeChild(item: any) {
		item.pageIndexChild = item.pageIndexChild == null ? 1 : item.pageIndexChild
		this.commentService.getPageByParentId({ ParentId: item.id, PageIndex: item.pageIndexChild }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				item.listChild = item.listChild == null ? res.result : item.listChild.concat(res.result)
				item.pageIndexChild += 1
				this.listData = [...this.listData]
			}
		}, (err) => {
			console.log(err)
		})
	}

	preCreateInfomationExchangeChild(item) {
		item.IsShowChild = !item.IsShowChild
	}
}
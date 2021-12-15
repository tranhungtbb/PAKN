import { Component, Input, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { ViewChild } from '@angular/core'
import { RecommendationCommentService } from 'src/app/services/recommendation-comment.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RecommnendationInfomationExchange } from 'src/app/models/recommendationObject'


@Component({
	selector: 'app-infomation-exchange-reply',
	templateUrl: './infomation-exchange-reply.component.html',
	styleUrls: ['./infomation-exchange-reply.component.css'],
})
export class InfomationExchangeReplyComponent implements OnInit {

	@Input() ParentId: number
	@Input() RecommentId: number


	IsShow: boolean = true


	public id: any = 0
	isLogin: boolean = this.storeageService.getIsHaveToken()
	typeObject: number = this.storeageService.getTypeObject()
	constructor(
		private commentService: RecommendationCommentService, private _toastr: ToastrService, private storeageService: UserInfoStorageService
	) { }

	ngOnInit() {

	}
	model: RecommnendationInfomationExchange = new RecommnendationInfomationExchange()

	onSave() {

		this.model.recommendationId = this.RecommentId
		this.model.parentId = this.ParentId
		this.model.contents = this.model.contents == null ? '' : this.model.contents.trim()
		this.model.isPublish = false
		if (this.model.contents == null || this.model.contents == '') {
			this._toastr.error('Không bỏ trống nội dung trao đổi')
			return
		}

		this.commentService.insert(this.model).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				this._toastr.error(res.message)
				return
			}
			this._toastr.success('Thêm mới thành công')
			this.model = new RecommnendationInfomationExchange()
		})
	}
}
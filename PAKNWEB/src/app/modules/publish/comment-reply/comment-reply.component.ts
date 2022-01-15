import { Component, Input, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { ViewChild } from '@angular/core'
import { RecommendationCommentService } from 'src/app/services/recommendation-comment.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RecommnendationCommentObject } from 'src/app/models/recommendationObject'


@Component({
	selector: 'app-comment-reply',
	templateUrl: './comment-reply.component.html',
	styleUrls: ['./comment-reply.component.css'],
})
export class CommentReplyComponent implements OnInit {

	@Input() ParentId: number
	@Input() RecommentId: number


	IsShow: boolean = true


	public id: any = 0
	public model: any
	isLogin: boolean = this.storeageService.getIsHaveToken()
	typeObject: number = this.storeageService.getTypeObject()
	constructor(
		private commentService: RecommendationCommentService, private _toastr: ToastrService, private storeageService: UserInfoStorageService
	) { }

	ngOnInit() {

	}
	commentModel: RecommnendationCommentObject = new RecommnendationCommentObject()

	onSave() {
		if (this.isLogin == false) {
			this._toastr.error('Vui lòng đăng nhập để gửi bình luận')
			return
		}
		this.commentModel.recommendationId = this.RecommentId
		this.commentModel.parentId = this.ParentId
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
		})
	}
}
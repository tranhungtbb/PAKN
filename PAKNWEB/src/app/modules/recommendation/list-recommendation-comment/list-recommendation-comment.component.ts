import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RecommendationObject, RecommendationSearchObject } from 'src/app/models/recommendationObject'
import { DataService } from 'src/app/services/sharedata.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { Router } from '@angular/router'
import { RecommendationCommentService } from 'src/app/services/recommendation-comment.service'
import { COMMONS } from 'src/app/commons/commons'

declare var $: any

@Component({
	selector: 'app-list-recommendation-comment',
	templateUrl: './list-recommendation-comment.component.html',
	styleUrls: ['./list-recommendation-comment.component.css'],
})
export class ListRecommendationCommentComponent implements OnInit {
	constructor(
		private _service: RecommendationCommentService,
		private _toastr: ToastrService,
		private _shareData: DataService,
		private _router: Router
	) {}

	listData : any [] = []
	dataSearch : any = {
		fullName : '',
		contents: '',
		titleRecommendation : '',
	}
	listStatus: any = [
		{ value: true, text: 'Đã công bố' },
		{ value: false, text: 'Đã thu hồi' }
	]
	pageIndex: number = 1
	pageSize: number = 20
	createdDate : Date = null
	isPublish = null

	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	id: number = 0

	ngOnInit() {
		this.getList()
	}

	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	dateChange(event) {
		if (event) {
			this.createdDate = event
		} else {
			this.createdDate = null
		}
		this.getList()
	}
	

	getList() {
		this.dataSearch.fullName = this.dataSearch.fullName == null ? '' : this.dataSearch.fullName.trim()
		this.dataSearch.contents = this.dataSearch.contents == null ? '' : this.dataSearch.contents.trim()
		this.dataSearch.titleRecommendation = this.dataSearch.titleRecommendation == null ? '' : this.dataSearch.titleRecommendation.trim()
		this.isPublish = this.isPublish //== null ?  : this.isPublish
		let request = {
			FullName : this.dataSearch.fullName,
			Contents: this.dataSearch.contents,
			RecommendationTitle : this.dataSearch.titleRecommendation,
			IsPublish : this.isPublish == null ? '' : this.isPublish,
			CreatedDate : this.createdDate == null ? '' : this.createdDate.toDateString(),
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}

		this._service.getAllOnPageBase(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = response.result.MRCommnentGetAllOnPage
					this.totalRecords = response.result.TotalCount
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	onPageChange(event: any) {
		this.pageSize = event.rows
		this.pageIndex = event.first / event.rows + 1
		this.getList()
	}

	dataStateChange() {
		this.pageIndex = 1
		this.table.first = 0
		this.getList()
	}
	titleConfirm : string
	preChangeStatus = (item : any) =>{
		this.id = item.id
		this.titleConfirm = item.isPublish == true ? 'Anh/Chị có chắc chắn muốn thu hồi bình luận này?' : 'Anh/Chị có chắc chắn muốn công bố bình luận này?'
		$('#modalConfirmChangeStatus').modal('show')
	}

	onChangeStatus = ()=>{
		let obj : any = this.listData.find(x=>x.id === this.id)
		this._service.updateStatus({Id : this.id, IsPublish : !obj.isPublish}).subscribe(res =>{
			if(res.success == RESPONSE_STATUS.success){
				this._toastr.success(COMMONS.UPDATE_SUCCESS)
				$('#modalConfirmChangeStatus').modal('hide')
				this.getList()
			}else{
				this._toastr.error(res.message)
			}
		},(err) =>{
			console.log(err)
		})
	}


	preDelete = (id : number) =>{
		this.id = id
		$('#modalConfirmDelete').modal('show')
	}
	onDelete = () =>{
		this._service.delete({Id : this.id}).subscribe(res =>{
			if(res.success == RESPONSE_STATUS.success){
				this._toastr.success(COMMONS.DELETE_SUCCESS)
				$('#modalConfirmDelete').modal('hide')
				this.getList()
			}else{
				this._toastr.error(res.message)
			}
		},(err) =>{
			console.log(err)
		})
	}
}

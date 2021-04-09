import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, FILETYPE, MESSAGE_COMMON, RECOMMENDATION_STATUS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { RecommendationObject, RecommendationProcessObject } from 'src/app/models/recommendationObject'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { HashtagObject } from 'src/app/models/hashtagObject'
import { CatalogService } from 'src/app/services/catalog.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { AppSettings } from 'src/app/constants/app-setting'
import { Api } from 'src/app/constants/api'
import { CaptchaService } from 'src/app/services/captcha-service'
import { UserService } from 'src/app/services/user.service'

declare var $: any
@Component({
	selector: 'app-my-recommendation',
	templateUrl: './my-recommendation.component.html',
	styleUrls: ['./my-recommendation.component.css'],
})
export class MyRecommendationComponent implements OnInit {
	userName: string = "";
	phone: string = "";
	recommandationId: number = 0;
	pageIndex: number = 1;
	pageSize: number = 20;
	@ViewChild('table', { static: false }) table: any;
	totalRecords: number = 0;
	listData = new Array<RecommendationObject>();
	constructor(
		private toastr: ToastrService,
		private fileService: UploadFileService,
		private recommendationService: RecommendationService,
		private storageService: UserInfoStorageService,
		private _serviceCatalog: CatalogService,
		private router: Router,
		private captchaService: CaptchaService,
		private userService: UserService,

	) { }

	ngOnInit() {
		this.userName = this.storageService.getFullName();
		this.getSoDienThoai();
		this.getList();
	}

	getSoDienThoai() {
		this.userService.getById({ id: this.storageService.getUserId() }).subscribe((res) => {
			if (res.success != 'OK') return
			var modelUser = res.result.SYUserGetByID[0];
			if (modelUser && modelUser.phone) {
				this.phone = modelUser.phone;
			}
		})
	}

	getList() {
		let request = {
			Code: '',
			SendName: '',
			Content: '',
			UnitId: '',
			Field: '',
			Status: '',
			UnitProcessId: '',
			UserProcessId: this.storageService.getUserId(),
			PageIndex: this.pageIndex,
			PageSize: this.pageSize,
		}

		this.recommendationService.recommendationGetListProcess(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.MRRecommendationGetAllWithProcess
					this.totalRecords = response.result.TotalCount
				}
			} else {
				this.toastr.error(response.message)
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
		// this.table.first = 0
		this.getList()
	}

	sendRecommandation() {
		this.router.navigateByUrl('/cong-bo/them-moi-kien-nghi');
	}

	//delete recommandateion
	preDelete(id: number) {
		this.recommandationId = id
		$('#modalConfirmDelete').modal('show')
	}

	onDelete(id: number) {
		let request = {
			Id: id,
		}
		this.recommendationService.recommendationDelete(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
				$('#modalConfirmDelete').modal('hide')
				this.getList()
			} else {
				this.toastr.error(response.message)
			}
		}),
			(error) => {
				console.error(error)
			}
	}

	preSend(id: any) {
		this.recommandationId = id
		$('#modalConfirmSend').modal('show')
	}
	onSend(status) {
		var request = {
			id: this.recommandationId,
			status: status,
		}
		this.recommendationService.recommendationUpdateStatus(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				$('#modalConfirmSend').modal('hide')
				this.toastr.success(COMMONS.ACCEPT_SUCCESS)
				this.getList()
			} else {
				this.toastr.error(response.message)
			}
		}),
			(err) => {
				console.error(err)
			}
	}
}

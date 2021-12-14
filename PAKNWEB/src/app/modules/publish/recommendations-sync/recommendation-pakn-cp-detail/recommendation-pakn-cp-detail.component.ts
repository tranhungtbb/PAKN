import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { RecommendationRequestService } from 'src/app/services/recommendation-req.service'
import { ActivatedRoute, Router } from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { AppSettings } from 'src/app/constants/app-setting'
import {RecommandationSyncService} from 'src/app/services/recommandation-sync.service'

declare var $: any

@Component({
	selector: 'app-detail-recommendation-pakn-cp-detail-public',
	templateUrl: './recommendation-pakn-cp-detail.component.html',
	styleUrls: ['./recommendation-pakn-cp-detail.component.css'],
})
export class DetailRecommendationPaknCPComponent implements OnInit {
	id : any
	files : any = []
	filesRequest : any = []
	APIADDRESS: any
	modelData: RequestData = new RequestData()
	constructor(
		private toastr: ToastrService,
		private storeageService: UserInfoStorageService,
		private recommendationService: RecommandationSyncService,
		private router: Router,
		private activatedRoute: ActivatedRoute
	) {
		this.APIADDRESS = AppSettings.API_ADDRESS.replace('api/', '')
		this.modelData = new RequestData()
	}

	ngOnInit() {
		this.activatedRoute.params.subscribe((params) => {
			this.id = +params['id']
			if (this.id != 0) {
				this.getData()
			}
		})
	}

	getData() {
		let request = {
			Id: this.id,
		}
		this.recommendationService.getHeThongPANKChinhPhuGetByObjectId(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if(response.result.MRRecommendationPAKNCPGetById.length > 0){
					this.modelData = response.result.MRRecommendationPAKNCPGetById[0]
					console.log(this.modelData.questionContent)
					this.files = response.result.FileAttach.filter((x)=>x.isReply == true)
					this.filesRequest = response.result.FileAttach.filter((x)=>x.isReply == false)
				}else{
					this.modelData = null
					this.files = []
					this.filesRequest = []
				}
			} else {
				this.toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
	}

	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()

		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}
}
export class RequestData {
	id : number
	questioner : string
	question : string
	questionContent : string
	reply : string
	createdDate : string
	status : number
	objectId : number

	constructor(){
		this.questioner = ''
		this.question = ''
		this.questionContent = ''
		this.reply = ''
	}
}

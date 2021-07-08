import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { ActivatedRoute, Router } from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { AppSettings } from 'src/app/constants/app-setting'
import {RecommandationSyncService} from 'src/app/services/recommandation-sync.service'

declare var $: any

@Component({
	selector: 'app-detail-he-thong-pakn-chinh-phu',
	templateUrl: './detail-he-thong-pakn-chinh-phu.component.html',
	styleUrls: ['./detail-he-thong-pakn-chinh-phu.component.css'],
})
export class DetailHeThongPAKNChinhPhuComponent implements OnInit {
	id : any
	file : any = []
	APIADDRESS: any
	modelData: RequestData = new RequestData()
	constructor(
		private toastr: ToastrService,
		private storeageService: UserInfoStorageService,
		private _service: RecommandationSyncService,
		private router: Router,
		private activatedRoute: ActivatedRoute
	) {
		this.APIADDRESS = AppSettings.API_ADDRESS.replace('api/', '')
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
		this._service.getHeThongPANKChinhPhuGetByObjectId(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.modelData = response.result.MRRecommendationDVCGetById[0]
				this.file = response.result.FileAttach

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

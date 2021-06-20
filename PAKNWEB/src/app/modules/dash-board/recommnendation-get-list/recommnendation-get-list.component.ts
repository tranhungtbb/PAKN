import { Component, OnInit } from '@angular/core'
import { RecommendationService } from 'src/app/services/recommendation.service'
import { ToastrService } from 'ngx-toastr'
import {Router} from '@angular/router'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
declare var $: any

@Component({
	selector: 'app-recommnendation-get-list',
	templateUrl: './recommnendation-get-list.component.html',
	styleUrls: ['./recommnendation-get-list.component.css'],
})
export class RecommnendationGetListComponent implements OnInit {
	constructor(private _service: RecommendationService, private _toast: ToastrService, private storeageService: UserInfoStorageService, private router : Router) {}

	listData: any[] =[]
	dataSearch: any = {}
	totalRecords: number = 0
	isMain: boolean = true
	title : any 
	ngOnInit() {
		this.isMain = this.storeageService.getIsMain()
		this.getList()
		if(this.isMain == true){
			this.title = 'Danh sách phản ánh kiến nghị chờ xử lý'
		}else{
			this.title =  'Danh sách phản ánh kiến nghị chưa giải quyết'
		}
	}
	
	getList() {
		let request = {
			Status: this.isMain == true ? 2 : 5,
			PageIndex: 1,
			PageSize: 4,
			UnitProcessId: this.storeageService.getUnitId(),
			UserProcessId: this.storeageService.getUserId(),
		}
		this._service.recommendationGetListProcess(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result != null) {
					this.listData = []
					this.listData = response.result.MRRecommendationGetAllWithProcess
				
					this.totalRecords = response.result.TotalCount
				}
			} else {
				this._toast.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
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
	redirectList(){
		this.router.navigate(['/quan-tri/kien-nghi/cho-giai-quyet'])
	}
}

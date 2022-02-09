import { AfterViewInit, Component, OnInit } from '@angular/core'
import { UserInfoStorageService } from '../../commons/user-info-storage.service'
import { HasPermission } from 'src/app/guards/has-permission.service'
import { UnitService } from 'src/app/services/unit.service'

declare var $: any
import { Router } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { StatisticService } from 'src/app/services/statistic.service'
import { RecommendationService } from 'src/app/services/recommendation.service'

@Component({
	selector: 'app-appmenu',
	templateUrl: './appmenu.component.html',
	styleUrls: ['./appmenu.component.css'],
})
export class AppmenuComponent implements OnInit, AfterViewInit {
	isSuperAdmin: boolean = false
	constructor(private userStorage: UserInfoStorageService, private recommendationService: RecommendationService, private _router: Router, private unitService: UnitService, private _service: StatisticService) {
		this.recommendationService.onReloadMenu.subscribe(() => {
			this.statisticRecommendationForMenu()
		})
	}

	isAdmin = this.userStorage.getIsAdmin()
	hasPermissionSMS: boolean = false
	statistic: any = {}
	isMainMenu: boolean = this.userStorage.getIsUnitMain()
	isApprove = this.userStorage.getIsApprove()
	unitLevel = this.userStorage.getUnitLevel()

	ngOnInit() {
		this.unitService.hasPermissionSMS({}).subscribe(res => {
			if (res.success == RESPONSE_STATUS.success) {
				this.hasPermissionSMS = res.result
			}
		})
		this.statisticRecommendationForMenu()
	}

	statisticRecommendationForMenu() {
		this._service.getStatisticRecommendationForMenu({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				this.statistic = response.result
			} else {
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	ngAfterViewInit() {
		this.loadScriptMenus('assets/dist/js/custom.min.js')
		this.loadScriptMenus('assets/dist/js/deznav-init.js')
	}
	public loadScriptMenus(url: string) {
		$('script[src="' + url + '"]').remove()
		$('<script>').attr('src', url).appendTo('body')
	}
}

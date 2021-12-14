import { Component, OnInit, ViewChild } from '@angular/core'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { AppSettings } from 'src/app/constants/app-setting'

import { IntroduceService } from 'src/app/services/introduce.service'
import { IntroduceObjet, IntroduceFunction, IntroduceUnit } from 'src/app/models/IntroductObject'
import { Router } from '@angular/router'

@Component({
	selector: 'app-introduce',
	templateUrl: './introduce.component.html',
	styleUrls: ['./introduce.component.css'],
})
export class IntroduceComponent implements OnInit {
	// property
	model: any = new IntroduceObjet()
	ltsIntroductUnit: Array<IntroduceUnit>
	lstIntroduceFunction: Array<IntroduceFunction>
	isPreview: boolean = false

	constructor(private _service: IntroduceService, private _router: Router) {
		this.lstIntroduceFunction = []
	}

	currentDate = new Date()

	ngOnInit() {
		this.isPreview = this._router.url.includes('xem-truoc') ? true : false
		this._service.GetInfo({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.model = res.result.model
				this.model.bannerUrl = this.model.bannerUrl
				this.lstIntroduceFunction = res.result.lstIntroduceFunction
				this.ltsIntroductUnit = res.result.lstIntroduceUnit
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}
}

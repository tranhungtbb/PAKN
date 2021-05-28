import { Component, OnInit, ViewChild } from '@angular/core'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { AppSettings } from 'src/app/constants/app-setting'

import { IntroduceService } from 'src/app/services/introduce.service'
import { ViewRightComponent } from 'src/app/modules/publish/view-right/view-right.component'
import { IntroduceObjet, IntroduceFunction, IntroduceUnit } from 'src/app/models/IntroductObject'

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
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
	constructor(private _service: IntroduceService) {
		this.lstIntroduceFunction = []
		for (var i = 0; i < 6; i++) {
			this.lstIntroduceFunction.push(new IntroduceFunction())
		}
	}

	ngOnInit() {
		this._service.GetInfo({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.model = res.result.model
				this.model.bannerUrl = this.model.bannerUrl
				this.lstIntroduceFunction = res.result.lstIntroduceFunction
				this.ltsIntroductUnit = res.result.lstIntroduceUnit

				console.log(this.model)
				console.log(this.lstIntroduceFunction)
				console.log(this.ltsIntroductUnit)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}
}

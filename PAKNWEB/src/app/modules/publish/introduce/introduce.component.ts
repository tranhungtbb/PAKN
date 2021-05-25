import { Component, OnInit, ViewChild } from '@angular/core'

import { ViewRightComponent } from 'src/app/modules/publish/view-right/view-right.component'
@Component({
	selector: 'app-introduce',
	templateUrl: './introduce.component.html',
	styleUrls: ['./introduce.component.css'],
})
export class IntroduceComponent implements OnInit {
	// property
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
	constructor() {}

	ngOnInit() {}
}

import { Component, OnInit, ViewChild } from '@angular/core'
import { ViewRightComponent } from 'src/app/modules/publish/view-right/view-right.component'

@Component({
	selector: 'app-view-administrative-procedures',
	templateUrl: './view-administrative-procedures.component.html',
	styleUrls: ['./view-administrative-procedures.component.css'],
})
export class ViewAdministrativeProceduresComponent implements OnInit {
	constructor() {}
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
	ngOnInit() {}
}

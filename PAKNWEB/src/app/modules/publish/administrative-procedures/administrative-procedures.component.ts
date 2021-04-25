import { Component, OnInit, ViewChild } from '@angular/core'
import { ViewRightComponent } from 'src/app/modules/publish/view-right/view-right.component'

@Component({
	selector: 'app-administrative-procedures',
	templateUrl: './administrative-procedures.component.html',
	styleUrls: ['./administrative-procedures.component.css'],
})
export class AdministrativeProceduresComponent implements OnInit {
	constructor() {}
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
	ngOnInit() {}
}

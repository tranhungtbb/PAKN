import { Component, OnInit } from '@angular/core'

import { UnitService } from '../../../../services/unit.service'

declare var $: JQuery
declare var jquery: JQuery
@Component({
	selector: 'app-unit',
	templateUrl: './unit.component.html',
	styleUrls: ['./unit.component.css'],
})
export class UnitComponent implements OnInit {
	listUnit: any[]

	query: any = {}

	constructor(private unitService: UnitService) {}

	ngOnInit() {
		this.loadData()
	}

	loadData(): void {
		this.unitService.getAllPagedList(this.query).subscribe((res) => {
			console.log(res)
		})
	}
}

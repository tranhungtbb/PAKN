import { Component, OnInit, ViewChild } from '@angular/core'

@Component({
	selector: 'app-list-unit',
	templateUrl: './list-unit.component.html',
	styleUrls: ['./list-unit.component.css'],
})
export class ListUnitComponent implements OnInit {
	listUnit: any[]
	reqObject: {
		pageIndex: 1
		pageSize: 20
		sort: ''
		parentId: number
		Level: number
		searchName: ''
		searchPhone: ''
		searchEmail: ''
		searchAddress: ''
		searchActive: boolean
	}

	@ViewChild('dataTable', { static: false }) public dataTable: any

	constructor() {}

	ngOnInit() {}

	onPageChange(ev: any) {}
}

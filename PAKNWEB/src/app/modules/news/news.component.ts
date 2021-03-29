import { Component, OnInit } from '@angular/core'

@Component({
	selector: 'app-news',
	templateUrl: './news.component.html',
	styleUrls: ['./news.component.css'],
})
export class NewsComponent implements OnInit {
	constructor() {}

	query: any = {
		pageIndex: '',
		pageSize: '',
		title: '',
		status: '',
		newType: '',
	}
	listNewCategories: any[] = []
	listStatus: any[] = [
		{ value: 0, text: 'Đã thu hồi' },
		{ value: 1, text: 'Đã công bố' },
		{ value: 2, text: 'Đang soạn thảo' },
	]
	ngOnInit() {}

	getListPaged() {}
}

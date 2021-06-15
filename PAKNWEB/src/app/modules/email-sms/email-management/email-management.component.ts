import { Component, OnInit } from '@angular/core'

@Component({
	selector: 'app-email-management',
	templateUrl: './email-management.component.html',
	styleUrls: ['./email-management.component.css'],
})
export class EmailManagementComponent implements OnInit {
	constructor() {}

	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
	]

	listCategory: any = [
		{ value: '1', text: 'Cá nhân' },
		{ value: '2', text: 'Doanh nghiệp' },
	]

	listHisStatus: any = [
		{ value: '0', text: 'Khởi tạo' },
		{ value: '1', text: 'Cập nhập' },
		{ value: '2', text: 'Đã gửi' },
	]

	listData: any[] = []
	query = {
		pageIndex: 1,
		pageSize: 20,
		title: '',
		unitId: '',
		objectType: '',
		status: '',
	}

	ngOnInit() {}
}

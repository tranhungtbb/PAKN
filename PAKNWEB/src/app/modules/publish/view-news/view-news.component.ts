import { Component, OnInit } from '@angular/core'

import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { NewsService } from 'src/app/services/news.service'

@Component({
	selector: 'app-news',
	templateUrl: './view-news.component.html',
	styleUrls: ['./view-news.component.css'],
})
export class ViewNewsComponent implements OnInit {
	constructor(private newsService: NewsService, private router: Router, private activatedRoute: ActivatedRoute) {}

	model: any = {}

	ngOnInit() {
		this.activatedRoute.params.subscribe((params) => {
			if (params['id']) {
				this.getData(params['id'])
			}
		})
	}

	getData(id) {
		this.newsService.getViewDetail({ id }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.model = res.result.NENewsViewDetail[0]
			}
		})
	}
}

import { Component, OnInit, ViewChild } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { ViewRightComponent } from 'src/app/modules/publish/view-right/view-right.component'
import { CatalogService } from 'src/app/services/catalog.service'
import { NewsService } from 'src/app/services/news.service'

@Component({
	selector: 'app-news-hight-light',
	templateUrl: './news-hight-light.component.html',
	styleUrls: ['./news-hight-light.component.css'],
})
export class NewsHightLightComponent implements OnInit {
	@ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
	listData: any[] = []

	constructor(private _toastr: ToastrService, private newsService: NewsService, private _router: Router) {}

	ngOnInit() {
		this.newsService.getListHomePage({}).subscribe((res) => {
			if (res.success != RESPONSE_STATUS.success) {
				return
			}
			if (res.result.length > 0) {
				this.listData = res.result
			}
			return
		})
	}
	redirectDetailNews(id: any) {
		this._router.navigate(['/cong-bo/tin-tuc-su-kien/', id])
	}
}

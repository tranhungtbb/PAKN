import { Component, OnInit, ViewEncapsulation } from '@angular/core'
import { PuSupportService } from 'src/app/services/pu-support.service'
import { DomSanitizer, SafeUrl } from '@angular/platform-browser'
import { SupportService } from 'src/app/services/support.service'
import { RESPONSE_STATUS, TYPE_SUPPORT } from 'src/app/constants/CONSTANTS'

@Component({
	selector: 'app-support',
	templateUrl: './support.component.html',
	styleUrls: ['./support.component.css']
})
export class SupportComponent implements OnInit {
	constructor(private supportService: SupportService, private sanitizer: DomSanitizer) {

	}

	listData: any[] = []
	model: any

	ngOnInit() {
		this.getList()
	}

	getList() {
		this.supportService.GetListByType({ Type: TYPE_SUPPORT.PUBLIC }).subscribe(
			(res) => {
				if (res.success != RESPONSE_STATUS.success) return
				this.listData = res.result
				if (this.listData && this.listData.length > 0) {
					this.selectMenu(this.listData[0].id)
				}
			},
			(err) => {
				console.log(err)
			}
		)
	}

	selectMenu = (id: any) => {

		this.model = this.listData.find(x => x.id == id)
	}

}

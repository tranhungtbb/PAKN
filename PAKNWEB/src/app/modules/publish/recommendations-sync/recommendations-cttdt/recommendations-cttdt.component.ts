import { Component, OnInit, ViewChild } from '@angular/core'
import { Router } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import {RecommandationSyncService} from 'src/app/services/recommandation-sync.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'

@Component({
	selector: 'app-recommendations-cttdt-public',
	templateUrl: './recommendations-cttdt.component.html',
	styleUrls: ['./recommendations-cttdt.component.css'],
})
export class RecommendationsCttdtComponent implements OnInit {
	// property
	public KeySearch: string = ''
	public PageSize = 20
	public PageIndex = 1
	public Total = 0

	pagination = []

	// arr

	listData : any = []

	constructor(private service: RecommandationSyncService, private routers: Router, private userService : UserInfoStorageService) {}

	ngOnInit() {
		this.getList()
	}

	redirect(id: any) {
		this.routers.navigate(['/cong-bo/phan-anh-kien-nghi/sync/cong-ttdt-tinh-khanh-hoa/' + id])
	}

	changeKeySearch(event) {
		this.KeySearch = event.target.value
	}

	getList() {
		this.KeySearch = this.KeySearch.trim()
	
		var obj = {
			questioner: this.KeySearch,
			question: this.KeySearch, //FINISED
			pageSize: this.PageSize,
			pageIndex: this.PageIndex,
		}
		this.service.getCongThongTinDienTuTinhPagedList(obj).subscribe((res) => {
			if (res != 'undefined' && res.success == RESPONSE_STATUS.success) {
				if (res.result.Data.length > 0) {
					this.listData = res.result.Data
					.map((item) => {
						item.shortName = this.getShortName(item.questioner)
						return item
					})
					this.Total = res.result.Data[0].rowNumber
					this.padi()
				} else {
					this.listData = this.pagination = []
					this.PageIndex = 1
					this.Total = 0
				}
			} else {
				this.listData = this.pagination = []
				this.PageIndex = 1
				this.Total = 0
			}
		})

	}
	getShortName(string) {
		var names = string.split(' '),
			initials = names[0].substring(0, 1).toUpperCase()
		if (names.length > 1) {
			initials += names[names.length - 1].substring(0, 1).toUpperCase()
		}
		return initials
	}

	padi() {
		this.pagination = []
		for (let i = 0; i < Math.ceil(this.Total / this.PageSize); i++) {
			this.pagination.push({ index: i + 1 })
		}
	}

	changePagination(index: any) {
		if (this.PageIndex > index) {
			if (index > 0) {
				this.PageIndex = index
				this.getList()
			}
			return
		} else if (this.PageIndex < index) {
			if (this.pagination.length >= index) {
				this.PageIndex = index
				this.getList()
			}
			return
		}
		return
	}
	redirectCreateRecommendation() {
		this.routers.navigate(['/cong-bo/them-moi-kien-nghi'])
	}
}

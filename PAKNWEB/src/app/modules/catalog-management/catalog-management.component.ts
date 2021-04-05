import { Component, OnInit } from '@angular/core'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'
defineLocale('vi', viLocale)
@Component({
	selector: 'app-catalog-management',
	template: ` <div class="content-body"><router-outlet> </router-outlet></div> `,
})
export class CatalogManagementComponent implements OnInit {
	constructor(private localeService: BsLocaleService) {}

	ngOnInit() {
		this.localeService.use('vi')
	}
}

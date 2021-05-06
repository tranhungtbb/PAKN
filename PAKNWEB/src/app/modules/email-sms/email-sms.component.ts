import { Component, OnInit } from '@angular/core'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'
@Component({
	selector: 'app-email-sms',
	// templateUrl: './email-sms.component.html',
	// styleUrls: ['./email-sms.component.css']
	template: `<router-outlet> </router-outlet> `,
})
export class EmailSmsComponent implements OnInit {
	constructor(private localeService: BsLocaleService) {}

	ngOnInit() {
		this.localeService.use('vi')
	}
}

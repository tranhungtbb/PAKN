import { Component, OnInit, AfterViewChecked, ChangeDetectorRef } from '@angular/core'
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker'
import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'

defineLocale('vi', viLocale)

@Component({
	selector: 'app-business-individual',
	templateUrl: './business-individual.component.html',
	styleUrls: ['./business-individual.component.css'],
})
export class BusinessIndividualComponent implements OnInit {
	constructor(private cdRef: ChangeDetectorRef, private localeService: BsLocaleService) {}

	ngOnInit() {
		this.localeService.use('vi')
	}

	ngAfterViewChecked(): void {
		this.cdRef.detectChanges()
	}
}

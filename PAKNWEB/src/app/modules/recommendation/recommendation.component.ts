import { AfterViewChecked, ChangeDetectorRef, Component, OnInit } from '@angular/core'
import { BsLocaleService } from 'ngx-bootstrap/datepicker'

@Component({
	selector: 'app-recommendation',
	templateUrl: './recommendation.component.html',
	styleUrls: ['./recommendation.component.css'],
})
export class RecommendationComponent implements OnInit, AfterViewChecked {
	constructor(private cdRef: ChangeDetectorRef, private localeService: BsLocaleService) {}

	ngOnInit() {
		this.localeService.use('vi')
	}

	ngAfterViewChecked(): void {
		this.cdRef.detectChanges()
	}
}

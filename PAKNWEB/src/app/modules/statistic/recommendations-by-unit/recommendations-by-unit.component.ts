import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { defineLocale } from 'ngx-bootstrap/chronos'
import { viLocale } from 'ngx-bootstrap/locale'
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker'

import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { CONSTANTS, STATUS_HISNEWS, FILETYPE } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'

declare var $: any
defineLocale('vi', viLocale)

@Component({
	selector: 'app-recommentdation-by-unit',
	templateUrl: './recommendations-by-unit.component.html',
	styleUrls: ['./recommendations-by-unit.component.css'],
})
export class RecommendationsByUnitComponent implements OnInit {
	@ViewChild('file', { static: false }) public file: ElementRef
	constructor(private _toastr: ToastrService, private router: Router, private activatedRoute: ActivatedRoute, private BsLocaleService: BsLocaleService) {}

	ngOnInit() {
		this.BsLocaleService.use('vi')
	}
}

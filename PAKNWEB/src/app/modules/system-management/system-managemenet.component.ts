import { Component, OnInit, AfterViewChecked, ChangeDetectorRef } from '@angular/core';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { viLocale } from 'ngx-bootstrap/locale';

defineLocale('vi', viLocale);

@Component({
  selector: 'app-system-managemenet',
  templateUrl: './system-managemenet.component.html',
  styleUrls: ['./system-managemenet.component.css']
})
export class SystemManagemenetComponent implements OnInit, AfterViewChecked {

  constructor(private cdRef: ChangeDetectorRef,
    private localeService: BsLocaleService,
  ) { }

  ngOnInit() {
    this.localeService.use('vi');
  }

  ngAfterViewChecked(): void {
    this.cdRef.detectChanges();
  }
}

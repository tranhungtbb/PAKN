import { Component, OnInit } from '@angular/core';
import { DashBoardService } from '../../services/dashboard.service';
import { UserInfoStorageService } from '../../commons/user-info-storage.service';
import { DatepickerDateCustomClasses } from 'ngx-bootstrap/datepicker/models';
import { DataService } from '../../services/sharedata.service';
import { Router } from '@angular/router';
import { ResolutionObject } from 'src/app/models/resolution';
import { ToastrService } from 'ngx-toastr';

declare var $: any;

@Component({
  selector: 'app-dashboard',
  templateUrl: './dash-board.component.html',
  styleUrls: ['./dash-board.component.css']
})
export class DashboardComponent implements OnInit {

  constructor(
    private _toast: ToastrService,
    private service: DashBoardService,
    private userStorage: UserInfoStorageService,
    private localStorage: UserInfoStorageService,
    private _shareData: DataService,
    private router: Router,
  ) {
  }

  ngOnInit() {
    
  }

  ngAfterViewInit() {
    
  }
}

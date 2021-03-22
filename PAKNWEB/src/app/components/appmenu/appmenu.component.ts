import { Component, OnInit } from '@angular/core';
import { UserInfoStorageService } from '../../commons/user-info-storage.service';
// declare var jquery: any;
declare var $: any;
import { Router } from '@angular/router';

@Component({
  selector: 'app-appmenu',
  templateUrl: './appmenu.component.html',
  styleUrls: ['./appmenu.component.css'],
})
export class AppmenuComponent implements OnInit {
  isSuperAdmin: boolean = false;
  constructor(private userStorage: UserInfoStorageService, private _router: Router) { }

  ngOnInit() { 
  }
}

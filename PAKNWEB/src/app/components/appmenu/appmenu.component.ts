import { Component, OnInit, AfterViewInit } from '@angular/core';
import { UserInfoStorageService } from '../../commons/user-info-storage.service';
// declare var jquery: any;
declare var $: any;
import { Router } from '@angular/router';

@Component({
  selector: 'app-appmenu',
  templateUrl: './appmenu.component.html',
  styleUrls: ['./appmenu.component.css'],
})
export class AppmenuComponent implements OnInit, AfterViewInit {
  isSuperAdmin: boolean = false;
  constructor(private userStorage: UserInfoStorageService, private _router: Router) { }

  ngOnInit() {
    this.isSuperAdmin = this.userStorage.getIsSuperAdmin();
  }
  ngAfterViewInit() {
    for (var nk = window.location,
      o = $("ul#menu a").filter(function () {
        return this.href == nk;
      })
        .addClass("mm-active")
        .parent()
        .addClass("mm-active"); ;) {
      // console.log(o)
      if (!o.is("li")) break;
      o = o.parent()
        .addClass("mm-show")
        .parent()
        .addClass("mm-active");
    }
  }
}

import { Component, OnInit, ViewChild } from '@angular/core';
import { SystemlogListComponent } from './system-log-list.component/system-log-list.component';
import { Router } from '@angular/router';

// declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-systemlog',
  templateUrl: './system-log.component.html',
  styleUrls: ['./system-log.component.css']
})
export class SystemLogComponent implements OnInit {
 
  constructor(private _router: Router) {
  }

  isDelete: boolean = false;
  @ViewChild(SystemlogListComponent, {static: false})
  private systemloglist: SystemlogListComponent;

  _fromDate = new Date();
  _toDate = new Date();
  totalRecords: number = 0;

  ngOnInit() { 
  }

  SearchData() {
    this.systemloglist.GetDataGrid(this._fromDate, this._toDate);
  }

  setDefaultDatefromDepChange() {
    this._fromDate = new Date();
    this._toDate = new Date();
  }

  changeStatusDelete(data: any) {
    if(data){
      if (data.length > 0) {
        this.isDelete = true;
      } else {
        this.isDelete = false;
      }
    }
  }

  ConfirmDelete() {
    $("#modal-confirm-delete").modal('show');
  }

  DeleteSystemlog() {
    this.systemloglist.deleteSystemLog();
  }

  previewInfo() {
    var url = "#" + this._router.url + "/report-view/" + "0" + "/SystemLogReport";
    window.open(url, '_blank');
  }

}

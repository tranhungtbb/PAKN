import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SystemLogService } from '../../../../../services/systemlog.service';
import { DataService } from '../../../../../services/sharedata.service';

declare var $: any;

@Component({
  selector: 'app-systemlog-list',
  templateUrl: './system-log-list.component.html',
  styleUrls: ['./system-log-list.component.css']
})

export class SystemlogListComponent implements OnInit {
  constructor(private service: SystemLogService,
    private _shareData: DataService,
    private _notifi: ToastrService) { }
  _fromDate = new Date;
  _toDate = new Date;
  systemLoglst: any = [];
  userNameSearch: string = '';
  actionSearch: string = '';
  pageSize: number = 20;
  pageIndex: number = 1;
  depId: number = -1;
  status: number = -1;
  request: any = {};
  systemlogdelete: any = [];
  lstPage: any = [];
  pageCount: number = 0;
  total: number = 0;
  fromDate: any = '';
  toDate: any = '';
  idDelete: number = 0;

  @Output() eventCheckDelete = new EventEmitter();

  ngOnInit() {
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  GetDataGrid(fromdate, todate) {
    if (fromdate == '' || fromdate == null) {
      this.fromDate = new Date();
    } else {
      this.fromDate = fromdate;
    }

    if (todate == '' || todate == null) {
      this.toDate = new Date();
    } else {
      this.toDate = todate;
    }

    this.request.Depid = this.depId;
    this.request.Action = this.actionSearch;
    this.request.Name = this.userNameSearch;
    this.request.Status = this.status;
    this.request.FromDate = this.fromDate;
    this.request.ToDate = this.toDate;
    this.request.Pagesize = this.pageSize;
    this.request.PageIndex = this.pageIndex;

    this.service.getGridData(this.request).subscribe(data => {
      if (data.gridData) {
        if (data.gridData.length > 0) {
          this.total = data.gridData[0].tongso;
          for (var i = 0; i < data.gridData.length; i++) {
            data.gridData[i].ngayHanhDong = new Date(Date.parse(data.gridData[i].ngayHanhDong));
          }
        } else
          this.total = 0;
        this.systemLoglst = data.gridData;
      }
    }, error => {
      alert(error);
    });
  }

  setDefaultDatefromDepChange() {
    this._fromDate = new Date();
    this._toDate = new Date();
  }

  Changedepartment(data) {
    $('#deleteSelected').prop('checked', false);
    this.userNameSearch = '';
    this.actionSearch = '';
    this.pageSize = 20;
    this.pageIndex = 1;
    if (data == null) {
      this.depId = this.depId;
    } else {
      this.depId = data.data.ma;
    }

    this.status = -1;
    this.request = {};
    this.systemlogdelete = [];
    this.lstPage = [];
    this.pageCount = 0;
    this.total = 0;
    this.fromDate = '';
    this.toDate = '';

    this.eventCheckDelete.emit(this.systemlogdelete);
    this.GetDataGrid('', '');
  }

  DeleteLog(id: number) {
    this.idDelete = id;
    $("#modal-confirm-delete1").modal('show');
  }

  deleteSingle(id: number) {
    var request = {
      DelteteSystemLog: {
        lstId: id.toString(),
        DepId: this.depId
      }
    }
    this.service.deleteLog(request).subscribe(data => {
      $("#modal-confirm-delete1").modal('hide');
      this._notifi.success("Xóa thành công");
      this.Changedepartment(null);
    }, error => {
      this._notifi.error(error);
    })

  }

  deleteSystemLog() {
    let LstId: Array<any> = [];
    LstId = this.getListMaDelete(this.systemlogdelete);
    var request = {
      DelteteSystemLog: {
        lstId: LstId.join(),
        DepId: this.depId
      }
    }
    this.service.deleteLog(request).subscribe(data => {

      $("#modal-confirm-delete").modal('hide');
      this._notifi.success("Xóa thành công");
      this.Changedepartment(null);
    }, error => {
      this._notifi.error(error);
    })
  }
  public dataStateChange(): void {
    this.GetDataGrid(this.fromDate, this.toDate);
  }

  public onPageChange(event: any): void {
    this.pageSize = event.rows;
    this.pageIndex = (event.first / event.rows) + 1;
    this.GetDataGrid(this.fromDate, this.toDate);
  }

  changeState(event: any) {
    this.status = event.target.value;
    this.GetDataGrid(this.fromDate, this.toDate);
  }

  getListMaDelete(data) {
    const result = data;
    if (!result && !result.length) {
      return null;
    }
    return result.map(obj => obj.ma)
  }

  public onSelectedHeaderChange(event) {
    if (event.checked) {
      this.eventCheckDelete.emit(this.getListMaDelete(this.systemlogdelete));
    } else {
      this.eventCheckDelete.emit([]);
    }
  }

  public onSelectedKeysChange() {
    this.eventCheckDelete.emit(this.getListMaDelete(this.systemlogdelete));
  }

}


import { Component, OnInit } from '@angular/core';
import { DepartmentService } from 'src/app/services/department.service';
import { Router } from '@angular/router';
import { DataService } from '../../../../services/sharedata.service';
@Component({
  selector: 'app-department',
  templateUrl: './department.component.html',
  styleUrls: ['./department.component.css']
})
export class DepartmentComponent implements OnInit {
  lstDepartments: any = [];
  modelDonVi: any = {};
  modelSearch: any = {};
  total: number = 0;
  lstCatalogValue: any = [];
  NameSearch: string;
  deptId: number;
  constructor(private _router: Router, private service: DepartmentService,
    private _shareData: DataService) { }

  ngOnInit() {
    //this.GetDropDown();
    this.GetInfoUnitAndListDepartment();

  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown();
  }

  GetInfoUnitAndListDepartment() {
    this.service.getTreeDepartment()
      .subscribe(data => {
        this.lstDepartments = data.departmentTree;
        this.modelDonVi = data.modelDonVi;
        this.onLoadImage(data.modelDonVi.anhDaiDien);
        this.lstCatalogValue = data.lstUserByDepartment;
        this.total = data.totalRecord;
      }, error => {
        alert(error);
      });
  }

  onLoadImage(anhDaiDien) {
    var request = {
      filePath: anhDaiDien,
      Name: anhDaiDien,
    }
    this.service.LoadImage(request).subscribe(data => {
      if (data != undefined) {
        var blob = new Blob([data], { type: data.type });
        var blob_url = URL.createObjectURL(blob);
        var t = document.getElementById('userImageOfUnit');
        t.setAttribute('src', blob_url);
      }
    });
  }

  onPageChange(event: any): void {
    if (event.rows == undefined) {
      //this.modelSearch.pageSize = 20;
      //this.modelSearch.pageIndex = 1;
    } else {
      this.modelSearch.pageSize = event.rows;
      this.modelSearch.pageIndex = (event.first / event.rows) + 1;
      this.GetData();
    }

  }


  dataStateChange(): void {
    this.modelSearch.pageIndex = 1;
    this.modelSearch.pageSize = 20;
    this.GetData();

  }

  //GetDropDown() {
  //  this.service.GetDropDown().subscribe((result) => {
  //    if (result.status === 1) {
  //      this.lstCatalogValue = result.lstDepartment;
  //    } else if (result.status === 2) {

  //    }
  //  }, error => {
  //    console.error(error);
  //    alert(error);
  //  });
  //}

  GetData() {
    var request = {
      Request: this.modelSearch,
    }
    this.service.DepartmentGetlstUserbyId(request).subscribe((result) => {
      if (result.status === 1) {
        if (result.data.length > 0) {
          this.lstCatalogValue = result.data;
          this.total = result.data[0].totalRecords;
        } else {
          this.lstCatalogValue = [];
          this.total = 0;
        }
      } else if (result.status === 2) {

      }
    }, error => {
      console.error(error);
      alert(error);
    });
  }

  PreAddUserToDepartment(data: any) {

  }

}

import { Component, OnInit } from '@angular/core';

import {RecommandationSyncService} from 'src/app/services/recommandation-sync.service'

declare var $ :any
@Component({
  selector: 'app-cong-thong-tin-dien-tu-tinh',
  templateUrl: './cong-thong-tin-dien-tu-tinh.component.html',
  styleUrls: ['./cong-thong-tin-dien-tu-tinh.component.css']
})
export class CongThongTinDienTuTinhComponent implements OnInit {

  constructor(
    private _RecommandationSyncService:RecommandationSyncService
  ) { }

  ngOnInit() {
  }

  listData:any[] = []

  query={
    questioner:'',
    question:'',
    pageIndex:1,
    pageSize:20
  }

  totalRecords=0

  //event
  onPageChange(event:any){
    this.query.pageSize = event.rows
		this.query.pageIndex = event.first / event.rows + 1
    this.getData();
  }
  dataStateChange (){
    this.getData();
  }

  reSync(){
    //TODO
    this.getData();
  }
  getData(){
    this.query.questioner = this.query.questioner.trim();
    this.query.question = this.query.question.trim();
    let query = {...this.query}
    this._RecommandationSyncService.getCongThongTinDienTuTinhPagedList(query).subscribe(res=>{
      if(res){
        this.listData = res.result.Data;
        this.totalRecords = this.listData[0].rowNumber;
      }
    })
  }

  ///view detail
  modelView:any={}
  getDetail(item:any){
    this.modelView = item
    $('#modalDetail').modal('show')
  }

}

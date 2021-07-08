import { Component, OnInit } from '@angular/core';
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS';
import {RecommandationSyncService} from 'src/app/services/recommandation-sync.service'

declare var $:any
@Component({
  selector: 'app-he-thong-pakn-chinh-phu',
  templateUrl: './he-thong-pakn-chinh-phu.component.html',
  styleUrls: ['./he-thong-pakn-chinh-phu.component.css']
})
export class HeThongPaknChinhPhuComponent implements OnInit {

  constructor(
    private _RecommandationSyncService:RecommandationSyncService
  ) { }

  ngOnInit() {
  }

  listData:any[] = []

  query={
    questioner : '',
    question : '',
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

  getDataSync(){
    //TODO
    this._RecommandationSyncService.asyncPAKNChinhPhu().subscribe(res=>{
      if(res.success == RESPONSE_STATUS.success){
        this.getData();
      }
    })
  }
  getData(){
    this.query.questioner = this.query.questioner.trim();
    this.query.question = this.query.question.trim();

    let query = {...this.query}
    
    this._RecommandationSyncService.getHeThongPANKChinhPhuPagedList(query).subscribe(res=>{
      if(res.success === RESPONSE_STATUS.success){
        if(res.result.Data.length > 0){
          this.listData = res.result.Data
          this.totalRecords = res.result.TotalCount
          this.query.pageIndex = res.result.PageIndex
          this.query.pageSize = res.result.PageSize
        }
        else{
          this.listData = []
          this.totalRecords = 0
          this.query.pageIndex = 1
          this.query.pageSize = 20
        }
        
      }
    }),(err)=>{
      console.log(err)
    }
  }

}

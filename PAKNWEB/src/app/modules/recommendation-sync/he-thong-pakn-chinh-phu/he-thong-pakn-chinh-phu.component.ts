import { Component, OnInit } from '@angular/core';
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
    title:'',
    status:'',
    createdby:'',
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
    this.query.title = this.query.title.trim();
    this.query.createdby = this.query.createdby.trim();
    this.query.status = this.query.status.trim();

    let query = {...this.query}
    
    this._RecommandationSyncService.getHeThongPANKChinhPhuPagedList(query).subscribe(res=>{
      if(res){
        this.listData = res.result.Data.map(c=>{
          if(c.questioner)
            c.questioner = c.questioner.substr(0,c.questioner.length-1)
          return c;
        })

        if(this.listData[0])
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

import { Component, OnInit } from '@angular/core';
import {RecommandationSyncService} from 'src/app/services/recommandation-sync.service'

@Component({
  selector: 'app-cong-thong-tin-dv-hcc',
  templateUrl: './cong-thong-tin-dv-hcc.component.html',
  styleUrls: ['./cong-thong-tin-dv-hcc.component.css']
})
export class CongThongTinDvHccComponent implements OnInit {

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

  modelView:any

  //event
  onPageChange(event:any){
    this.query.pageSize = event.rows
		this.query.pageIndex = event.first / event.rows + 1
    this.getData();
  }
  dataStateChange (){
    this.getData();
  }
  getDetail(item:any){}

  reSync(){
    //TODO
    this.getData();
  }
  getData(){
    this.query.questioner = this.query.questioner.trim();
    this.query.question = this.query.question.trim();
    let query = {...this.query}
    this._RecommandationSyncService.getDichVuHCCPagedList(query).subscribe(res=>{
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

}

import { Component, OnInit } from '@angular/core';
import {RecommandationSyncService} from 'src/app/services/recommandation-sync.service'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { ToastrService } from 'ngx-toastr'

declare var $:any
@Component({
  selector: 'app-cong-thong-tin-dv-hcc',
  templateUrl: './cong-thong-tin-dv-hcc.component.html',
  styleUrls: ['./cong-thong-tin-dv-hcc.component.css']
})
export class CongThongTinDvHccComponent implements OnInit {

  constructor(
    private _RecommandationSyncService:RecommandationSyncService,
    private _toastr : ToastrService
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

  asyncData(){
    this._RecommandationSyncService.asyncDichVuHCC({}).subscribe(res=>{
      console.log(res)
      if(res.success == RESPONSE_STATUS.success){
        this._toastr.success('Đồng bộ thành công')
        this.getData();
      }
      else{
        this._toastr.error('Đồng bộ lỗi')
      }
    })
    , (err)=>{
      console.log(err)
    }
  }
  getData(){
    this.query.questioner = this.query.questioner.trim();
    this.query.question = this.query.question.trim();
    let query = {...this.query}
    this._RecommandationSyncService.getDichVuHCCPagedList(query).subscribe(res=>{
      if(res){
        this.listData = res.result.Data

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

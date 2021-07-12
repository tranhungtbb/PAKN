import { Component, OnInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import {RecommandationSyncService} from 'src/app/services/recommandation-sync.service'

declare var $ :any
@Component({
  selector: 'app-cong-thong-tin-dien-tu-tinh',
  templateUrl: './cong-thong-tin-dien-tu-tinh.component.html',
  styleUrls: ['./cong-thong-tin-dien-tu-tinh.component.css']
})
export class CongThongTinDienTuTinhComponent implements OnInit {

  constructor(
    private _RecommandationSyncService:RecommandationSyncService,
    private _toastr : ToastrService
  ) { }

  ngOnInit() {
  }

  listData:any[] = []
  files : any [] = []

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
    this._RecommandationSyncService.asyncCongThongTinDienTu({}).subscribe(res=>{
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
    let request = {
			Id: item.id,
		}
		this._RecommandationSyncService.getCongThongTinDienTuTinhGetById(request).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if(response.result.Data.length > 0){
					this.modelView = response.result.Data[0]
          this.files = response.result.FileAttach

          $('#modalDetail').modal('show')
				}else{
					this.modelView = null
				}
			} else {
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
			}
  }

}

import { Component, OnInit, ViewChild } from '@angular/core';
import {Router} from '@angular/router'
import {RecommandationSyncService} from 'src/app/services/recommandation-sync.service'
import {TableViewComponent} from './table-view/table-view.component'

@Component({
  selector: 'app-recommendations-sync',
  templateUrl: './recommendations-sync.component.html',
  styleUrls: ['./recommendations-sync.component.css']
})
export class RecommendationsSyncComponent implements OnInit {

  constructor(
    private router: Router,
    private _RecommandationSyncService:RecommandationSyncService
  ) { }

  @ViewChild(TableViewComponent,{static:true}) childTableView :TableViewComponent
  
    title='Danh sách PAKN Cổng thông tin điện tử tỉnh Khánh Hòa'

  ngOnInit() {
    //this.childTableView.parent = this;
    
    if(this.router.url.includes('/cong-ttdt-tinh-khanh-hoa'))
      {
        this.query.src = 1
        this.title = 'Danh sách PAKN Cổng thông tin điện tử tỉnh Khánh Hòa'
      }
    else if(this.router.url.includes('/cong-dv-hcc-tinh-khoanh-hoa'))
      {
        this.query.src = 2
        this.title = 'Danh sách PAKN Cổng thông tin hành chính công tỉnh Khánh Hòa'
      }
    else if(this.router.url.includes('/he-thong-cu-tri-khanh-hoa'))
      {
        this.query.src = 3
        this.title ='Hệ thống quản lý kiến nghị cử tri tỉnh Khánh Hòa'
      }
    else if(this.router.url.includes('/he-thong-pakn-quoc-gia'))
      {
        this.query.src = 4
        this.title = 'Hệ thống tiếp nhận, trả lời PAKN của người dân của chính phủ'
      }
    
    this.getData();
  }

  listData :any[]=[]

  query = {
    keyword :'',
    src:0,
    pageIndex:1,
    pageSize:20
  }
  total = 0
  pagination = []


  getData(){
    this.query.keyword = this.query.keyword.trim();

    let query = {...this.query}
    this._RecommandationSyncService.getAllPagedList(query).subscribe(res=>{
      console.log(res)
      if(res){
        this.listData = res.result.Data;
        this.total = this.listData[0].rowNumber;

        this.padi()
      }
    })
  }

  onPageChange(event:any){
    // this.query.pageSize = event.rows
		let pageIndex = event.first / event.rows + 1
    this.changePagination(pageIndex);
  }
  changeKeySearch(ev:any){
    this.getData();
  }

  padi() {
		this.pagination = []
		for (let i = 0; i < Math.ceil(this.total / this.query.pageSize); i++) {
			this.pagination.push({ index: i + 1 })
		}
	}
  changePagination(index: any) {
		if (this.query.pageIndex > index) {
			if (index > 0) {
				this.query.pageIndex = index
				this.getData()
			}
			return
		} else if (this.query.pageIndex < index) {
			if (this.pagination.length >= index) {
				this.query.pageIndex = index
				this.getData()
			}
			return
		}
		return
	}
}

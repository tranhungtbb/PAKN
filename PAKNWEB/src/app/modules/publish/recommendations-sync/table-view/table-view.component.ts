import { Component, OnInit, Input, Output } from '@angular/core';
import {RecommendationsSyncComponent} from '../recommendations-sync.component'

@Component({
  selector: 'app-table-view',
  templateUrl: './table-view.component.html',
  styleUrls: ['./table-view.component.css']
})
export class TableViewComponent implements OnInit {

  constructor() { }
  listData:any[] = []

  parent: RecommendationsSyncComponent
  query:any
  totalRecords=0

  @Input('data') data:any[]

  ngOnInit() {
    
  }

  onPageChange(event:any){
    // this.query.pageSize = event.rows
		let pageIndex = event.first / event.rows + 1
    this.parent.changePagination(pageIndex);
  }

}

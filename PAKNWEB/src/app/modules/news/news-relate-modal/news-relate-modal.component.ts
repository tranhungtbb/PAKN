import { Component, OnInit } from '@angular/core';

import { NewsService } from 'src/app/services/news.service'
import {NewsCreateOrUpdateComponent}  from '../news-create-or-update/news-create-or-update.component'
declare var $:any
@Component({
  selector: 'app-news-relate-modal',
  templateUrl: './news-relate-modal.component.html',
  styleUrls: ['./news-relate-modal.component.css']
})
export class NewsRelateModalComponent implements OnInit {

	constructor(
		private newsService:NewsService,
		private newsCreateOrUpdateComponent:NewsCreateOrUpdateComponent
	) { }

  	listDataPaged:any[]
  	newsSelected: any[] = []
  	query: any = {
		pageSize: 20,
		pageIndex: 1,
		title: '',
		newType: '',
	}
  	totalCount:number = 0;
	pageCount:number = 0
	ngOnInit() {
		this.getListPaged()
	}
	onChangeChecked(id:number,checked:boolean){
		if(checked){
			this.newsSelected.push(id);
		}else{
			let index = this.newsSelected.indexOf(id);
			this.newsSelected.splice(index,1);
		}
	}
	onSave(){
		this.newsCreateOrUpdateComponent.onModalNewsRelate_Close();
		$('#modal-news-relate').modal('hide');
	}
	getListPaged() {
		this.newsService.getAllPagedList(this.query).subscribe(res=>{
			if (res.success != 'OK') return
			this.listDataPaged = res.result.NENewsGetAllOnPage
			if (this.totalCount <= 0) this.totalCount = res.result.TotalCount
			this.totalCount = Math.ceil(this.totalCount / this.query.pageSize)
		});
	}
	changePage(page:any){
		this.query.pageIndex += page
		if (this.query.pageIndex < 1) {
			this.query.pageIndex = 1
			return
		}
		if (this.query.pageIndex > this.pageCount) {
			this.query.pageIndex = this.pageCount
			return
		}
		this.getListPaged()
	}
	filterChange(){
		this.getListPaged()
	}

	openModal(newsRelate:any[]){
		if(newsRelate)
			this.newsSelected = newsRelate
		$('#modal-news-relate').modal('show');
	}
}

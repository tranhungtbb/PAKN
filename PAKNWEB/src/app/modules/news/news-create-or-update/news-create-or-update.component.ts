import { Component, OnInit,ViewChild } from '@angular/core'
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { Router, ActivatedRoute, ParamMap } from '@angular/router'
import {MatDialog} from '@angular/material/dialog'

import { NewsService } from 'src/app/services/news.service'

import { NewsRelateModalComponent} from '../news-relate-modal/news-relate-modal.component'

import { COMMONS } from 'src/app/commons/commons'
import { NewsModel } from 'src/app/models/NewsObject'

@Component({
	selector: 'app-news-create-or-update',
	templateUrl: './news-create-or-update.component.html',
	styleUrls: ['./news-create-or-update.component.css'],
})
export class NewsCreateOrUpdateComponent implements OnInit {
	@ViewChild(NewsRelateModalComponent,{static:false}) newsRelateChild: NewsRelateModalComponent
	constructor(
		private toast: ToastrService, 
		private formBuilder: FormBuilder, 
		private newsService: NewsService, 
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private dialog:MatDialog) {}
	public Editor = ClassicEditor
	model: NewsModel = new NewsModel()
	newsFrom: FormGroup
	listNewsCategories: Array<any>
	postTypes: any[] = [
		{ text: 'thường', value: true, checked: true },
		{ text: 'nổi bật', value: false, checked: false },
	]

	categoriesSelected: any[]
	ngOnInit() {
		this.newsFrom = this.formBuilder.group({
			title: ['', [Validators.required]],
			summary: [this.model.contents, [Validators.required]],
			contents: [''],
			newsType: [''],
			postType: [''],
			pushNotify: [''],

		})
		this.activatedRoute.params.subscribe(params => {
			if (params['id']) {
				this.newsService.getById({ id: params['id']}).subscribe(res => {
					if (res.success != 'OK') {
						return
					}
					this.model=res.result.NENewsGetByID[0]
					this.Editor
				});
			}
	    });
	}

	get f() {
		return this.newsFrom.controls
	}

	submitted = false
	onSave(event) {
		this.submitted = true
		if (this.newsFrom.invalid) {
			return
		}

		if (this.model.id && this.model.id > 0) {
			this.newsService.update(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.UPDATE_FAILED)
					return
				}
				this.toast.success(COMMONS.UPDATE_SUCCESS)
				this.router.navigate(['/quan-tri/tin-tuc'])
			})
		} else {
			this.newsService.create(this.model).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.ADD_FAILED)
					return
				}
				this.toast.success(COMMONS.ADD_SUCCESS)
				this.router.navigate(['/quan-tri/tin-tuc'])
			})
		}
	}

	onPublished() {
		this.router.navigate(['/quan-tri/tin-tuc'])
	}

	onModalNewsRelate(){
		//this.dialog.open(NewsRelateModalComponent,{width:'500px'});
		this.newsRelateChild.openModal(this.model.newsRelates)
	}
	onModalNewsRelate_Close(){
		this.model.newsRelates = this.newsRelateChild.newsSelected;
		console.log(this.model)
	}

	public onChangeEditor({ editor }: any) {
		let data = editor.getData()
		this.model.contents = data
	}
	public onReady(editor) {
		editor.ui.getEditableElement().parentElement.insertBefore(editor.ui.view.toolbar.element, editor.ui.getEditableElement())
	}
}

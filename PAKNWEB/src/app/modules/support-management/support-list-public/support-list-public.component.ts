import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import * as ClassicEditor from '../../../../assets/dist/ckeditor5'
import { RESPONSE_STATUS, CATEGORY_SUPPORT, TYPE_SUPPORT } from 'src/app/constants/CONSTANTS'
import { SupportService } from 'src/app/services/support.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { COMMONS } from 'src/app/commons/commons'
import { DataService } from 'src/app/services/sharedata.service'
import { AppSettings } from 'src/app/constants/app-setting'
import { Api } from 'src/app/constants/api'
import { HttpClient } from '@angular/common/http'
import { UploadDocumentAdapter } from 'src/app/services/UploadDocumentAdapter'

declare var $: any

@Component({
	selector: 'app-support-public',
	templateUrl: './support-list-public.component.html',
	styleUrls: ['./support-list-public.component.css'],
})
export class SupportListPublicComponent implements OnInit, AfterViewInit {
	constructor(
		private toastr: ToastrService,
		private supportService: SupportService,
		private _shareData: DataService,
		private _fb: FormBuilder,
		private http: HttpClient,
	) {
		this.lstSupport = []
		this.ltsUpdateMenu = []
		this.ltsDeleteMenu = []
	}
	@ViewChild('file', { static: false }) public file: ElementRef
	lstSupport: any = []
	objSupport: any
	treeSp: any = []
	submitted: boolean = false
	ltsUpdateMenu: any[]
	ltsDeleteMenu: any[]
	form: FormGroup
	model: any = {
		id: 0,
		title: null,
		category: CATEGORY_SUPPORT.DOCUMENT,
		parentId: 0,
		level: 1,
		type: 2,
		index: null

	}
	action: string = 'Thêm mới'

	public Editor = ClassicEditor
	public ckConfig = {
		simpleUpload: {
			uploadUrl: AppSettings.API_ADDRESS + Api.UploadImageDocument,
		}
	}

	config = {
		simpleUpload: {
			uploadUrl: AppSettings.API_ADDRESS + Api.UploadImageDocument,
		},
		toolbar: {
			items: [
				'heading', '|',
				'fontfamily', 'fontsize',
				'alignment',
				'fontColor', 'fontBackgroundColor', '|',
				'bold', 'italic', 'strikethrough', 'underline', 'subscript', 'superscript', '|',
				'link', '|',
				'outdent', 'indent', '|',
				'bulletedList', '-', 'numberedList', 'todoList', '|',
				'code', 'codeBlock', '|',
				'insertTable', '|',
				'imageUpload', 'blockQuote', '|',
				'todoList',
				'undo', 'redo',
			],
			shouldNotGroupWhenFull: true,

		},
		image: {
			// Configure the available styles.
			styles: [
				'alignLeft', 'alignCenter', 'alignRight'
			],

			// Configure the available image resize options.
			resizeOptions: [
				{
					name: 'resizeImage:original',
					label: 'Original',
					value: null
				},
				{
					name: 'resizeImage:50',
					label: '25%',
					value: '25'
				},
				{
					name: 'resizeImage:50',
					label: '50%',
					value: '50'
				},
				{
					name: 'resizeImage:75',
					label: '75%',
					value: '75'
				}
			],

			// You need to configure the image toolbar, too, so it shows the new style
			// buttons as well as the resize buttons.
			toolbar: [
				'imageStyle:alignLeft', 'imageStyle:alignCenter', 'imageStyle:alignRight',
				'|',
				'ImageResize',
				'|',
				'imageTextAlternative'
			]
		},
		// simpleUpload: {
		//    The URL that the images are uploaded to.
		// uploadUrl: 'http://localhost:52536/api/Image/ImageUpload',

		//   Enable the XMLHttpRequest.withCredentials property.

		//},

		language: 'en'
	};

	ngOnInit() {
		this.getAll()
		this.form = this._fb.group({
			title: [this.model.title, [Validators.required]],
			content: [this.model.content, [Validators.required]],
			index: [this.model.index]
		})
	}
	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	clearModel() {
		this.model = {
			id: 0,
			title: '',
			category: CATEGORY_SUPPORT.DOCUMENT,
			parentId: 0,
			level: 1,
			type: TYPE_SUPPORT.PUBLIC,
			index: null
		}
	}

	getAll() {
		this.supportService.GetListByType({ Type: TYPE_SUPPORT.PUBLIC }).subscribe(
			(res) => {
				if (res.success != RESPONSE_STATUS.success) return
				this.lstSupport = res.result
			},
			(err) => {
				console.log(err)
			}
		)
	}

	treeViewActive(model: any) {
		this.model = { ...model }
		this.submitted = false
		this.action = 'Cập nhật'
		this.rebuidForm()
		return
	}
	get f() {
		return this.form.controls
	}

	rebuidForm() {
		this.form.reset({
			title: this.model.title,
			content: this.model.content,
			index: this.model.index
		})
	}

	preCreate() {
		this.submitted = false
		this.clearModel()
		this.action = 'Thêm mới'
		this.rebuidForm()

	}

	onSave() {
		this.submitted = true
		if (!this.form.valid) {
			return
		}
		let obj = {
			model: this.model
		}
		if (this.model.id == undefined || this.model.id == 0) {
			this.supportService.Insert(obj).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.getAll()
					this.toastr.success(COMMONS.ADD_SUCCESS)
				} else {
					let result = isNaN(res.result) == true ? 0 : res.result
					if (result == -1) {
						this.toastr.error('Tên tài liệu đã tồn tại')
						return
					} else {
						this.toastr.error(res.message)
						return
					}
				}
			}),
				(err) => {
					console.log(err)
				}
		} else {
			this.supportService.Update(obj).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.getAll()
					this.toastr.success(COMMONS.UPDATE_SUCCESS)
				} else {
					let result = isNaN(res.result) == true ? 0 : res.result
					if (result == -1) {
						this.toastr.error('Tên tài liệu đã tồn tại')
						return
					} else {
						this.toastr.error(res.message)
						return
					}
				}
			}),
				(err) => {
					console.log(err)
				}
		}
	}

	preDelete(id: any) {
		this.model = this.lstSupport.find((x) => x.id == id)
		$('#modal-confirm').modal('show')
	}
	onDelete() {
		$('#modal-confirm').modal('hide')
		this.supportService.Delete({ Id: this.model.id }, this.model.title).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.toastr.success(COMMONS.DELETE_SUCCESS)
				this.getAll()
				this.preCreate()
			} else {
				this.toastr.success(COMMONS.DELETE_FAILED)
			}
		}),
			(err) => {
				console.log(err)
			}
	}

	public onReady(editor) {
		if (editor.model.schema.isRegistered('image')) {
			editor.model.schema.extend('image', { allowAttributes: 'blockIndent' });
		}
		editor.ui.getEditableElement().parentElement.insertBefore(editor.ui.view.toolbar.element, editor.ui.getEditableElement())
		editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
			return new UploadDocumentAdapter(loader, this.http, AppSettings.API_ADDRESS + Api.UploadImageDocument)
		}
	}
}

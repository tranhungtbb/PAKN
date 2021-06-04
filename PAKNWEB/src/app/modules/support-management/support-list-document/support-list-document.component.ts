import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { saveAs as importedSaveAs } from 'file-saver'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { AppSettings } from 'src/app/constants/app-setting'
import { CONSTANTS, FILETYPE, RESPONSE_STATUS, MESSAGE_COMMON, RECOMMENDATION_STATUS, CATEGORY_SUPPORT } from 'src/app/constants/CONSTANTS'
import { SupportService } from 'src/app/services/support.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { COMMONS } from 'src/app/commons/commons'
declare var $: any

@Component({
	selector: 'app-support-document',
	templateUrl: './support-list-document.component.html',
	styleUrls: ['./support-list-document.component.css'],
})
export class SupportListDocumentComponent implements OnInit {
	constructor(
		private localStorage: UserInfoStorageService,
		private fileService: UploadFileService,
		private toastr: ToastrService,
		private supportService: SupportService,
		private _fb: FormBuilder
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
		title: '',
		category: CATEGORY_SUPPORT.DOCUMENT,
		parentId: 0,
		level: 1,
		type: 1,
	}
	fileAccept = CONSTANTS.FILEACCEPT
	files: any[] = []
	ngOnInit() {
		this.getAllUnitShortInfo()
		this.form = this._fb.group({
			title: [this.model.title, [Validators.required]],
		})
	}

	getAllUnitShortInfo(activeTreeNode: any = null) {
		this.supportService.GetList({ Category: CATEGORY_SUPPORT.DOCUMENT }).subscribe(
			(res) => {
				if (res.success != RESPONSE_STATUS.success) return
				this.lstSupport = res.result
				let listSP = res.result.map((e) => {
					if (e.level == 1) {
						this.ltsUpdateMenu.push(e.id)
					}
					if (e.type == 1 && e.level == 1) {
						this.ltsDeleteMenu.push(e.id)
					}
					let item = {
						id: e.id,
						name: e.title,
						parentId: e.parentId == null ? 0 : e.parentId,
						level: e.level,
						children: [],
						label: e.title,
					}
					return item
				})

				this.model.parentId = this.lstSupport[0].id

				this.treeSp = this.unflatten(listSP)

				//active first
				let active = 0
				if (this.ltsDeleteMenu.length > 0) {
					active = this.ltsDeleteMenu[0]
				} else {
					let filter = this.lstSupport.filter((x) => {
						if (x.type == 2 && x.level == 1) {
							return x
						}
						return
					})
					active = filter[0].id
				}
				this.treeViewActive(active)
			},
			(err) => {
				console.log(err)
			}
		)
	}

	treeViewActive(id: any) {
		let s = this.lstSupport.find((x) => x.id == id)
		if (s.level == 1) {
			this.objSupport = { ...s }
			$('#show-document').attr('src', this.objSupport.filePath) // 'http://localhost:51046/' +
		}
		return
	}
	get f() {
		return this.form.controls
	}

	rebuidForm() {
		this.form.reset({
			title: this.model.title,
		})
	}

	preCreate() {
		this.submitted = false
		this.model.title = ''
		this.model.id = 0
		this.files = []
		this.rebuidForm()
		$('#modal').modal('show')
	}

	preUpdate(id: any) {
		this.submitted = false
		this.files = []
		this.model = this.lstSupport.find((x) => x.id == id)
		this.files.push({
			fileType: this.model.fileType,
			name: this.model.fileName,
		})
		this.rebuidForm()
		$('#modal').modal('show')
	}
	onSave() {
		if (this.files.length == 0) {
			this.toastr.error('Vui lòng chọn file đính kèm')
			return
		}
		if (this.form.invalid) {
			return
		}
		this.submitted = true
		let obj = {
			model: this.model,
			files: this.files,
		}
		if (this.model.id == undefined || this.model.id == 0) {
			this.supportService.Insert(obj).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					$('#modal').modal('hide')
					this.files = []
					this.getAllUnitShortInfo()
				} else {
					let result = isNaN(res.result) == true ? 0 : res.result
					if (result == -1) {
						this.toastr.error('Tên chuyên mục đã tồn tại')
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
					$('#modal').modal('hide')
					this.files = []
					this.getAllUnitShortInfo()
				} else {
					let result = isNaN(res.result) == true ? 0 : res.result
					if (result == -1) {
						this.toastr.error('Tên chuyên mục đã tồn tại')
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
		this.files = []
		this.model = this.lstSupport.find((x) => x.id == id)
		$('#modal-confirm').modal('show')
	}
	onDelete() {
		$('#modal-confirm').modal('hide')
		this.supportService.Delete({ Id: this.model.id }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.toastr.success(COMMONS.DELETE_SUCCESS)
				this.getAllUnitShortInfo()
			} else {
				this.toastr.success(COMMONS.DELETE_FAILED)
			}
		}),
			(err) => {
				console.log(err)
			}
	}

	onUpload(event) {
		if (event.target.files.length == 0) {
			return
		}
		if (event.target.files[0].type != 'application/pdf') {
			this.toastr.error('Định dạng không được hỗ trợ')
			return
		}

		if (this.files.length == 1) {
			this.toastr.error('Chỉ được chọn 1 file')
			return
		}
		for (let item of event.target.files) {
			FILETYPE.forEach((fileType) => {
				if (item.type == fileType.text) {
					let max = this.files.reduce((a, b) => {
						return a.id > b.id ? a.id : b.id
					}, 0)
					item.id = max + 1
					item.fileType = fileType.value
					this.files.push(item)
				}
			})
			if (!item.fileType) {
				this.toastr.error('Định dạng không được hỗ trợ')
			}
		}
		this.file.nativeElement.value = ''
	}

	onRemoveFile(item: any) {
		for (let index = 0; index < this.files.length; index++) {
			if (this.files[index].id == item.id) {
				this.files.splice(index, 1)
				break
			}
		}
	}

	private unflatten(arr): any[] {
		var tree = [],
			mappedArr = {},
			arrElem,
			mappedElem

		// First map the nodes of the array to an object -> create a hash table.
		for (var i = 0, len = arr.length; i < len; i++) {
			arrElem = arr[i]
			mappedArr[arrElem.id] = arrElem
			mappedArr[arrElem.id]['children'] = []
		}

		for (var id in mappedArr) {
			if (mappedArr.hasOwnProperty(id)) {
				mappedElem = mappedArr[id]
				// If the element is not at the root level, add it to its parent array of children.
				if (mappedElem.parentId) {
					if (!mappedArr[mappedElem['parentId']]) continue
					mappedArr[mappedElem['parentId']]['children'].push(mappedElem)
				}
				// If the element is at the root level, add it to first level elements array.
				else {
					mappedElem['expanded'] = true
					tree.push(mappedElem)
				}
			}
		}
		return tree
	}
}

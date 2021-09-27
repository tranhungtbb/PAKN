import { Component, OnInit } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { TreeviewItem, TreeviewConfig } from 'ngx-treeview'
import { TreeviewI18n } from 'ngx-treeview'

import { ToastrService } from 'ngx-toastr'
import { RESPONSE_STATUS, FILETYPE } from 'src/app/constants/CONSTANTS'
import { STATUS_HIS_SMS } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { EmailManagementService } from 'src/app/services/email-management.service'
import { SMSManagementService } from 'src/app/services/sms-management'
import { SMSTreeviewI18n } from 'src/app/shared/sms-treeview-i18n'
import { EmailAttachmentObject, EmailBusinessIndividualObject, EmailObject } from 'src/app/models/emailManagementObject'

import { UploadFileService } from 'src/app/services/uploadfiles.service'

declare var $: any

@Component({
	selector: 'app-email-create',
	templateUrl: './email-create.component.html',
	styleUrls: ['./email-create.component.css'],
	providers: [{ provide: TreeviewI18n, useClass: SMSTreeviewI18n }],
})
export class EmailCreateComponent implements OnInit {
	constructor(
		private _toastr: ToastrService,
		private formBuilder: FormBuilder,
		private router: Router,
		private emailService: EmailManagementService,
		private activatedRoute: ActivatedRoute,
		private smsService: SMSManagementService,
		private fileService: UploadFileService
	) {}

	fileAccept = ''
	redirectList: any
	statusCurent: any
	///data model
	model: EmailObject = new EmailObject()
	form: FormGroup
	listAttachment: any[] = []
	title = 'Soạn thảo email'

	listAttchamentDel: any[] = []
	listFileNew: any[] = []
	listBusinessIndividual = new Array<EmailBusinessIndividualObject>()

	///data display
	listDiadanh: any[] = []
	listDiadanhTree: TreeviewItem[] = []
	config = TreeviewConfig.create({
		hasAllCheckBox: true,
		hasFilter: true,
		hasCollapseExpand: true,
		decoupleChildFromParent: false,
		maxHeight: 400,
	})

	selectedItems: any[] = []
	listIndividualAndBusinessGetByAdmintrativeId: any[] = []

	ngOnInit() {
		this.form = this.formBuilder.group({
			title: [this.model.title, [Validators.required]],
			content: [this.model.content, [Validators.required]],
			signature: [this.model.signature, [Validators.required]],
		})
		this.getData()
	}
	ltsUnitFirst: any = []

	getData() {
		this.activatedRoute.params.subscribe((params) => {
			let id = params['id']
			if (id) {
				this.title = 'Cập nhập Email'
				this.emailService.getById(id).subscribe((res) => {
					if (res && res.success == 'OK') {
						this.model = res.result.Data
						this.listAttachment = res.result.ListAttachment
						this.listBusinessIndividual = res.result.ListBusinessIndividual
						this.listBusinessIndividual.forEach((x) => {
							this.ltsUnitFirst.push(x['admintrativeUnitId'])
						})
						this.ltsUnitFirst = this.ltsUnitFirst.filter((item, index) => this.ltsUnitFirst.indexOf(item) === index)
						console.log(this.ltsUnitFirst)
						this.getAdministrativeUnits()
					}
				})
			} else {
				this.getAdministrativeUnits()
			}
		})
	}
	getAdministrativeUnits() {
		this.smsService.GetListAdmintrative({ id: 37 }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.listDiadanh = res.result.CAAdministrativeUnitsGetDropDown
				if (this.listDiadanh.length > 0) {
					var itemFirst = new TreeViewDrop()
					itemFirst.text = this.listDiadanh[0].name
					itemFirst.value = this.listDiadanh[0].id
					itemFirst.children = []
					itemFirst.checked = false
					for (const iterator of this.listDiadanh.filter((x) => x.parentId == itemFirst.value)) {
						var item = new TreeViewDrop()
						item.value = iterator.id
						item.text = iterator.name
						item.children = []
						item.checked = this.ltsUnitFirst.includes(iterator.id) == true ? true : false
						item.collapsed = true
						for (const iterator1 of this.listDiadanh.filter((x) => x.parentId == iterator.id)) {
							let item2 = new TreeViewDrop()
							item2.value = iterator1.id
							item2.text = iterator1.name
							item2.checked = this.ltsUnitFirst.includes(iterator1.id) == true ? true : false
							item2.collapsed = false
							if (item2.checked) {
								item.collapsed = false
							}
							item.children.push(item2)
						}
						itemFirst.children.push(item)
					}
					this.listDiadanhTree = [new TreeviewItem({ ...itemFirst })]
				}
			}
		})
	}

	get f() {
		return this.form.controls
	}
	submitted = false
	onSave(sendNow = false, cb: any = null) {
		this.submitted = true
		this.model.status = sendNow == false ? 1 : 2
		if (this.form.invalid) {
			return
		}

		if (this.listBusinessIndividual.length == 0 && this.model.status == 2) {
			this._toastr.error('Vui lòng chọn cá nhân, doanh nghiệp được gửi SMS')
			return
		}

		let model = {
			Data: this.model,
			ListAttachmentDel: this.listAttchamentDel,
			ListBusinessIndividual: this.listBusinessIndividual,
		}

		this.emailService.createOrUpdate(model, this.listFileNew).subscribe((res) => {
			if (res && res.success) {
				if (cb) cb(res.result.Data)
				this.router.navigate(['/quan-tri/email-sms/email'])
			}
		})
	}

	///
	pressSendButton() {
		this.onSave(false, (item) => {
			this.emailService.SendEmail(item.id).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
				} else {
				}
			})
			// console.log(item)
		})
		$('#modalConfirmChangeStatus').modal('hide')
	}

	userId: any[] = []
	onCreateUser() {
		if (this.ltsAdministrativeUnitId == undefined || this.ltsAdministrativeUnitId == '') {
			this._toastr.error('Vui lòng chọn đơn vị')
			return
		}
		if (this.userId != undefined && this.userId.length > 0 && this.userId != null) {
			let indexSuccess = 0
			let indexError = 0
			for (const iterator of this.userId) {
				let check = this.listBusinessIndividual.find((x) => x.objectId == iterator.id && x.category == iterator.category)
				if (check != null || check != undefined) {
					indexError++
					continue
				}
				let item: any = new EmailBusinessIndividualObject()
				item.objectId = iterator.id
				item.category = iterator.category
				item.objectName = iterator.name
				item.unitName = iterator.administrativeUnitName
				item.admintrativeUnitId = iterator.administrativeUnitId
				this.listBusinessIndividual.push(item)
				indexSuccess++
			}
			if (indexSuccess > 0) {
				this._toastr.success('Thêm mới thành công ' + indexSuccess + ' người dùng!')
			}
			if (indexError > 0) {
				this._toastr.error('Thêm mới thất bại ' + indexError + ' người dùng!')
			}
			// this.userId = []
		} else {
			this._toastr.error('Vui lòng chọn cá nhân, doanh nghiệp')
			return
		}
		console.log(this.listBusinessIndividual)
	}
	onRemoveUser(item: any) {
		this.listBusinessIndividual.splice(this.listBusinessIndividual.indexOf(item), 1)
		return
	}

	onUpload(event) {
		if (event.target.files.length == 0) {
			return
		}
		const check = this.fileService.checkFileWasExitsted(event, this.listFileNew)
		if (check === 1) {
			for (let item of event.target.files) {
				FILETYPE.forEach((fileType) => {
					if (item.type == fileType.text) {
						item.fileType = fileType.value
						this.listFileNew.push(item)
					}
				})
				if (!item.fileType) {
					this._toastr.error('Định dạng không được hỗ trợ')
				}
			}
		} else if (check === 2) {
			this._toastr.error('Không được phép đẩy trùng tên file lên hệ thống')
		} else {
			this._toastr.error('File tải lên vượt quá dung lượng cho phép 20MB')
		}
		event.target.value = ''
	}
	onRemoveFile(args) {
		const index = this.listFileNew.indexOf(args)
		const file = this.listFileNew[index]

		if (!file) {
			let index = this.listAttachment.indexOf(args)
			let file = this.listAttachment[index]

			this.listAttchamentDel.push(file)
			this.listAttachment.splice(index, 1)
			return
		}
		this.listFileNew.splice(index, 1)
	}

	objectType = 3
	onChange(event: any, category: number) {
		if (event.target.checked) {
			this.objectType += category
		} else {
			this.objectType -= category
		}
		console.log(this.objectType)
		this.onLoadListIndividualAndBusiness()
	}
	onSelectedChange(values: any[]) {
		this.ltsAdministrativeUnitId = ''
		if (values.length > 0) {
			this.ltsAdministrativeUnitId = values.reduce((x, y) => {
				return (x += ',' + y)
			}, '')
		}
		this.onLoadListIndividualAndBusiness()
	}

	ltsAdministrativeUnitId: string
	onLoadListIndividualAndBusiness() {
		let obj = {
			LtsAdministrativeId: this.ltsAdministrativeUnitId,
			type: this.objectType,
		}
		this.smsService.GetListIndividualAndBusinessByAdmintrativeUnitId(obj).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				if (res.result.BIIndividualOrBusinessGetDropListByProviceId.length > 0) {
					this.listIndividualAndBusinessGetByAdmintrativeId = res.result.BIIndividualOrBusinessGetDropListByProviceId
				} else {
					this.listIndividualAndBusinessGetByAdmintrativeId = []
				}
			} else {
				this.listIndividualAndBusinessGetByAdmintrativeId = []
			}
		})
	}

	//preview file
	previewFilePath = ''
	previewType = 0
	previewFile(item: any) {
		if (![3, 4].includes(item.fileType)) {
			//TODO
			//create download link
			let a = document.createElement('a')
			a.href = item.fileAttach
			a.download = item.name
			$('body').append(a)
			a.click()
			setTimeout(() => {
				$(a).remove()
			}, 200)
			return
		}
		this.previewFilePath = `${item.fileAttach}`
		this.previewType = item.fileType
		$('#previewFile').modal('show')
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
					tree.push(mappedElem)
				}
			}
		}
		tree = tree.sort((x, y) => {
			return x.index - y.index
		})
		return tree
	}
}
class TreeViewDrop {
	text: string
	value: number
	children: any[]
	checked: boolean
	collapsed: boolean
}

import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { TreeviewItem, TreeviewConfig } from 'ngx-treeview'
import { TreeviewI18n } from 'ngx-treeview'

import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { STATUS_HIS_SMS } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { SMSManagementService } from 'src/app/services/sms-management'
import { smsManagementObject, smsManagementMapObject } from 'src/app/models/smsManagementObject'
import { SMSTreeviewI18n } from 'src/app/shared/sms-treeview-i18n'

declare var $: any

@Component({
	selector: 'sms-create-or-update',
	templateUrl: './sms-create-or-update.component.html',
	styleUrls: ['./sms-create-or-update.component.css'],
	providers: [{ provide: TreeviewI18n, useClass: SMSTreeviewI18n }],
})
export class SMSCreateOrUpdateComponent implements OnInit {
	model: smsManagementObject = new smsManagementObject()
	form: FormGroup
	submitted = false
	action: any
	title: string = 'Soạn thảo SMS'
	checkFirst: number
	ltsUnitFirst: any[]
	statusCurent: any = 1
	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
	]
	listItemUserSelected: Array<smsManagementMapObject>
	administrativeUnitId: any
	administrativeUnitsBase: any[]
	listIndividualAndBusinessGetByAdmintrativeId: any[]
	userId: any[] = []
	individual: boolean = true
	business: boolean = true
	individualBusinessInfo: any[]
	constructor(
		private _toastr: ToastrService,
		private formBuilder: FormBuilder,
		private router: Router,
		private smsService: SMSManagementService,
		private activatedRoute: ActivatedRoute
	) {
		this.listItemUserSelected = []
		this.listIndividualAndBusinessGetByAdmintrativeId = []
		this.individualBusinessInfo = []
		this.userId = []
		this.checkFirst = 0
		this.ltsUnitFirst = []
	}

	// treeview

	administrativeUnits: TreeviewItem[]
	ltsAdministrativeUnitId: string
	config = TreeviewConfig.create({
		hasAllCheckBox: true,
		hasFilter: true,
		hasCollapseExpand: true,
		decoupleChildFromParent: false,
		maxHeight: 400,
	})

	ngOnInit() {
		this.buildForm()
		this.administrativeUnits = []
		this.getSMSModelById()
	}

	getAdministrativeUnits() {
		this.smsService.GetListAdmintrative({ id: 37 }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.administrativeUnitsBase = res.result.CAAdministrativeUnitsGetDropDown
				if (this.administrativeUnitsBase.length > 0) {
					var itemFirst = new TreeViewDrop()
					itemFirst.text = this.administrativeUnitsBase[0].name
					itemFirst.value = this.administrativeUnitsBase[0].id
					itemFirst.children = []
					itemFirst.checked = false
					for (const iterator of this.administrativeUnitsBase.filter((x) => x.parentId == itemFirst.value)) {
						var item = new TreeViewDrop()
						item.value = iterator.id
						item.text = iterator.name
						item.children = []
						item.checked = false
						for (const iterator1 of this.administrativeUnitsBase.filter((x) => x.parentId == iterator.id)) {
							let item2 = new TreeViewDrop()
							item2.value = iterator1.id
							item2.text = iterator1.name
							item2.checked = false
							// if (this.ltsUnitFirst.includes(iterator1.value)) {
							// 	item2.checked = true
							// }
							item.children.push(item2)
						}
						itemFirst.children.push(item)
					}
					this.administrativeUnits = [new TreeviewItem({ ...itemFirst })]
					// this.onSelectedChange(this.ltsUnitFirst)
				}
			} else {
				this.administrativeUnits = []
			}
		})
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

	buildForm() {
		this.form = this.formBuilder.group({
			title: [this.model.title, Validators.required],
			content: [this.model.content, Validators.required],
			signature: [this.model.signature, Validators.required],
			administrativeUnitId: [this.administrativeUnitId],
			userId: [this.userId],
			business: [this.business],
			individual: [this.individual],
		})
	}

	getSMSModelById() {
		this.activatedRoute.params.subscribe((params) => {
			let id = +params['id']
			this.model.id = isNaN(id) == true ? 0 : id
			if (this.model.id != 0) {
				this.smsService.GetById({ id: this.model.id }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result) {
							this.model = { ...res.result.model }
							this.listItemUserSelected = [...res.result.individualBusinessInfo]
							// for (const iterator of this.listItemUserSelected) {
							// 	this.userId.push(iterator.id)
							// 	this.ltsAdministrativeUnitId += ',' + iterator.administrativeUnitId
							// 	// this.ltsUnitFirst.push(iterator.administrativeUnitId)
							// }
							this.getAdministrativeUnits()
						}
					}
				})
			}
		})
		this.action = this.model.id == 0 ? 'Lưu' : 'Lưu'
	}

	get f() {
		return this.form.controls
	}

	rebuilForm() {
		this.form.reset({
			title: this.model.title,
			content: this.model.content,
			signature: this.model.signature,
			administrativeUnitId: this.administrativeUnitId,
			userId: this.userId,
			individual: this.individual,
			business: this.business,
		})
	}

	onChange(category: number) {
		if (category == 1) {
			this.individual = !this.individual
		} else if (category == 2) {
			this.business = !this.business
		}
		this.onLoadListIndividualAndBusiness()
	}

	onLoadListIndividualAndBusiness() {
		if (this.ltsAdministrativeUnitId == '' && this.checkFirst > 1) {
			this._toastr.error('Vui lòng chọn đơn vị')
			this.listIndividualAndBusinessGetByAdmintrativeId = []
			return
		} else {
			this.checkFirst++
		}

		let type: number = 0
		if (this.individual == true && this.business == true) {
			type = 3
		} else {
			if (this.individual == true) {
				type = 1
			} else if (this.business == true) {
				type = 2
			} else {
				this._toastr.error('Vui lòng chọn cá nhân hoặc doanh nghiệp')
				this.listIndividualAndBusinessGetByAdmintrativeId = []
				return
			}
		}
		let obj = {
			LtsAdministrativeId: this.ltsAdministrativeUnitId,
			type: type,
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

	onSave(isSend: boolean) {
		this.model.status = isSend == false ? 1 : 2
		this.submitted = true

		this.model.title = this.model.title.trim()
		this.model.content = this.model.content.trim()
		this.model.signature = this.model.signature.trim()
		this.rebuilForm()

		if (this.form.invalid) {
			return
		}
		if (this.listItemUserSelected.length == 0 && this.model.status == 2) {
			this._toastr.error('Vui lòng chọn cá nhân, doanh nghiệp được gửi SMS')
			return
		}
		var s = []
		this.listItemUserSelected.forEach((item) => {
			s.push(item.category)
			let ob = {
				Id: item.id,
				Category: item.category,
				AdmintrativeUnitId: item.administrativeUnitId,
			}
			this.individualBusinessInfo.push(ob)
		})
		s = s.filter((x, index) => s.indexOf(x) === index)
		this.model.type = s.reduce((x, y) => {
			return (x += y + ',')
		}, '')
		this.model.type = this.model.type.substring(0, this.model.type.length - 1)
		var obj = {
			model: this.model,
			IndividualBusinessInfo: this.individualBusinessInfo,
		}
		if (this.model.id == 0 || this.model.id == null) {
			this.smsService.Insert(obj).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					// insert his

					this.smsService
						.InsertHisSMS({
							ObjectId: response.result,
							Status: STATUS_HIS_SMS.CREATE,
						})
						.subscribe((res) => {
							if (res.success == RESPONSE_STATUS.success) {
								if (isSend == true) {
									this.smsService
										.InsertHisSMS({
											ObjectId: response.result,
											Status: STATUS_HIS_SMS.SEND,
										})
										.subscribe()
								}
								return
							}
						})
					this._toastr.success(COMMONS.ADD_SUCCESS)
					this.redirectList()
					return
				} else {
					let res = isNaN(response.result) == true ? 0 : response.result
					if (res == -1) {
						this._toastr.error('Tiêu đề SMS đã tồn tại')
						return
					} else {
						this._toastr.error(response.message)
						return
					}
					return
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		} else {
			this.smsService.Update(obj).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					// ghi his

					this.smsService
						.InsertHisSMS({
							ObjectId: response.result,
							Status: STATUS_HIS_SMS.UPDATE,
						})
						.subscribe((res) => {
							if (res.success == RESPONSE_STATUS.success) {
								if (isSend == true) {
									this.smsService
										.InsertHisSMS({
											ObjectId: response.result,
											Status: STATUS_HIS_SMS.SEND,
										})
										.subscribe()
								}
							}
						})
					this._toastr.success(COMMONS.UPDATE_SUCCESS)
					this.redirectList()

					return
				} else {
					let res = isNaN(response.result) == true ? 0 : response.result
					if (res == -1) {
						this._toastr.error('Tiêu đề SMS đã tồn tại')
						return
					} else {
						this._toastr.error(response.message)
						return
					}
					return
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		}
	}
	redirectList() {
		this.router.navigate(['/quan-tri/email-sms/sms'])
	}

	onCreateUser() {
		if (this.ltsAdministrativeUnitId == undefined || this.ltsAdministrativeUnitId == '') {
			this._toastr.error('Vui lòng chọn đơn vị')
			return
		}
		if (this.userId != undefined && this.userId.length > 0 && this.userId != null) {
			this.listItemUserSelected = []
			for (const iterator of this.userId) {
				var obj = new smsManagementMapObject()
				obj.id = iterator.id
				obj.category = iterator.category
				obj.name = iterator.name
				obj.administrativeUnitName = iterator.administrativeUnitName
				obj.administrativeUnitId = iterator.administrativeUnitId
				this.listItemUserSelected.push(obj)
			}
			this._toastr.success('Thêm mới thành công ' + this.userId.length + ' người dùng!')
		} else {
			this._toastr.error('Vui lòng chọn cá nhân, doanh nghiệp')
			return
		}
	}
	onRemoveUser(item: any) {
		this.listItemUserSelected = this.listItemUserSelected.filter((x) => x.id != item.id)
		return
	}
}

class TreeViewDrop {
	text: string
	value: number
	children: any[]
	checked: boolean
}

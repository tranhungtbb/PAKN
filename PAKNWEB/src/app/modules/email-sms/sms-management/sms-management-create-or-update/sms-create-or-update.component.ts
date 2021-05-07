import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { CONSTANTS, STATUS_HISNEWS, FILETYPE } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { SMSManagementService } from 'src/app/services/sms-management'
import { smsManagementObject, smsManagementMapObject } from 'src/app/models/smsManagementObject'
import { from } from 'rxjs'
import { iterator } from 'rxjs/internal-compatibility'

declare var $: any

@Component({
	selector: 'sms-create-or-update',
	templateUrl: './sms-create-or-update.component.html',
	styleUrls: ['./sms-create-or-update.component.css'],
})
export class SMSCreateOrUpdateComponent implements OnInit {
	model: smsManagementObject = new smsManagementObject()
	form: FormGroup
	submitted = false
	action: any
	title: string
	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
	]
	AdministrativeUnits: any[]
	listItemUserSelected: Array<smsManagementMapObject>
	administrativeUnitId: any
	listIndividualAndBusinessGetByAdmintrativeId: any[]
	userId: any
	individual: boolean = false
	business: boolean = false
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
	}

	ngOnInit() {
		this.getAdministrativeUnits()
		this.buildForm()
		// this.getInvitatonModelById()
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

	getInvitatonModelById() {
		this.activatedRoute.params.subscribe((params) => {
			let id = +params['id']
			this.model.id = isNaN(id) == true ? 0 : id
			if (this.model.id != 0) {
				this.smsService.GetById({ id: this.model.id }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						debugger
						if (res.result) {
							this.model = { ...res.result.model }
							this.listItemUserSelected = [...res.result.individualBusinessInfo]
						}
					}
				})
			}
		})
		this.action = this.model.id == 0 ? 'Lưu' : 'Lưu'
		this.title = this.model.id == 0 ? 'Soạn thảo SMS' : 'Soạn thảo SMS'
	}

	getAdministrativeUnits() {
		this.smsService.GetListAdmintrative({ id: 37 }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.AdministrativeUnits = res.result.CAAdministrativeUnitsGetDropDown
				this.getInvitatonModelById()
			} else {
				this.AdministrativeUnits = []
			}
		})
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
		this.listIndividualAndBusinessGetByAdmintrativeId = []
		let type: number
		if (this.individual == true && this.business == true) {
			type = 3
		} else {
			if (this.individual == true) {
				type = 1
			} else if (this.business == true) {
				type = 2
			} else {
				this._toastr.error('Vui lòng chọn cá nhân hoặc doanh nghiệp')
				return
			}
		}
		if (this.administrativeUnitId == undefined) {
			this._toastr.error('Vui lòng chọn đơn vị')
			return
		}
		this.smsService
			.GetListIndividualAndBusinessByAdmintrativeUnitId({
				id: this.administrativeUnitId,
				type: type,
			})
			.subscribe((res) => {
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
		if (this.listItemUserSelected.length == 0) {
			this._toastr.error('Chưa có cá nhân, doanh nghiệp được gửi')
			return
		}
		this.model.title = this.model.title.trim()
		this.model.content = this.model.content.trim()
		this.model.signature = this.model.signature.trim()
		this.rebuilForm()

		if (this.form.invalid) {
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
		debugger
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
					this._toastr.success(COMMONS.ADD_SUCCESS)
					this.redirectList()
					return
				} else {
					this._toastr.error(response.message)
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
					this._toastr.success(COMMONS.UPDATE_SUCCESS)
					this.redirectList()
					return
				} else {
					this._toastr.error(response.message)
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
		if (this.userId != undefined) {
			if (this.listItemUserSelected.length == 0) {
				let item = this.listIndividualAndBusinessGetByAdmintrativeId.find((x) => x.id == this.userId)
				var obj = new smsManagementMapObject()
				obj.id = this.userId
				obj.category = item.category
				obj.name = item.name
				obj.administrativeUnitName = item.administrativeUnitName
				obj.administrativeUnitId = item.administrativeUnitId
				this.listItemUserSelected.push(obj)
			} else {
				let check = this.listItemUserSelected.find((x) => x.id == this.userId)
				if (check != undefined) {
					this._toastr.error('Bạn đã chọn người này')
					return
				}
				let item = this.listIndividualAndBusinessGetByAdmintrativeId.find((x) => x.id == this.userId)
				var obj = new smsManagementMapObject()
				obj.id = this.userId
				obj.category = item.category
				obj.name = item.name
				obj.administrativeUnitName = item.administrativeUnitName
				obj.administrativeUnitId = item.administrativeUnitId
				this.listItemUserSelected.push(obj)
			}
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

import { Component, OnInit, ElementRef } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { UnitService } from '../../../../../services/unit.service'
import { UserService } from '../../../../../services/user.service'
import { PositionService } from '../../../../../services/position.service'
import { RoleService } from '../../../../../services/role.service'
import { AppSettings } from 'src/app/constants/app-setting'

import { COMMONS } from 'src/app/commons/commons'
import { UserObject2 } from 'src/app/models/UserObject'

import { UnitComponent } from '../../unit/unit.component'

declare var jquery: any
declare var $: any
@Component({
	selector: 'app-user-create-or-update',
	templateUrl: './user-create-or-update.component.html',
	styleUrls: ['./user-create-or-update.component.css'],
})
export class UserCreateOrUpdateComponent implements OnInit {
	constructor(
		private elm: ElementRef,
		private unitService: UnitService,
		private userService: UserService,
		private positionService: PositionService,
		private formBuilder: FormBuilder,
		private toast: ToastrService,
		private roleService: RoleService,
		private sanitizer: DomSanitizer
	) {
		this.modalId = elm.nativeElement.getAttribute('modalid')
	}

	modalId = ''
	public parentUnit: UnitComponent
	editByMyself: boolean = false

	modelUser: UserObject2 = new UserObject2()
	createUserForm: FormGroup
	modalTitle: string = 'Thông tin người dùng'
	unitId: number

	positionsList: any[] = []
	rolesList: any[] = []
	unitsList: any[] = []
	selectedRoles: Array<number>

	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	listGender: any = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	ngOnInit() {
		this.createUserForm = this.formBuilder.group({
			//userName: ['', [Validators.required]],
			email: ['', [Validators.required, Validators.email]],
			fullName: ['', [Validators.required]],
			phone: ['', [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			positionId: ['', [Validators.required]],
			unitId: ['', [Validators.required]],
			gender: ['', [Validators.required]],
			roleId: ['', [Validators.required]],
			isActived: ['', [Validators.required]],
			address: [''],
		})

		this.positionService
			.positionGetList({
				pageIndex: 1,
				pageSize: 1000,
			})
			.subscribe((res) => {
				if (res.success != 'OK') return
				this.positionsList = res.result.CAPositionGetAllOnPage
			})
		this.roleService.getAll({}).subscribe((res) => {
			if (res.success != 'OK') return
			this.rolesList = res.result.SYRoleGetAll
		})
		this.unitService.getAll({}).subscribe((res) => {
			if (res.success != 'OK') return
			this.unitsList = res.result.CAUnitGetAll
		})
	}

	get fUser() {
		return this.createUserForm.controls
	}
	userFormSubmitted = false
	onSaveUser(): void {
		this.userFormSubmitted = true
		this.modelUser.userName = this.modelUser.email
		this.modelUser.typeId = 1
		if (this.createUserForm.invalid) {
			this.toast.error('Dữ liệu không hợp lệ')
			return
		}

		//avatar file;
		let files = $('#' + this.modalId + ' .seclect-avatar')[0].files

		this.modelUser.roleIds = this.selectedRoles.toString()
		this.modelUser.userName = this.modelUser.email
		this.modelUser.avatar = ''
		this.modelUser.countLock = 0
		this.modelUser.lockEndOut = ''

		if (this.modelUser.id != null && this.modelUser.id > 0) {
			this.userService.update(this.modelUser, files).subscribe((res) => {
				$('#' + this.modalId + ' .seclect-avatar').val('')

				if (res.success != 'OK') {
					let errorMsg = res.message
					this.toast.error(errorMsg)
					return
				}
				this.toast.success(COMMONS.UPDATE_SUCCESS)

				if (!this.editByMyself) {
					this.parentUnit.getUserPagedList()
				}

				this.modelUser = new UserObject2()
				$('#' + this.modalId).modal('hide')
			})
		} else {
			this.userService.insert(this.modelUser, files).subscribe((res) => {
				$('#' + this.modalId + ' .seclect-avatar').val('')

				if (res.success != 'OK') {
					let errorMsg = res.message
					this.toast.error(errorMsg)
					return
				}
				this.toast.success(COMMONS.ADD_SUCCESS)
				this.parentUnit.getUserPagedList()
				this.modelUser = new UserObject2()
				$('#' + this.modalId).modal('hide')
			})
		}
	}

	onChangeAvatar() {
		$('#' + this.modalId + ' .seclect-avatar').click()
	}
	changeSelectAvatar(event: any) {
		var file = event.target.files[0]

		if (!['image/jpeg', 'image/png'].includes(file.type)) {
			this.toast.error('Chỉ chọn tệp tin ảnh')
			event.target.value = null
			return
		}
		if (file.size > 3000000) {
			this.toast.error('Ảnh dung lượng tối đa 3MB')
			event.target.value = null
			return
		}

		let output: any = $('#' + this.modalId + ' .user-avatar-view')[0]
		output.src = URL.createObjectURL(file)
		output.onload = function () {
			URL.revokeObjectURL(output.src) // free memory
		}
	}

	modal_btn_save = 'Tạo mới'
	openModal(unitId = 0, userId = 0, editByMyself = false): void {
		this.createUserForm = this.formBuilder.group({
			//userName: ['', [Validators.required]],
			email: [this.modelUser.email, [Validators.required, Validators.email]],
			fullName: [this.modelUser.fullName, [Validators.required]],
			phone: [this.modelUser.phone, [Validators.required, Validators.pattern('^(84|0[3|5|7|8|9])+([0-9]{8})$')]],
			positionId: [this.modelUser.positionId, [Validators.required]],
			unitId: [this.modelUser.unitId, [Validators.required]],
			gender: [this.modelUser.gender, [Validators.required]],
			roleId: [this.modelUser.positionId, [Validators.required]],
			isActived: [this.modelUser.isActived, [Validators.required]],
			address: [this.modelUser.address],
		})

		this.userFormSubmitted = false
		this.modelUser = new UserObject2()

		this.modelUser.unitId = unitId
		if (userId > 0) {
			this.modalTitle = 'Chỉnh sửa người dùng'
			this.modal_btn_save = 'Cập nhật'
			this.userService.getById({ id: userId }).subscribe((res) => {
				if (res.success != 'OK') return
				this.modelUser = res.result.SYUserGetByID[0]

				//if (this.modelUser.avatar != null && this.modelUser.avatar != '') this.getUserAvatar(this.modelUser.id)
				this.userAvatar = AppSettings.API_DOWNLOADFILES + '/' + this.modelUser.avatar

				if (this.modelUser.roleIds) this.selectedRoles = this.modelUser.roleIds.split(',').map((c) => parseInt(c))
				else this.selectedRoles = []
			})
		} else {
			this.modalTitle = 'Tạo mới người dùng'
			this.modal_btn_save = 'Tạo mới'
			//set value
			this.modelUser.positionId = null
			this.modelUser.gender = null
			this.modelUser.isActived = true
		}

		$('#' + this.modalId + ' .user-avatar-view').attr('src', '')
		$('#' + this.modalId).modal('show')

		this.editByMyself = editByMyself
	}

	userAvatar: any
	getUserAvatar(id: number) {
		this.userService.getAvatar(id).subscribe((res) => {
			if (res) {
				let objectURL = 'data:image/jpeg;base64,' + res
				this.userAvatar = this.sanitizer.bypassSecurityTrustUrl(objectURL)
			}
		})
	}
}

import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { UnitService } from '../../../../../services/unit.service'
import { UserService } from '../../../../../services/user.service'
import { PositionService } from '../../../../../services/position.service'
import { RoleService } from '../../../../../services/role.service'

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
		private unitService: UnitService,
		private userService: UserService,
		private positionService: PositionService,
		private formBuilder: FormBuilder,
		private toast: ToastrService,
		private roleService: RoleService,
		private parentUnit: UnitComponent
	) {}

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
			isActived: [''],
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
		this.modelUser.roleIds = this.selectedRoles.toString()
		this.modelUser.userName = this.modelUser.email
		if (this.modelUser.id != null && this.modelUser.id > 0) {
			this.userService.update(this.modelUser).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.UPDATE_FAILED)
					return
				}
				this.toast.success(COMMONS.UPDATE_SUCCESS)
				this.parentUnit.getUserPagedList()
				this.modelUser = new UserObject2()
				$('#modal-user-create-or-update').modal('hide')
			})
		} else {
			this.userService.insert(this.modelUser).subscribe((res) => {
				if (res.success != 'OK') {
					this.toast.error(COMMONS.ADD_FAILED)
					return
				}
				this.toast.success(COMMONS.ADD_SUCCESS)
				this.parentUnit.getUserPagedList()
				this.modelUser = new UserObject2()
				$('#modal-user-create-or-update').modal('hide')
			})
		}
	}

	onChangeAvatar() {
		$('#seclect-avatar').click()
	}
	changeSelectAvatar(event: any) {
		var file = event.target.files[0]
		if (!['image/jpeg', 'image/png'].includes(file.type)) {
			this.toast.error('Chỉ chọn tệp tin ảnh')
			event.target.value = null
			return
		}

		var reader = new FileReader()
		reader.onload = function (e) {
			$('#avatar-img').attr('src', e.target.result)
		}
		reader.readAsDataURL(file) // convert to base64 string
	}

	openModal(unitId = 0, userId = 0): void {
		this.modelUser = new UserObject2()
		this.userFormSubmitted = false
		this.modelUser.unitId = unitId
		if (userId > 0) {
			this.modalTitle = 'Sửa người dùng'
			this.userService.getById({ id: userId }).subscribe((res) => {
				if (res.success != 'OK') return
				this.modelUser = res.result.SYUserGetByID[0]
				if (this.modelUser.roleIds) this.selectedRoles = this.modelUser.roleIds.split(',').map((c) => parseInt(c))
				else this.selectedRoles = []
			})
		} else {
			this.modalTitle = 'Tạo người dùng mới'
		}
		$('#modal-user-create-or-update').modal('show')
	}
}

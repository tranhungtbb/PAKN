import { Component, OnInit, ViewChild } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { Router, ActivatedRoute } from '@angular/router'
import { RESPONSE_STATUS } from '../../../../../constants/CONSTANTS'
import { CONSTANTS, STATUS_HISNEWS } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { RoleObject } from '../../../../../models/roleObject'
import { RoleService } from '../../../../../services/role.service'
import { from } from 'rxjs'
import { UserService } from 'src/app/services/user.service'

declare var $: any

@Component({
	selector: 'app-role-create-or-update',
	templateUrl: './role-create-or-update.component.html',
	styleUrls: ['./role-create-or-update.component.css'],
})
export class RoleCreateOrUpdateComponent implements OnInit {
	model: RoleObject = new RoleObject()
	form: FormGroup
	submitted = false
	action: any
	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	listUserIsSystem: any[]
	listItemUserSelected: any[]
	userId: any
	constructor(
		private _toastr: ToastrService,
		private formBuilder: FormBuilder,
		private router: Router,
		private roleService: RoleService,
		private userService: UserService,
		private activatedRoute: ActivatedRoute
	) {
		this.listItemUserSelected = []
	}

	ngOnInit() {
		this.getRoleById()
		this.buildForm()
		this.getUsersIsSystem()
	}

	buildForm() {
		this.form = this.formBuilder.group({
			name: [this.model.name, Validators.required],
			isActived: [this.model.isActived, Validators.required],
			orderNumber: [this.model.orderNumber],
			description: [this.model.description],
			userId: [this.userId],
		})
	}

	getRoleById() {
		this.activatedRoute.params.subscribe((params) => {
			let id = +params['id']
			this.model.id = isNaN(id) == true ? 0 : id
			if (this.model.id != 0) {
				this.roleService.getRoleById({ id: this.model.id }).subscribe((res) => {
					if (res.success == RESPONSE_STATUS.success) {
						if (res.result.SYRoleGetByID) {
							this.model = { ...res.result.SYRoleGetByID[0] }
						}
					}
				})
			}
		})
		this.action = this.model.id == 0 ? 'Thêm mới' : 'Cập nhập'
	}

	getUsersIsSystem() {
		this.userService.getIsSystem({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.listUserIsSystem = res.result.SYUserGetIsSystem
			} else {
				this.listUserIsSystem = []
			}
		})
	}

	get f() {
		return this.form.controls
	}

	rebuilForm() {
		this.form.reset({
			name: this.model.name,
			isActived: this.model.isActived,
			orderNumber: this.model.orderNumber,
			description: this.model.description,
			userId: this.userId,
		})
	}

	onSave() {
		this.submitted = true
		this.rebuilForm()
		this.model.name = this.model.name.trim()
		if (this.model.name == '') return
		if (this.form.invalid) {
			return
		}
		if (this.model.id == 0 || this.model.id == null) {
			this.roleService.insert(this.model).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error('Vai trò đã bị trùng tên')
					} else {
						this._toastr.success(COMMONS.ADD_SUCCESS)
						this.onCreateUserRole(response.result)
						this.redirectList()
					}
				} else {
					this._toastr.error(response.message)
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		} else {
			this.roleService.update(this.model).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error('Vai trò đã bị trùng tên')
					} else if (response.result == 0) {
						this._toastr.success(COMMONS.UPDATE_FAILED)
						this.redirectList()
					} else {
						this._toastr.success(COMMONS.UPDATE_SUCCESS)
						this._toastr.success(COMMONS.ADD_SUCCESS)
						this.onCreateUserRole(response.result)
						this.redirectList()
					}
				} else {
					this._toastr.error(response.message)
				}
			}),
				(error) => {
					console.error(error)
					alert(error)
				}
		}
	}
	redirectList() {
		this.router.navigate(['quan-tri/he-thong/vai-tro'])
	}

	onCreateUser() {
		if (this.listItemUserSelected.length == 0) {
			let item = this.listUserIsSystem.find((x) => x.value == this.userId)
			this.listItemUserSelected.push(item)
			console.log(this.listItemUserSelected)
		} else {
			let check = this.listItemUserSelected.find((x) => x.value == this.userId)
			if (check != undefined) {
				this._toastr.error('Bạn đã chọn người này')
				return
			}
			let item = this.listUserIsSystem.find((x) => x.value == this.userId)
			this.listItemUserSelected.push(item)
			console.log(this.listItemUserSelected)
		}
	}
	onRemoveUser(item: any) {
		this.listItemUserSelected = this.listItemUserSelected.filter((x) => x.value != item.value)
		return
	}

	onCreateUserRole(roleId: any) {
		if (this.listItemUserSelected.length == 0) {
			return
		} else {
			let listModel = []
			this.listItemUserSelected.forEach((item) => {
				listModel.push({
					UserId: item.value,
					RoleId: roleId,
				})
			})
			this.userService.insertMultiUserRole(listModel).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.redirectList()
				}
			})
		}
	}
}

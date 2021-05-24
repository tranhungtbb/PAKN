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
	listPermissionCategories: any[]
	listPermissionGroupUserSelected: any[] = []
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
			orderNumber: [this.model.orderNumber, Validators.pattern(/^-?(0|[1-9]\d*)?$/)],
			description: [this.model.description],
			userId: [this.userId],
		})
	}

	getRoleById() {
		this.roleService.getDataForCreate({}).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.listPermissionCategories = res.result
				this.activatedRoute.params.subscribe((params) => {
					let id = +params['id']
					this.model.id = isNaN(id) == true ? 0 : id
					if (this.model.id != 0) {
						this.roleService.getRoleById({ id: this.model.id }).subscribe((res) => {
							if (res.success == RESPONSE_STATUS.success) {
								this.model = res.result.Data
								this.listPermissionGroupUserSelected = res.result.ListPermission
								this.onGroupUserLoadPermission(this.listPermissionGroupUserSelected)
							}
						})
					}
					this.action = this.model.id == 0 ? 'Thêm mới vai trò' : 'Chỉnh sửa vai trò'
				})
			}
		})
	}

	getUsersByRoleId(roleId: any) {
		this.userService.getByRoleId({ RoleId: roleId }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				res.result.SYUserGetAllByRoleId.forEach((element) => {
					var obj = {
						value: element.id,
						text: element.userName,
					}
					this.listItemUserSelected.push(obj)
				})
			}
		})
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
						return
					} else {
						this._toastr.success(COMMONS.ADD_SUCCESS)
						this.onCreateUserRole(response.result)
						this.onCreatePermission(response.result)
						this.redirectList()
						return
					}
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
			this.roleService.update(this.model).subscribe((response) => {
				if (response.success == RESPONSE_STATUS.success) {
					if (response.result == -1) {
						this._toastr.error('Vai trò đã bị trùng tên')
					} else if (response.result == 0) {
						this._toastr.success(COMMONS.UPDATE_FAILED)
						this.redirectList()
						return
					} else {
						this._toastr.success(COMMONS.UPDATE_SUCCESS)
						this.onCreateUserRole(response.result)
						this.onCreatePermission(response.result)
						this.redirectList()
						return
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

	onCreatePermission(id) {
		var request = {
			lstid: this.listPermissionGroupUserSelected.join(','),
			GroupUserId: id,
		}
		this.roleService.insertPermission(request).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.redirectList()
			}
		})
	}

	onCreateUser() {
		if (this.listItemUserSelected.length == 0) {
			let item = this.listUserIsSystem.find((x) => x.value == this.userId)
			this.listItemUserSelected.push(item)
		} else {
			let check = this.listItemUserSelected.find((x) => x.value == this.userId)
			if (check != undefined) {
				this._toastr.error('Bạn đã chọn người này')
				return
			}
			let item = this.listUserIsSystem.find((x) => x.value == this.userId)
			this.listItemUserSelected.push(item)
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

	onGroupUserLoadPermission(listPermissionGroupUserSelected): void {
		this.clearPermisison()
		for (var i = 0; i < this.listPermissionGroupUserSelected.length; i++) {
			this.checkPermission(this.listPermissionGroupUserSelected[i], true)
		}
	}

	onCategoryChange(ev, permisionCategory): void {
		for (var i = 0; i < permisionCategory.function.length; i++) {
			permisionCategory.function[i].selected = ev.checked
			for (var j = 0; j < permisionCategory.function[i].permission.length; j++) {
				permisionCategory.function[i].permission[j].selected = ev.checked
				var permissionId = permisionCategory.function[i].permission[j].id
				if (ev.checked) {
					this.listPermissionGroupUserSelected.push(permissionId)
				} else {
					var index = this.listPermissionGroupUserSelected.indexOf(permissionId, 0)
					if (index > -1) {
						this.listPermissionGroupUserSelected.splice(index, 1)
					}
				}
			}
		}
	}

	onFunctionChange(ev, funct, permisionCategory): void {
		for (var j = 0; j < funct.permission.length; j++) {
			funct.permission[j].selected = ev.checked
			var permissionId = funct.permission[j].id
			if (ev.checked) {
				this.listPermissionGroupUserSelected.push(permissionId)
			} else {
				var index = this.listPermissionGroupUserSelected.indexOf(permissionId, 0)
				if (index > -1) {
					this.listPermissionGroupUserSelected.splice(index, 1)
				}
			}
		}
		if (ev.checked) {
			permisionCategory.selected = ev.checked
		} else {
			this.checkCategorySelected(permisionCategory)
		}
	}

	onPermissionChange(event, permission, funct, permisionCategory): void {
		this.checkFunctionSelected(permisionCategory, funct)
		if (event.checked) {
			this.listPermissionGroupUserSelected.push(permission.id)
		} else {
			var index = this.listPermissionGroupUserSelected.indexOf(permission.id, 0)
			if (index > -1) {
				this.listPermissionGroupUserSelected.splice(index, 1)
			}
		}
	}

	private checkCategorySelected(permisionCategory: any) {
		var hasSelectedChild = false
		for (var j = 0; j < permisionCategory.function.length; j++) {
			if (permisionCategory.function[j].selected) {
				hasSelectedChild = true
				break
			}
		}
		permisionCategory.selected = hasSelectedChild
	}

	private checkFunctionSelected(permisionCategory, funct) {
		var hasSelectedChild = false
		for (var j = 0; j < funct.permission.length; j++) {
			if (funct.permission[j].selected) {
				hasSelectedChild = true
				break
			}
		}
		funct.selected = hasSelectedChild
		this.checkCategorySelected(permisionCategory)
	}

	private clearPermisison() {
		for (var i = 0; i < this.listPermissionCategories.length; i++) {
			var permissioncategory = this.listPermissionCategories[i]
			permissioncategory.selected = false
			permissioncategory.disabled = false
			for (var j = 0; j < permissioncategory.function.length; j++) {
				var funct = permissioncategory.function[j]
				funct.selected = false
				funct.disabled = false
				for (var k = 0; k < funct.permission.length; k++) {
					funct.permission[k].selected = false
					funct.permission[k].disabled = false
				}
			}
		}
	}

	private checkPermission(permissionId, isUserSelected) {
		for (var i = 0; i < this.listPermissionCategories.length; i++) {
			var permissioncategory = this.listPermissionCategories[i]
			var isCategorySelected = this.listPermissionCategories[i].selected
			var isCategoryDisabled = this.listPermissionCategories[i].disabled
			for (var j = 0; j < permissioncategory.function.length; j++) {
				var funct = permissioncategory.function[j]
				var isFunctSelected = permissioncategory.function[j].selected
				var isFunctDisabled = permissioncategory.function[j].disabled
				for (var k = 0; k < funct.permission.length; k++) {
					if (funct.permission[k].id == permissionId) {
						funct.permission[k].selected = true
						funct.permission[k].disabled = isUserSelected ? false : true
						isCategorySelected = true
						isFunctSelected = true
						isCategoryDisabled = isUserSelected ? isCategoryDisabled : true
						isFunctDisabled = isUserSelected ? isFunctDisabled : true
					}
				}
				funct.selected = isFunctSelected
				funct.disabled = isFunctDisabled
			}
			permissioncategory.selected = isCategorySelected
			permissioncategory.disabled = isCategoryDisabled
		}
	}
}

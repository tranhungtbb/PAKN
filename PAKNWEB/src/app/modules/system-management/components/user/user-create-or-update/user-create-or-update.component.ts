import { Component, OnInit, ElementRef } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { UnitService } from '../../../../../services/unit.service'
import { UserService } from '../../../../../services/user.service'
import { PositionService } from '../../../../../services/position.service'
import { RoleService } from '../../../../../services/role.service'
import { AppSettings } from 'src/app/constants/app-setting'

import { CONSTANTS, MESSAGE_COMMON, PROCESS_STATUS_RECOMMENDATION, RECOMMENDATION_STATUS, RESPONSE_STATUS, STEP_RECOMMENDATION } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { UserObject2 } from 'src/app/models/UserObject'

import { UnitComponent } from '../../unit/unit.component'
import { UserComponent } from 'src/app/modules/system-management/components/user/user.component'
import { BusinessComponent } from 'src/app/modules/business.component'
import file_uploader from 'devextreme/ui/file_uploader'
import { Console } from 'console'
import { debounceTime } from 'rxjs/operators'

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
		this.isOrganizational = elm.nativeElement.getAttribute('isOrganizational') == 'true' ? true : false
	}

	modalId = ''
	isOrganizational: boolean = false
	public parentUnit: UnitComponent
	public parentUser: UserComponent
	public parent_BusinessComponent: BusinessComponent
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
	fileAccept = CONSTANTS.FILEACCEPTAVATAR
	listPermissionCategories: any[]
	listPermissionUserSelected: any[] = []

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
		this.userService.getDataForCreate({}).subscribe((res) => {
			if (res.success == 'OK') {
				this.positionsList = res.result.lstPossition
				this.unitsList = res.result.lstUnit
				this.rolesList = res.result.lstRoles
				this.listPermissionCategories = res.result.lstPermissionCategories
			}
		})
	}

	get fUser() {
		return this.createUserForm.controls
	}
	checkExists = {
		Email: false,
		Phone: false,
	}
	onCheckExist(field: string, value: string) {
		this.userService
			.checkExists({
				field,
				value,
				id: this.modelUser.id ? this.modelUser.id : 0,
			})
			.subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.checkExists[field] = res.result.SYUserCheckExists[0].exists
				}
			})
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
		if (this.checkExists['Email'] || this.checkExists['Phone']) return

		//avatar file;
		let files = $('#' + this.modalId + ' .seclect-avatar')[0].files

		this.modelUser.roleIds = this.selectedRoles.toString()
		this.modelUser.permissionIds = this.listPermissionUserSelected.join(',')
		this.modelUser.userName = this.modelUser.email
		// this.modelUser.avatar = ''
		this.modelUser.countLock = 0
		this.modelUser.lockEndOut = ''
		if (this.modelUser.id != null && this.modelUser.id > 0) {
			this.modelUser.avatar = this.modelUser.avatar
			this.modelUser.address == null ? (this.modelUser.address = '') : (this.modelUser.address = this.modelUser.address.trim())
			this.userService.update(this.modelUser, files).subscribe((res) => {
				$('#' + this.modalId + ' .seclect-avatar').val('')

				if (res.success != 'OK') {
					let errorMsg = res.message
					this.toast.error(errorMsg)
					return
				}
				this.toast.success(COMMONS.UPDATE_SUCCESS)
				if (this.isOrganizational == true) {
					if (!this.editByMyself) {
						this.parentUnit.getUserPagedList()
					}
				} else {
					this.parentUser.getList()
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
				if (this.isOrganizational == true) {
					if (!this.editByMyself) {
						this.parentUnit.getUserPagedList()
					}
				} else {
					this.parentUser.getList()
				}
				this.toast.success(COMMONS.ADD_SUCCESS)
				// this.parentUnit.getUserPagedList()
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
		let output: any = $('#' + this.modalId + ' .user-avatar-view')
		// output.src = URL.createObjectURL(file)
		output.attr('src', URL.createObjectURL(file))
		this.userAvatar = ''
		output.onload = function () {
			URL.revokeObjectURL(output.src) // free memory
		}
	}

	modal_btn_save = 'Tạo mới'
	openModal(unitId = 0, userId = 0, editByMyself = false): void {
		this.listPermissionUserSelected = []
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
		if (this.isOrganizational == true) {
			if (!this.editByMyself) this.modelUser.unitId = unitId
		}
		if (userId > 0) {
			this.modalTitle = 'Chỉnh sửa người dùng'
			this.modal_btn_save = 'Cập nhật'
			this.userService.getByIdUpdate({ id: userId }).subscribe((res) => {
				if (res.success != 'OK') return
				this.modelUser = res.result.SYUserGetByID[0]
				if (this.modelUser.avatar == '' || this.modelUser.avatar == null) {
					this.userAvatar = null
				} else {
					this.userAvatar = this.modelUser.avatar
					let output: any = $('#' + this.modalId + ' .user-avatar-view')
					output.attr('src', this.userAvatar)
				}
				this.listPermissionUserSelected = res.result.lstPermissionUserSelected
				if (this.modelUser.roleIds) this.selectedRoles = this.modelUser.roleIds.split(',').map((c) => parseInt(c))
				else this.selectedRoles = []
				this.onGroupUserChange()
			})
		} else {
			this.modalTitle = 'Tạo mới người dùng'
			this.modal_btn_save = 'Tạo mới'
			//set value
			this.userAvatar = null
			this.modelUser.gender = true
			this.modelUser.positionId = null
			this.modelUser.isActived = true
			this.selectedRoles = []
			this.onGroupUserChange()
		}

		// $('#' + this.modalId + ' .user-avatar-view').attr('src', '')
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

	onGroupUserChange(): void {
		this.clearPermisison()
		let listGroup: any[] = []
		for (var i = 0; i < this.selectedRoles.length; i++) {
			for (let j = 0; j < this.rolesList.length; j++) {
				if (this.selectedRoles[i] == this.rolesList[i].value) {
					listGroup.push(this.rolesList[i])
					break
				}
			}
		}

		for (var i = 0; i < this.listPermissionUserSelected.length; i++) {
			this.checkPermission(this.listPermissionUserSelected[i], true)
		}

		for (var j = 0; j < listGroup.length; j++) {
			for (var i = 0; i < listGroup[j].permissionIds.length; i++) {
				this.checkPermission(listGroup[j].permissionIds[i], false)
			}
		}
	}

	onCategoryChange(ev, permisionCategory): void {
		for (var i = 0; i < permisionCategory.function.length; i++) {
			permisionCategory.function[i].selected = ev.checked
			for (var j = 0; j < permisionCategory.function[i].permission.length; j++) {
				permisionCategory.function[i].permission[j].selected = ev.checked
				var permissionId = permisionCategory.function[i].permission[j].id
				if (ev.checked) {
					this.listPermissionUserSelected.push(permissionId)
				} else {
					var index = this.listPermissionUserSelected.indexOf(permissionId, 0)
					if (index > -1) {
						this.listPermissionUserSelected.splice(index, 1)
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
				this.listPermissionUserSelected.push(permissionId)
			} else {
				var index = this.listPermissionUserSelected.indexOf(permissionId, 0)
				if (index > -1) {
					this.listPermissionUserSelected.splice(index, 1)
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
			this.listPermissionUserSelected.push(permission.id)
		} else {
			var index = this.listPermissionUserSelected.indexOf(permission.id, 0)
			if (index > -1) {
				this.listPermissionUserSelected.splice(index, 1)
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

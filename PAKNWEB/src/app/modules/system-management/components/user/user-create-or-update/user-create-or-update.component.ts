import { Component, OnInit } from '@angular/core'
import { MatDialog } from '@angular/material'
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'

import { UnitService } from '../../../../../services/unit.service'
import { UserService } from '../../../../../services/user.service'
import { PositionService } from '../../../../../services/position.service'
import { RoleService } from '../../../../../services/role.service'

import { COMMONS } from 'src/app/commons/commons'
import {UserObject2} from 'src/app/models/UserObject'

import {UnitComponent} from '../../unit/unit.component'

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
		private _toastr: ToastrService,
		private dialog: MatDialog,
		private roleService: RoleService,
		private parentUnit: UnitComponent
	) {}

	modelUser: UserObject2 = new UserObject2()
	createUserForm:FormGroup
	modalTitle:string = 'Thông tin người dùng'
	unitId:number

	positionsList:any[] = []
	rolesList:any[] = []
	unitsList:any[] = []

	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Không hiệu lực' },
	]
	listGender: any = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	ngOnInit() {

		this.createUserForm = this.formBuilder.group({
			userName: ['', [Validators.required]],
			email: ['', [Validators.required, Validators.pattern('^[a-z][a-z0-9_.]{5,32}@[a-z0-9]{2,}(.[a-z0-9]{2,4}){1,2}$')]],
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

		if (this.createUserForm.invalid) {
			this._toastr.error('Dữ liệu không hợp lệ')
			return
		}

		if (this.modelUser.id != null && this.modelUser.id > 0) {
			this.userService.update(this.modelUser).subscribe((res) => {
				if (res.success != 'OK') {
					this._toastr.error(COMMONS.UPDATE_FAILED)
					return
				}
				this._toastr.success(COMMONS.UPDATE_SUCCESS)
				this.parentUnit.getUserPagedList()
				this.modelUser = new UserObject2()
				$('#modal-user-create-or-update').modal('hide')
			})
		} else {
			this.userService.insert(this.modelUser).subscribe((res) => {
				if (res.success != 'OK') {
					this._toastr.error(COMMONS.ADD_FAILED)
					return
				}
				this._toastr.success(COMMONS.ADD_SUCCESS)
				this.parentUnit.getUserPagedList()
				this.modelUser = new UserObject2()
				$('#modal-user-create-or-update').modal('hide')
			})
		}
	}


	openModal(unitId=0,userId=0): void {
		this.modelUser = new UserObject2();
		this.userFormSubmitted = false;
		this.modelUser.unitId = unitId;
		if(userId > 0){
			this.modalTitle = "Tạo người dùng mới"
			this.userService.getById({ id: userId }).subscribe((res) => {
				if (res.success != 'OK') return
				this.modelUser = res.result.SYUserGetByID[0]
			})
		}else{
			this.modalTitle = "Sửa người dùng"
		}
		$('#modal-user-create-or-update').modal('show')
	}

}

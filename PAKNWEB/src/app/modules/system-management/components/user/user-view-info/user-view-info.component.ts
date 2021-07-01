import { Component, OnInit, AfterViewInit, ElementRef } from '@angular/core'
import { COMMONS } from 'src/app/commons/commons'
import { CONSTANTS, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { ToastrService } from 'ngx-toastr'
import { DomSanitizer } from '@angular/platform-browser'
import { Router } from '@angular/router'
import { AppSettings } from 'src/app/constants/app-setting'

import { UnitService } from '../../../../../services/unit.service'
import { UserService } from '../../../../../services/user.service'
import { PositionService } from '../../../../../services/position.service'
import { RoleService } from '../../../../../services/role.service'
import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { AccountService } from 'src/app/services/account.service'
import { BusinessComponent } from 'src/app/modules/business.component'
import { UserComponent } from 'src/app/modules/system-management/components/user/user.component'
import { from } from 'rxjs'

declare var $: any
@Component({
	selector: 'app-user-view-info',
	templateUrl: './user-view-info.component.html',
	styleUrls: ['./user-view-info.component.css'],
})
export class UserViewInfoComponent implements OnInit, AfterViewInit {
	constructor(
		private unitService: UnitService,
		private element: ElementRef,
		private userService: UserService,
		private positionService: PositionService,
		private toast: ToastrService,
		private roleService: RoleService,
		private sanitizer: DomSanitizer,
		private userStorage: UserInfoStorageService,
		private accountService: AccountService,
		private router: Router
	) {
		this.modalId = element.nativeElement.getAttribute('modalid')
	}
	userAvatar: any = ''
	model: any = {}
	modalId: any
	positionsList: any[] = []
	rolesList: any[] = []
	unitsList: any[] = []

	listStatus: any = [
		{ value: true, text: 'Hiệu lực' },
		{ value: false, text: 'Hết hiệu lực' },
	]
	userId: any
	public parent_BusinessComponent: BusinessComponent
	public parentUser: UserComponent

	ngOnInit() {
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

	ngAfterViewInit() {
		//this.openModal()
		$('#' + this.modalId).on('show.bs.modal', () => {})
	}

	private getInfo(id: number) {
		this.model = {}
		if (id != null && id != undefined && id != 0) {
			this.userService.getById({ id: id }).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.userId = id
					this.model = res.result.SYUserGetByID[0]
					//this.getUserAvatar(this.model.id)
					if (this.model.avatar == '' || this.model.avatar == null) {
						this.userAvatar = ''
					} else {
						this.userAvatar = this.model.avatar
					}

					this.model.positionName = this.unitsList.find((c) => c.id == this.model.unitId).name
					this.model.unitName = this.unitsList.find((c) => c.id == this.model.unitId).name
					if(this.model.roleIds){
						let rolesIds = this.model.roleIds.split(',').map((c) => parseInt(c))
						let rolesNames = this.rolesList.filter((c) => rolesIds.includes(c.id)).map((c) => c.name)
						this.model.rolesNames = rolesNames.join('; ')
					}else{
						this.model.rolesNames = ''
					}
					
					$('#' + this.modalId).modal('show')
				} else {
					this.userId = null
				}
			})
		} else {
			this.accountService.getUserInfo().subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					this.userId = null
					this.model = res.result
					//this.getUserAvatar(this.model.id)
					if (this.model.avatar == '' || this.model.avatar == null) {
						this.userAvatar = ''
					} else {
						this.userAvatar = this.model.avatar
					}

					this.model.positionName = this.unitsList.find((c) => c.id == this.model.unitId).name
					this.model.unitName = this.unitsList.find((c) => c.id == this.model.unitId).name
					if(this.model.roleIds){
						let rolesIds = this.model.roleIds.split(',').map((c) => parseInt(c))
						let rolesNames = this.rolesList.filter((c) => rolesIds.includes(c.id)).map((c) => c.name)
						this.model.rolesNames = rolesNames.join('; ')
					}else{
						this.model.rolesNames =''
					}
					
					$('#' + this.modalId).modal('show')
				}
			})
		}
	}

	public showEditUserInfo() {
		$('#' + this.modalId).modal('hide')

		if (this.userId != null) {
			this.parentUser.modalUserCreateOrUpdate(this.userId)
		}else {
			this.parent_BusinessComponent.openModalEditInfo(this.model.id)
		}
	}

	public openModal(userId: number = 0) {
		let accType = this.userStorage.getTypeObject()
		if (accType != 1) {
			return
		}
		this.getInfo(userId)
		//$('#modal-user-view-info').modal('show')
	}

	public closeModal() {
		$('#' + this.modalId).modal('hide')
		//this.router.navigate(['/quan-tri'])
	}

	private getUserAvatar(id: number) {
		this.userService.getAvatar(id).subscribe((res) => {
			if (res) {
				let objectURL = 'data:image/jpeg;base64,' + res
				this.userAvatar = this.sanitizer.bypassSecurityTrustUrl(objectURL)
			}
		})
	}
}

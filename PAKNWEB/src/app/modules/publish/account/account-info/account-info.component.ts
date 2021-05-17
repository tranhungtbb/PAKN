import { Component, OnInit, ViewChild } from '@angular/core'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { AccountService } from 'src/app/services/account.service'

import { UserInfoStorageService } from 'src/app/commons/user-info-storage.service'
import { RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'
import { AuthenticationService } from 'src/app/services/authentication.service'
import { DataService } from 'src/app/services/sharedata.service'
import { DiadanhService } from 'src/app/services/diadanh.service'
import { PuRecommendationService } from 'src/app/services/pu-recommendation.service'
import { UserInfoObject } from 'src/app/models/UserObject'
import { AccountSideLeftComponent } from '../account-side-left/account-side-left.component'
@Component({
	selector: 'app-account-info',
	templateUrl: './account-info.component.html',
	styleUrls: ['./account-info.component.css'],
})
export class AccountInfoComponent implements OnInit {
	constructor(
		private toast: ToastrService,
		private router: Router,
		private accountService: AccountService,
		private storageService: UserInfoStorageService,
		private authenService: AuthenticationService,
		private sharedataService: DataService,
		private diadanhService: DiadanhService,
		private puRecommendationService: PuRecommendationService
	) {}

	model: UserInfoObject = new UserInfoObject()
	recommendationStatistics: any
	totalRecommentdation: number = 0
	@ViewChild(AccountSideLeftComponent, { static: false }) child_SideLeft: AccountSideLeftComponent

	ngOnInit() {
		var userType = this.storageService.getTypeObject()

		// chuyển hướng trang xem tài khoản doanh nghiệp
		if (userType == 3 && this.router.url.includes('/tai-khoan/thong-tin')) {
			this.router.navigate(['/cong-bo/tai-khoan/thong-tin-doanh-nghiep'])
			return
		}

		this.getUserInfo()

		if (this.router.url.includes('/tai-khoan/thong-tin')) {
			this.viewVisiable = 'info'
		} else if (this.router.url.includes('/tai-khoan/thay-doi-mat-khau')) {
			this.viewVisiable = 'pwd'
		} else if (this.router.url.includes('/tai-khoan/chinh-sua-thong-tin')) {
			this.viewVisiable = 'edit'
		}
		if (this.storageService.getIsHaveToken()) {
			this.puRecommendationService.recommendationStatisticsGetByUserId({}).subscribe((res) => {
				if (res.success == RESPONSE_STATUS.success) {
					if (res.result != null) {
						this.recommendationStatistics = res.result.PURecommendationStatisticsGetByUserId[0]
						for (const iterator in this.recommendationStatistics) {
							this.totalRecommentdation += this.recommendationStatistics[iterator]
						}
					}
				}
				return
			})
		}
	}

	Percent(value: any) {
		var result = Math.ceil((value / this.totalRecommentdation) * 100)
		return result
	}

	viewVisiable: string = 'info' //info, pwd, edit

	getUserInfo() {
		this.accountService.getUserInfo().subscribe((res) => {
			if (res.success != 'OK') {
				this.toast.error(res.message)
				return
			}
			this.model = res.result
			// if (this.model.provinceId == null || this.model.provinceId < 0) {
			// 	this.model.districtId = ''
			// 	this.model.wardsId = ''
			// }s

			this.onChangeNation()

			this.child_SideLeft.model = this.model
		})
	}
	signOut(): void {
		this.authenService.logOut({}).subscribe((success) => {
			if (success.success == RESPONSE_STATUS.success) {
				this.sharedataService.setIsLogin(false)
				this.storageService.setReturnUrl('')
				this.storageService.clearStoreage()
				this.router.navigate(['/dang-nhap'])
				//location.href = "/dang-nhap";
			}
		})
	}

	listNation: any[] = [{ id: 'Việt Nam', name: 'Việt Nam' }]
	listProvince: any[] = []
	listDistrict: any[] = []
	listVillage: any[] = []

	listGender: any[] = [
		{ value: true, text: 'Nam' },
		{ value: false, text: 'Nữ' },
	]

	get getProvinceName() {
		if (this.listProvince == null || this.listProvince.length == 0) {
			return ''
		}
		return this.listProvince.find((c) => c.id == this.model.provinceId).name
	}
	get getDistrictName() {
		if (this.listDistrict == null || this.listDistrict.length == 0) {
			return ''
		}
		return this.listDistrict.find((c) => c.id == this.model.districtId).name
	}
	get getWardsName() {
		if (this.listVillage == null || this.listVillage.length == 0) {
			return ''
		}
		return this.listVillage.find((c) => c.id == this.model.wardsId).name
	}

	onChangeNation(clearable = false) {
		if (clearable) {
			this.listProvince = []
			this.listDistrict = []
			this.listVillage = []

			this.model.provinceId = ''
		}
		if (this.model.nation == 'Việt Nam') {
			this.diadanhService.getAllProvince().subscribe((res) => {
				if (res.success == 'OK') {
					this.listProvince = res.result.CAProvinceGetAll
					//get province
					this.onChangeProvince()
				}
			})
		} else {
		}
	}
	onChangeProvince(clearable = false) {
		if (clearable) {
			this.listDistrict = []
			this.listVillage = []

			this.model.districtId = ''
			this.model.wardsId = ''
		}
		if (this.model.provinceId != null && this.model.provinceId != '') {
			this.diadanhService.getAllDistrict(this.model.provinceId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listDistrict = res.result.CADistrictGetAll
					//get district
					this.onChangeDistrict()
				}
			})
		} else {
		}
	}

	onChangeDistrict(clearable = false) {
		if (clearable) {
			this.listVillage = []
			this.model.wardsId = ''
		}
		if (this.model.districtId != null && this.model.districtId != '') {
			this.diadanhService.getAllVillage(this.model.provinceId, this.model.districtId).subscribe((res) => {
				if (res.success == 'OK') {
					this.listVillage = res.result.CAVillageGetAll
				}
			})
		} else {
		}
	}
}

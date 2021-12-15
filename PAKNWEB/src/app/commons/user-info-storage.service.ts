import { Injectable } from '@angular/core'

@Injectable({
	providedIn: 'root',
})
export class UserInfoStorageService {
	userName: string
	accessToken: string
	permissions: string
	isSaveLogin: boolean
	unitName: string
	role: number

	constructor() { }
	clear = () => {
		var remember = this.getRecommentdationObjectRemember()
		var satisfactionRecommendation = localStorage.getItem('satisfaction')
		localStorage.clear()
		localStorage.setItem('satisfaction', satisfactionRecommendation)
		if (remember && remember != null && remember != 'null') {
			this.setRecommentdationObjectRemember(remember)
		}
	}
	setIsAdmin(key: string): void {
		localStorage.setItem('isAdmin', key)
	}
	setKeyRemember(key: string): void {
		localStorage.setItem('KeyRework', key)
	}
	setReturnUrl(url: string): void {
		localStorage.setItem('ReturnUrl', url)
	}
	setUserId(id: string): void {
		localStorage.setItem('userId', id)
	}

	setUnitLevel(id: number): void {
		localStorage.setItem('unitLevel', String(id))
	}


	setAccessToken(token): void {
		localStorage.setItem('accessToken', token)
	}

	setSaveLogin(isSavelogin: boolean): void {
		localStorage.setItem('isSaveLogin', String(isSavelogin))
	}

	setIsSession(isSession: boolean): void {
		localStorage.setItem('isSession', String(isSession))
	}

	setPermissions(permissions: string): void {
		localStorage.setItem('permissions', permissions)
	}

	setFunctions(functions: string): void {
		localStorage.setItem('functions', functions)
	}

	setPermissionCategories(cate: string): void {
		localStorage.setItem('permissionCategories', cate)
	}

	setIsHaveToken(value: boolean): void {
		localStorage.setItem('IsHaveToken', String(value))
	}

	setIpAddress(value: string): void {
		localStorage.setItem('IpAddress', value)
	}

	setRole(value): void {
		localStorage.setItem('Role', value)
	}

	setFullName(value): void {
		localStorage.setItem('FullName', value)
	}

	setUnitName(value): void {
		localStorage.setItem('unitName', value)
	}

	setUnitId(value): void {
		localStorage.setItem('unitId', value)
	}

	setIsMain(value): void {
		localStorage.setItem('isMain', value)
	}
	setIsUnitMain(value): void {
		localStorage.setItem('isUnitMain', value)
	}

	setIsAprove(value): void {
		localStorage.setItem('isApprove', value)
	}

	setTypeObject(value): void {
		localStorage.setItem('typeObject', value)
	}

	setRecommentdationObjectRemember = (value) => {
		localStorage.setItem('recommentdationObjRemember', value)
	}
	getIsAdmin(): boolean {
		return localStorage.getItem('isAdmin') === 'true'
	}

	getRecommentdationObjectRemember = () => {
		return localStorage.getItem('recommentdationObjRemember')
	}

	getKeyRemember(): string {
		return localStorage.getItem('KeyRework')
	}

	getUnitName(): string {
		return localStorage.getItem('unitName')
	}

	getUnitId(): number {
		return localStorage.getItem('unitId') as any
	}

	getUserId(): number {
		return localStorage.getItem('userId') as any
	}

	getUnitLevel(): number {
		return localStorage.getItem('unitLevel') as any
	}



	getIsMain(): boolean {
		return localStorage.getItem('isMain') === 'true'
	}
	getIsUnitMain(): boolean {
		return localStorage.getItem('isUnitMain') === 'true'
	}
	getIsApprove(): boolean {
		return localStorage.getItem('isApprove') === 'true'
	}

	getReturnUrl(): string {
		return localStorage.getItem('ReturnUrl')
	}

	getAccessToken(): string {
		return localStorage.getItem('accessToken')
	}

	getSaveLogin(): boolean {
		return localStorage.getItem('isSaveLogin') === 'true'
	}
	getIsSession(): boolean {
		return localStorage.getItem('isSession') === 'true'
	}

	getPermissions(): string {
		return localStorage.getItem('permissions')
	}

	getFunctions(): string {
		return localStorage.getItem('functions')
	}

	getPermissionCategories(): string {
		return localStorage.getItem('permissionCategories')
	}

	getIsHaveToken(): boolean {
		return localStorage.getItem('IsHaveToken') === 'true'
	}

	getIpAddress(): string {
		return localStorage.getItem('IpAddress')
	}

	getFullName(): string {
		return localStorage.getItem('FullName')
	}

	getRole() {
		var role = localStorage.getItem('Role')
		if (role == 'null') return null
		else return role
	}

	getTypeObject(): number {
		return localStorage.getItem('typeObject') as any
	}

	// clearStoreage(): void {
	// 	this.setAccessToken('')
	// 	this.setFunctions('')
	// 	this.setIsHaveToken(false)
	// 	this.setIpAddress('')
	// 	this.setPermissionCategories('')
	// 	this.setPermissions('')
	// 	this.setSaveLogin(false)
	// 	this.setUserId('')
	// 	this.setRole('')
	// 	this.setFullName('')
	// 	localStorage.clear()
	// }

	checkRoleChuyenVien() {
		let r = this.getRole()
		if (r == '0') return true
		return false
	}
}

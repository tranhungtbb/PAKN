export class UserObject {
	constructor() {
		this.isActived = true
		this.isDeleted = false
		this.gender = true
		this.type = 1
		this.isSuperAdmin = false
	}
	id: number
	fullName: string
	userName: string
	isActived: boolean
	isDeleted: boolean
	gender: boolean
	type: number
	isSuperAdmin: boolean
	email: string
	phone: string
	unitId: number
	countLock: number
	lockEndOut: number
	avatar: string
	address: string
	positionId: number
}

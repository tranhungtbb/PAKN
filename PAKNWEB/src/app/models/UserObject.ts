export class UserObject {
	constructor() {
		this.isActived = true
		this.isDeleted = false
		this.gender = true
		this.type = 1
		this.isSuperAdmin = false
		this.salt = 'rDZ82OI0dXmfNXETo3JOWNwYQSn46bmgFFinJs8OK/A='
		this.password = '12345'
	}

	id: number
	fullName: string
	userName: string
	salt: string
	password: string
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

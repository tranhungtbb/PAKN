export class smsManagementObject {
	id: number
	title: string
	content: string
	signature: string
	type: string
	status: Number
	constructor() {
		this.id = 0
		this.title = ''
		this.content = null
		this.signature = null
		this.content = ''
		this.status = 1
	}
}

export class smsManagementGetAllOnPageObject {
	id: number
	title: string
	content: string
	unitName: string
	status: number
	type: string
	constructor() {}
}

export class smsManagementMapObject {
	id: number
	category: boolean
	name: boolean
	administrativeUnitName: string
	administrativeUnitId: number
	constructor() {}
}

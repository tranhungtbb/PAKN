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
	title: boolean
	content: boolean
	unitName: string
	status: number
	type: string
	constructor() {}
}

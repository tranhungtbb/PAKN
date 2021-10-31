export class InvitationObject {
	id: number
	title: string
	startDate: Date
	endDate: Date
	content: string
	place: string
	amountWatched: Number
	note: string
	status: Number
	constructor() {
		this.id = 0
		this.title = ''
		this.startDate = null
		this.endDate = null
		this.content = ''
		this.place = ''
		this.note = ''
		this.status = 1
	}
}

export class InvitationUserMapObject {
	userId: number
	sendEmail: boolean
	sendSMS: boolean
	fullName: string
	unitName: string
	positionName: string
	avatar: string
	constructor() {
		this.userId = 0
		this.sendEmail = false
		this.sendSMS = false
		this.avatar = ''
		this.fullName = ''
		this.positionName = ''
	}
}

export class InvitationMapObject {
	id: number
	title: boolean
	type: boolean
	sendEmail: boolean
	sendSMS: boolean
	address: string
	constructor() {
		this.id = 0
		this.sendEmail = false
		this.sendSMS = false
	}
}

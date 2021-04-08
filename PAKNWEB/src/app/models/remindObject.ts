export class RemindObject {
	id: number
	recommendationId: number
	content: string
	constructor() {
		this.content = ''
	}
}

export class Forward {
	id: number
	SenderId: number
	SendOrgId: number
	ReceiveOrgId: number
	DateSend: Date
	IsView: number
}

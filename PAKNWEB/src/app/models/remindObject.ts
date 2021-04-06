export class RemindObject {
	id: number
	PetitionId: number
	Content: string
	constructor() {}
}

export class Forward {
	id: number
	SenderId: number
	SendOrgId: number
	ReceiveOrgId: number
	DateSend: Date
	IsView: number
}

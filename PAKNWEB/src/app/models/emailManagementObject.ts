export class EmailObject {
	constructor() {}
	id: number
	title: string
	content: string
	signature: string
	status: number
	sendDate: Date
	userSend: number
}
export class EmailAttachmentObject {
	id: number
	emailId: number
	fileAttach: string
	name: string
	fileType: number
}
export class EmailIndividualObject {
	id: number
	emailId: number
	individualId: number
	unitName:string
}
export class EmailBusinessObject {
	id: number
	emailId: number
	businessId: number
	unitName:string
}

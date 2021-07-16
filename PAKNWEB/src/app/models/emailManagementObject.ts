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
export class EmailBusinessIndividualObject {
	id : number
	emailId : number
	objectId : number
	objectName : string
	unitName : string
	category : number
	admintrativeUnitId : number
}

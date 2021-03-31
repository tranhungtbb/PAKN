export class RecommendationObject {
	constructor() {
		this.id = 0
		this.code = ''
		this.title = ''
		this.content = ''
		this.field = null
		this.unitId = null
		this.typeObject = 1
		this.sendId = null
		this.name = ''
		this.status = null
		this.sendDate = null
		this.createdBy = null
		this.createdDate = null
		this.updatedBy = null
		this.updatedDate = null
	}
	id: number
	code: string
	title: string
	content: string
	field: number
	unitId: number
	typeObject: number
	sendId: number
	name: string
	status: number
	sendDate: Date
	createdBy: number
	createdDate: Date
	updatedBy: number
	updatedDate: Date
}
export class RecommendationSearchObject {
	constructor() {
		this.code = ''
		this.name = ''
		this.title = ''
		this.content = ''
		this.field = null
		this.unitId = null
		this.status = null
	}
	code: string
	name: string
	title: string
	content: string
	unitId: number
	field: number
	status: number
}
export class RecommendationForwardObject {
	id: number = 0
	recommendationId: number = null
	userSendId: number = null
	unitSendId: number = null
	receiveId: number = null
	unitReceiveId: number = null
	status: number = null
	content: string = ''
	reasonDeny: string = ''
	sendDate: Date = null
	expiredDate: Date = null
	processingDate: Date = null
	isViewed: boolean = false
}
export class RecommendationProcessObject {
	id: number = 0
	recommendationId: number = null
	status: number = null
	reasonDeny: string = ''
	reactionaryWord: boolean = false
}

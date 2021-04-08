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
	step: number = null
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
	step: number = null
	status: number = null
	reasonDeny: string = ''
	reactionaryWord: boolean = false
}
export class RecommendationViewObject {
	id: number
	code: string
	title: string
	content: string
	field: number
	fieldName: string
	unitId: number
	unitName: string
	typeObject: number
	sendId: number
	name: string
	shortName: string
	status: number
	sendDate: Date
	createdBy: number
	createdDate: Date
	updatedBy: number
	updatedDate: Date
	unitActive: number
	userActive: number
	idProcess: number
	stepProcess: number
}
export class RecommendationConclusionObject {
	id: number = 0
	recommendationId: number = null
	userCreatedId: number = null
	unitCreatedId: number = null
	receiverId: number = null
	unitReceiverId: number = null
	content: string = ''
}

export class PuRecommendation {
	id: number = 0
	name: string
	title: string
	content: string
	createDate: Date
	quantityLike: number
	quantityDislike: number
	shortName: string
}

// export class PuRecommendationSearch {
// 	keyseach: string = ''
// 	code: number
// 	pagesize: number
// 	pageindex: number
// }

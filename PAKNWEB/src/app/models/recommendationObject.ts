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
		this.reactionaryWord = false
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
	reactionaryWord: boolean
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
		this.place = ''
		this.unit = ''
		this.field = null
		this.unitId = null
		this.status = null
	}
	code: string
	name: string
	title: string
	content: string
	unitId: string
	field: number
	status: number
	place: string
	unit: string
	groupWord : number
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
	processId: number
	idProcess: number
	stepProcess: number
	receiveName: string
	processUnitName: string
	expriredDate: Date
	processingDate: Date
	approvedName: string
	approvedDate: Date
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

export class RecommendationSuggestObject {
	id: number = 0
	code: string = ''
	title: string = ''
	name: string = ''
	sendDate: Date = null
	contentReply: string = ''
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
export class RecommnendationCommentObject {
	constructor() {
		this.contents = ''
	}
	contents: string
	userId: number
	recommendationId: number
	fullName: string
	isPublish: boolean
}

export class RecommendationSearchStatisticObject {
	constructor() {
		this.code = ''
		this.sendName = ''
		this.title = ''
		this.content = ''
		this.unitId = null
		this.groupWordId = null
	}
	code: string
	sendName: string
	title: string
	content: string
	unitId: number
	groupWordId: number
}

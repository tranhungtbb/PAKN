export class RecommendationObject {
	constructor() {
		this.id = 0
		this.code = ''
		this.title = ''
		this.content = ''
		this.field = null
		this.unitId = null
		this.typeObject = null
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
		this.title = ''
		this.field = null
		this.unitId = null
		this.name = ''
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

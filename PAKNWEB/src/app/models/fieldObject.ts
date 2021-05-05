export class FieldObject {
	constructor() {
		this.id = 0
		this.orderNumber = null
		this.name = ''
		this.code = ''
		this.description = ''
		this.isDeleted = false
		this.isActived = true
	}
	id: number
	orderNumber: number
	name: string
	code: string
	description: string
	isActived: boolean
	isDeleted: boolean
}
export class WordObject {
	constructor() {
		this.id = 0
		this.groupId = null
		this.orderNumber = null
		this.name = ''
		this.description = ''
		this.isDeleted = false
		this.isActived = true
	}
	id: number
	groupId: number
	orderNumber: number
	name: string
	description: string
	isActived: boolean
	isDeleted: boolean
}

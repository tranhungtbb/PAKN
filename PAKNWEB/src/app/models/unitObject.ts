export class UnitObject {
	constructor() {
		this.isDeleted = false
		this.unitLevel = 1
		this.isMain = false
		this.parentId = 0
		this.isActived = true
		this.id = 0
		this.index = 0
		this.listField = null
		this.isPermisstion = false
		this.group = null
	}
	id: number
	name: string
	unitLevel: number
	isActived: boolean
	isDeleted: boolean
	parentId: any
	description: string
	email: string
	phone: string
	address: string
	isMain: boolean
	index: number
	listField: number
	isPermisstion: boolean
	group: number
}

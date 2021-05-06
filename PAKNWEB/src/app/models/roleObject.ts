export class RoleObject {
	id: number
	name: string
	orderNumber: number
	description: string
	isActived: boolean
	isDeleted: boolean
	userCount: Number
	constructor() {
		this.id = 0
		this.isDeleted = false
		this.isActived = true
	}
}

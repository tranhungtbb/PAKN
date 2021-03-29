export class DepartmentObject {
	constructor() {
		this.id = 0
		this.orderNumber = null
		this.name = ''
		this.code = ''
		this.description = ''
		this.isDeleted = false
	}
	id: number
	orderNumber: number
	name: string
	code: string
	description: string
	isActived: boolean
	isDeleted: boolean
	departmentGroupId: number
	phone: string
	email: string
	address: string
	fax: string
}

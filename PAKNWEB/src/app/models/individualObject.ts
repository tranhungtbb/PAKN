export class IndividualObject {
	constructor() {
		this.phone = null
		this.password = null
		this.gender = true
	}
	//thong tin dang nhap
	phone: string
	password: string
	rePassword: string

	//thong tin tai khoan
	fullName: string
	nation: number
	province: number
	district: number
	village: number
	gender: boolean

	email: string
	address: string

	identity: string
	placeIssue: string
	dateIssue: string
}

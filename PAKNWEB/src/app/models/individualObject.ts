export class IndividualObject {
	constructor() {
		this.phone = null
		this.password = null
		this.gender = true

		//
		this.nation = ''
		this.province = ''
		this.district = ''
		this.village = ''
	}
	//thong tin dang nhap
	phone: string
	password: string
	rePassword: string

	//thong tin tai khoan
	fullName: string
	nation: any // quoc tich
	province: any
	district: any
	village: any
	gender: boolean
	odb:string// ngay thang nam sinh

	email: string
	address: string

	identity: string
	placeIssue: string
	dateIssue: string
}

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
	fullName: string // fullName
	nation: any // quoc tich
	province: any
	district: any
	village: any
	gender: boolean
	dob: string // ngay thang nam sinh

	email: string
	address: string

	identity: string
	placeIssue: string
	dateIssue: string
}

export class OrganizationObject {
	constructor() {
		this.phone = null
		this.password = null
		this.Gender = true

		//
		this.Nation = ''
		this.Province = ''
		this.District = ''
		this.Village = ''

		this.OrgProvince = ''
		this.OrgDistrict = ''
		this.OrgVillage = ''

		this.Status = 1
	}
	//----thông tin tài khoản đăng nhập
	phone: string
	password: string
	rePassword: string

	//----thông tin người đại diện
	RepresentativeName: string // tên người đại diện
	Email: string
	Gender: boolean
	DOB: string
	Nation: any
	Province: any //int
	District: any // int
	Village: any // int
	Address: string

	//---thông tin doanh nghiệp
	Business: string // tên tổ chức
	Tax: string //Mã số thuế
	OrgProvince: any //int
	OrgDistrict: any //int
	OrgVillage: any //int
	OrgAddress: string
	OrgPhone: string
	OrgEmail: string

	RegistrationNum: string //Số ĐKKD
	DecisionFoundation: string //Quyết định thành lập
	DateIssue: string //Ngày cấp/thành lập

	Status: number //

	// CreatedBy: string
	// CreatedDate: string
	// UpdatedBy: string
	// UpdatedDate: string
}

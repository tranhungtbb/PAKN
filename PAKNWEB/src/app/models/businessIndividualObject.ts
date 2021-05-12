export class IndividualObject {
	constructor() {
		this.fullName = ''
		this.phone = null
		this.gender = true
		this.email = ''
		//
		this.nation = 'Việt Nam'
		this.provinceId = ''
		this.districtId = ''
		this.wardsId = ''
		this.isActived = true
		this.address = ''
		this.IsDeleted = false
		this.userId = null
	}
	phone: string
	fullName: string // fullName
	nation: any // quoc tich
	provinceId: any
	districtId: any
	wardsId: any
	gender: boolean
	_birthDay: string

	email: string
	address: string

	iDCard: string
	issuedPlace: string
	_dateOfIssue: string

	PermanentPlace: string
	NativePlace: string
	isActived: boolean
	IsDeleted: boolean
	userId: number
}

export class BusinessionObject {
	constructor() {
		this.representativeName = ''
		this.phone = null
		this.gender = true
		this.email = ''
		//
		this.nation = 'Việt Nam'
		this.provinceId = ''
		this.districtId = ''
		this.wardsId = ''
		this.isActived = true
		this.address = ''
		this.IsDeleted = false
		this.userId = null
	}
	phone: string
	representativeName: string // representativeName
	nation: any // quoc tich
	provinceId: any
	districtId: any
	wardsId: any
	gender: boolean
	_birthDay: string

	email: string
	address: string

	iDCard: string
	issuedPlace: string
	_dateOfIssue: string

	PermanentPlace: string
	NativePlace: string
	isActived: boolean
	IsDeleted: boolean
	userId: number
}

export class OrganizationObject {
	constructor() {
		this.phone = null
		this.password = null
		this.RepresentativeGender = true

		//
		this.RepresentativeName = ''
		this.Email = ''
		this.phone = ''

		this.Address = ''
		this.Nation = 'Việt Nam'
		this.ProvinceId = ''
		this.DistrictId = ''
		this.WardsId = ''

		this.Business = ''
		this.BusinessRegistration = ''
		this.DecisionOfEstablishing = ''
		this.OrgProvinceId = ''
		this.OrgDistrictId = ''
		this.OrgWardsId = ''
		this.OrgEmail = ''
		this.OrgAddress = ''
		this.OrgPhone = ''
		this.Tax = ''

		this.Status = 1
		this.isActived = true
		this.IsDeleted = false
		this.userId = null
	}
	//----thông tin tài khoản đăng nhập
	phone: string
	password: string
	rePassword: string

	//----thông tin người đại diện
	RepresentativeName: string // RepresentativeName tên người đại diện
	RepresentativeGender: boolean
	_RepresentativeBirthDay: string
	Email: string
	Nation: any
	ProvinceId: any //int
	DistrictId: any // int
	WardsId: any // int
	Address: string
	PermanentPlace: string // nơi thường trú
	NativePlace: string // nguyên quán

	//---thông tin doanh nghiệp
	Business: string // tên tổ chức
	Tax: string //Mã số thuế
	OrgProvinceId: any //int
	OrgDistrictId: any //int
	OrgWardsId: any //int
	OrgAddress: string
	OrgPhone: string
	OrgEmail: string

	BusinessRegistration: string //Số ĐKKD
	DecisionOfEstablishing: string //Quyết định thành lập
	_DateOfIssue: string //Ngày cấp/thành lập

	Status: number //
	isActived: boolean
	IsDeleted: boolean

	userId: number
}

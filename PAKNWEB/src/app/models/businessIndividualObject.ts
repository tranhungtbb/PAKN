export class IndividualObject {
	constructor() {
		this.fullName = ''
		this.phone = ''
		this.gender = true
		this.email = ''
		//
		this.nation = 'Việt Nam'
		this.provinceId = null
		this.districtId = null
		this.wardsId = null
		this.isActived = true
		this.address = ''
		this.IsDeleted = false
		this.userId = null
		this.status = 1
		this.birthDate = null
		this.dateOfIssue = null
		this.issuedPlace = ""
	}
	phone: string
	fullName: string // fullName
	nation: any // quoc tich
	provinceId: any
	districtId: any
	wardsId: any
	proviceName: any
	wardsName: any
	districtName: any
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
	status: number
	id: number
	imagePath: string
	// birthDay: string
	birthDate: Date
	dateOfIssue: Date
}

export class BusinessionObject {
	constructor() {
		this.representativeName = ''
		this.phone = null
		this.gender = true
		this.email = ''
		//
		this.nation = 'Việt Nam'
		this.provinceId = null
		this.districtId = null
		this.wardsId = null
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
		this.id = 0
		this.RepresentativeGender = true

		//
		this.RepresentativeName = ''
		this.Email = ''
		this.phone = ''

		this.Address = ''
		this.Nation = 'Việt Nam'
		this.ProvinceId = null
		this.DistrictId = null
		this.WardsId = null

		this.Business = ''
		this.BusinessRegistration = ''
		this.DecisionOfEstablishing = ''
		this.OrgProvinceId = null
		this.OrgDistrictId = null
		this.OrgWardsId = null
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
	id: number
}

export class IndividualExportObject {
	constructor() {
		this.fullName = ''
		this.address = ''
		this.phone = ''
		this.email = ''
		this.status = null
	}
	fullName: string
	address: string
	phone: string
	email: string
	status: number
}

export class BusinessExportObject {
	constructor() {
		this.representativeName = ''
		this.address = ''
		this.phone = ''
		this.email = ''
		this.status = null
	}
	representativeName: string
	address: string
	phone: string
	email: string
	status: number
}

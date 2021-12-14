export class IndividualObject {
	constructor() {
		this.phone = null
		this.password = null
		this.rePassword = ''
		this.gender = true
		this.email = null
		this.fullName = ''
		this.issuedPlace = ''
		this.iDCard = ''
		//
		this.nation = 'Việt Nam'
		this.provinceId = null
		this.districtId = null
		this.wardsId = null
		this.isActived = true
		this.address = ''
	}
	//thong tin dang nhap
	phone: string
	password: string
	rePassword: string

	//thong tin tai khoan
	fullName: string // fullName
	nation: any // quoc tich
	provinceId: any
	districtId: any
	wardsId: any
	gender: boolean
	birthDay: Date

	email: string
	address: string

	iDCard: string
	issuedPlace: string
	dateOfIssue: Date

	PermanentPlace: string
	NativePlace: string
	isActived: boolean
}

export class OrganizationObject {
	constructor() {
		this.phone = null
		this.password = null
		this.representativeGender = true

		//
		this.representativeName = ''
		this.address = ''
		this.nation = 'Việt Nam'
		this.provinceId = null
		this.districtId = null
		this.wardsId = null

		this.business = ''
		this.businessRegistration = ''
		this.decisionOfEstablishing = ''
		this.orgProvinceId = null
		this.orgDistrictId = null
		this.orgWardsId = null
		this.orgEmail = ''
		this.orgAddress = ''
		this.orgPhone = ''
		this.tax = ''

		this.status = 1
		this.isActived = true
	}
	//----thông tin tài khoản đăng nhập
	phone: string
	password: string
	rePassword: string

	//----thông tin người đại diện
	representativeName: string // RepresentativeName tên người đại diện
	representativeGender: boolean
	_RepresentativeBirthDay: string
	//RepresentativePhone:string
	email: string
	nation: any
	provinceId: any //int
	districtId: any // int
	wardsId: any // int
	address: string
	permanentPlace: string // nơi thường trú
	nativePlace: string // nguyên quán

	//---thông tin doanh nghiệp
	business: string // tên tổ chức
	tax: string //Mã số thuế
	orgProvinceId: any //int
	orgDistrictId: any //int
	orgWardsId: any //int
	orgAddress: string
	orgPhone: string
	orgEmail: string

	businessRegistration: string //Số ĐKKD
	decisionOfEstablishing: string //Quyết định thành lập
	_DateOfIssue: string //Ngày cấp/thành lập

	status: number //
	isActived: boolean
}

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
		this.userId = ''
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
	userId: string
}

export class BusinessionObject2 {
	constructor() {
		this.phone = null
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
	}
	//----thông tin tài khoản đăng nhập
	phone: string
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
}

export class BusinessIndividualObject {
	constructor() {
		this.id = 0
		this.code = ''
		this.title = ''
		this.content = ''
		this.field = null
		this.unitId = null
		this.typeObject = 1
		this.sendId = null
		this.name = ''
		this.status = null
		this.reactionaryWord = false
		this.sendDate = null
		this.createdBy = null
		this.createdDate = null
		this.updatedBy = null
		this.updatedDate = null

		this.phone = ''
		this.fullName = ''
		this.nation = ''
		this.provinceId = ''
		this.districtId = ''
		this.wardsId = ''
		this.gender = true
		this._birthDay = ''
		this.email = ''
		this.address = ''
		this.iDCard = ''
		this.issuedPlace = ''
		this._dateOfIssue = ''
		this.PermanentPlace = ''
		this.NativePlace = ''
		this.isActived = true
	}
	id: number
	code: string
	title: string
	content: string
	field: number
	unitId: number
	typeObject: number
	sendId: number
	name: string
	status: number
	reactionaryWord: boolean
	sendDate: Date
	createdBy: number
	createdDate: Date
	updatedBy: number
	updatedDate: Date

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
}
export class BusinessIndividualSearchObject {
	constructor() {
		this.code = ''
		this.name = ''
		this.title = ''
		this.content = ''
		this.place = ''
		this.unit = ''
		this.field = null
		this.unitId = null
		this.status = null

		this.phone = ''
		this.fullName = ''
		this.nation = ''
		this.provinceId = ''
		this.districtId = ''
		this.wardsId = ''
		this.gender = true
		this._birthDay = ''
		this.email = ''
		this.address = ''
		this.iDCard = ''
		this.issuedPlace = ''
		this._dateOfIssue = ''
		this.PermanentPlace = ''
		this.NativePlace = ''
		this.isActived = true
	}
	code: string
	name: string
	title: string
	content: string
	unitId: string
	field: number
	status: number
	place: string
	unit: string

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
}

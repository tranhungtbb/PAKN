export class UserObject {
	constructor() {
		this.ma = -1
		this.tenDangNhap = null
		this.hoTen = ''
		this.homThu = ''
		this.gioiTinh = true
		this.loginId = ''
		this.unitId = null
		this.departmentId = null
		this.kichHoat = true
		this.isHaveToken = false
		this.isLeader = false
		this.unitIdCapSo = 0
		this.accountType = 0
		this.maChucVu = null
		this.checkFileChange = false
		this.listPhongBan = []
		this.role = 2
		this.diaChi = ''
		this.dienThoai = ''
	}
	ma: number
	tenDangNhap: string
	loginId: string
	hoTen: string
	homThu: string
	dienThoai: string
	ngaySinh: string
	gioiTinh: boolean
	unitId: number
	departmentId: number
	kichHoat: boolean
	isHaveToken: boolean
	isLeader: boolean
	unitIdCapSo: number
	accountType: number
	donViId: number
	anhDaiDien: string
	phongBanId: number
	xoa: boolean
	maChucVu: boolean
	matKhau: string
	isSuperAdmin: boolean
	checkFileChange: boolean
	listPhongBan: any[]
	role: number
	diaChi: string
}

export class UserObject2 {
	constructor() {
		this.fullName = ''
		this.userName = ''
		this.email = ''
		this.phone = ''
		this.positionId = null
		this.positionName = ''
		this.roleIds = null
		this.isActived = true
		this.isDeleted = false
		this.gender = null
		this.type = 1
		this.isSuperAdmin = false
		this.typeId = 1
		this.isActived = true
		this.address = ''
	}

	id: number
	typeId: number
	fullName: string
	userName: string
	salt: string
	password: string
	isActived: boolean
	isDeleted: boolean
	gender: boolean
	type: number
	isSuperAdmin: boolean
	email: string
	phone: string
	unitId: number
	unitName: string
	countLock: number
	lockEndOut: string
	avatar: string
	address: string
	positionId: number
	positionName: string
	roleIds: string
}

export class UserInfoObject {
	userName: string
	fullName: string
	dateOfBirth: string
	email: string
	phone: string
	nation: any
	provinceId: any
	districtId: any
	wardsId: any
	address: string
	idCard: string
	issuedPlace: string
	issuedDate: string
	gender: boolean
}

export class ChangePwdObject {
	oldPassword: string
	newPassword: string
	rePassword: string
}

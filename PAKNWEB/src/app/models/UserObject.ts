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
		this.roleIds = null
		this.isActived = true
		this.isDeleted = false
		this.gender = true
		this.type = 1
		this.isSuperAdmin = false
		this.typeId = 1
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
	countLock: number
	lockEndOut: string
	avatar: string
	address: string
	positionId: number
	roleIds: string
}

// export class UnitObject {
// 	ma: number
// 	ten: string
// 	code: string
// 	moTa: string
// 	kichHoat: boolean
// 	xoa: boolean
// 	soDienThoai: string
// 	khuVucId: number
// 	nguoiPhuTrachId: number
// 	tinh: string
// 	huyen: string
// 	tenLoaiDonVi: string
// 	diaChi: string
// }
export class UnitObject {
	constructor() {
		this.isActived = true
		this.isDeleted = false
	}
	id: number
	name: string
	unitLevel: number
	isActived: boolean
	isDeleted: boolean
	parentId: number
	description: string
	email: string
	phone: string
	address: string
}

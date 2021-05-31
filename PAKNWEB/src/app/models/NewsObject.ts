export class NewsModel {
	constructor() {
		this.postType = ''
		this.isPublished = false
		this.status = 2 // 0: đã thu hồi | 1: đã công bố | 2: đang soạn thảo
		this.imagePath = ''
		this.newsRelateIds = ''
		this.newsType = null
	}
	id: number
	title: string
	summary: string
	contents: string
	newsType: number
	postType: string
	imagePath: string
	isPublished: boolean
	status: number

	newsRelateIds: string

	//system field
	createdAt: string
	createdBy: number
	updatedAt: string
	updatedBy: number
	isNotification: boolean
}

export class HISNewsModel {
	id: number
	objectId: number
	type: number
	content: string
	status: number
	createBy: number
	createDate: Date
}

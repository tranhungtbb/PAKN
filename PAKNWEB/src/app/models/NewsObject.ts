export class NewsModel {
	constructor() {
		this.postType = ''
		this.isPublished = false
		this.status = 2 // 0: đã thu hồi | 1: đã công bố | 2: đang soạn thảo
		this.imagePath = ''
		this.newsRelateIds = ''
		this.newsType = null
		this.createdAt = ''
		this.createdBy = 0
		this.updatedAt = ''
		this.isNotification = false
		this.publishedBy = 0
		this.publishedDate = ''
		this.viewCount = 0
		this.withdrawDate = ''
		this.withdrawDate = ''
	}
	id: number
	title: string
	summary: string
	contents: string
	newsType: any
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
	publishedBy: number
	publishedDate: any
	viewCount: any
	withdrawBy: any
	withdrawDate: any
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

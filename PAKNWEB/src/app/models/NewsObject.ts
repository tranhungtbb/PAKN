export class NewsModel {
	constructor() {
		this.postType = true
		this.isPublished = false
		this.status = 2 // 0: đã thu hồi | 1: đã công bố | 2: đang soạn thảo
		this.imagePath = ' '
		this.newsRelateIds = ''
		this.newsType = null
	}
	id: number
	title: string
	summary: string
	contents: string
	newsType: number
	postType: boolean
	imagePath: string
	isPublished: boolean
	status: number

	newsRelateIds: string

	//system field
	createdAt: string
	createdBy: number
	updatedAt: string
	updatedBy: number
}

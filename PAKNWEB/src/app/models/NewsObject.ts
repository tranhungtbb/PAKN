export class NewsModel {
	constructor() {
		this.postType = true
	}
	id: number
	title: string
	summary: string
	content: string
	newsType: number
	postType: boolean
	imagePath: string

	newsRelates: any

	//system field
	createdAt: string
	createdBy: number
	updatedAt: string
	updatedBy: number
}

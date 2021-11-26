export class ChatbotObject {
	constructor() {
		this.id = 0
		this.question = ''
		this.answer = ''
		this.categoryId = 0
		this.isDeleted = false
		this.isActived = true
	}
	id: number
	question: string
	answer: string
	categoryId: number
	isActived: boolean
	isDeleted: boolean
}

export class BotRoom {
	name: string
	id: number
}

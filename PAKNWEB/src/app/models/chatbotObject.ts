import * as signalR from '@aspnet/signalr'
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

export class BotMessage {
	content?: string
	timeStamp?: string
	from?: string
	fromId?: string
	subTags?: any
	type?: 'Conversation' | 'All'
}

export class CustomHttpClient extends signalR.DefaultHttpClient {
	public send(request: signalR.HttpRequest): Promise<signalR.HttpResponse> {
		request.headers = { ...request.headers, 'Access-Control-Allow-Origin': '*', "'Access-Control-Allow-Credentials": 'true' }
		return super.send(request)
	}
}

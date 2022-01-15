import * as signalR from '@aspnet/signalr'
export class ChatbotObject {
	constructor() {
		this.id = 0
		this.title = ''
		this.question = ''
		this.typeChat = 1
		// this.answer = ''
		this.categoryId = 0
		this.isDeleted = false
		this.isActived = true
	}
	id: number
	title: string
	question: string
	// answer: string
	typeChat: number
	categoryId: number
	isActived: boolean
	isDeleted: boolean
	lstHashtags: any
}

export class BotRoom {
	name: string
	id: number
	type: number
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

export class SystemtConfig {
	constructor() {
		this.id = 0
		this.title = ''
		this.description = ''
		this.content = ''
	}
	id: number
	title: string
	description: string
	type: number
	content: string
}

export class ConfigSwitchboard {
	constructor() {
		this.link = ''
		this.config = ''
	}
	link: string
	config: string
}

export class ConfigSMS {
	constructor() {
		this.linkwebservice = ''
		this.password = ''
		this.user = ''
		this.code = ''
		this.serviceID = ''
		this.commandCode = ''
		this.contentType = null
	}
	linkwebservice: string = ''
	password: string = ''
	user: string = ''
	code: string = ''
	serviceID: string = ''
	commandCode: string = ''
	contentType: boolean
}

export class ConfigEmail {
	constructor() {
		this.email = ''
		this.password = ''
		this.server = ''
		this.port = ''
	}
	email: string
	password: string
	server: string
	port: string
}

export class GeneralSetting {
	constructor() {
		this.numberOfWarning = ''
		this.fileQuantity = ''
		this.fileSize = ''
	}
	numberOfWarning: string
	fileQuantity: string
	fileSize: string
}


export class ConfigApplication {
	constructor() {
		this.urlIOS = ''
		this.urlAndroid = ''
	}
	urlIOS: string
	urlAndroid: string
}

export class ConfigSync {
	cskhLinkApi: string = ''
	cskhMethod: number = null
	tdLinkApi: string = ''
	tdMethod: number = null
	cttLinkApi: string = ''
	cttMethod: number = null
	httnLinkApi: string = ''
	httnMethod: number = null
	smsLinkApi: string = ''
	smsMethod: number = null
}

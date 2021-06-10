export class IntroduceObjet {
	id: number
	title: string
	summary: string
	descriptionUnit: string
	descriptionFunction: string
	bannerUrl: string

	constructor() {
		this.id = 0
	}
}

export class IntroduceUnit {
	id: number
	title: string
	description: string
	infomation: string
	index: number
	introduceId: number

	constructor() {
		this.id = 0
		this.title = ''
		this.description = ''
		this.infomation = ''
		this.index = 0
		this.introduceId = 0
	}
}

export class IntroduceFunction {
	id: number
	title: string
	content: string
	icon: string
	iconNew: string
	introduceId: number
}

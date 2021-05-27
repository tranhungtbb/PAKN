export class IntroduceObjet {
	Id: number
	Title: string
	Summary: string
	DescriptionUnit: string
	DescriptionFunction: string
	BannerUrl: string

	constructor() {
		this.Id = 0
	}
}

export class IntroduceUnit {
	Id: number
	Title: string
	Description: string
	Infomation: string
	Index: number
	IntroduceId: number

	constructor() {
		this.Id = 0
		this.Title = null
		this.Description = null
		this.Infomation = null
		this.Index = 0
		this.IntroduceId = 0
	}
}

export class IntroduceFunction {
	Id: number
	Title: string
	Content: string
	Icon: string
	IntroduceId: number
}

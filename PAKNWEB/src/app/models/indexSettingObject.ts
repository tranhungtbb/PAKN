export class IndexSettingObjet {
	id: number
	bannerUrl: string
	phone: string
	email: string
	address: string
	description: string
	license: string

	constructor() {
		this.id = 0
	}
}

export class IndexWebsite {
	id: Number
	nameWebsite: string
	urlWebsite: string
	indexSystemId: number

	constructor() {
		this.id = 0
		this.nameWebsite = ''
		this.urlWebsite = ''
		this.indexSystemId = 0
	}
}

export class IndexBanner {
	id: number
	name: string
	fileAttach: any
	fileType: string
	indexSystemId: number
}

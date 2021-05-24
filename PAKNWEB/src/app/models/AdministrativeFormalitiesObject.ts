export class AdministrativeFormalitiesObject {
	constructor() {
		this.unitReceive = null
		this.field = null
		this.typeSend = null
	}
	id: number = 0
	name: string = ''
	code: string = ''
	countryCode: string = ''
	unitReceive: number = null
	field: number = null
	fieldName: string = ''
	rankReceive: string = ''
	typeSend: boolean = null
	fileNum: string = ''
	amountTime: string = ''
	proceed: string = ''
	object: string = ''
	organization: string = ''
	unitReceiveName: string = ''
	organizationDecision: string = ''
	address: string = ''
	organizationAuthor: string = ''
	organizationCombine: string = ''
	result: string = ''
	legalGrounds: string = ''
	request: string = ''
	impactAssessment: string = ''
	note: string = ''
	status: number = 1
	isShow: boolean = false
	administrationId: number
	//system field
	createdDate: string
	createdBy: number
	updatedDate: string
	updatedBy: number
}

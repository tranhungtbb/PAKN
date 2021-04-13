export class AdministrativeFormalitiesObject {
	id: number = 0;
	name: string = '';
	code: string = '';
	countryCode: string = '';
	unitReceive: number = null;
	field: number = null;
	rankReceive: string = '';
	typeSend: Boolean = null;
	fileNum: string = '';
	amountTime: string = '';
	proceed: string = '';
	object: string = '';
	organization: string = '';
	organizationDecision: string = '';
	address: string = '';
	organizationAuthor: string = '';
	organizationCombine: string = '';
	result: string = '';
	legalGrounds: string = '';
	request: string = '';
	impactAssessment: string = '';
	note: string = '';
	status: number = 1;
	isShow: Boolean = false;

	//system field
	createdAt: string
	createdBy: number
	updatedAt: string
	updatedBy: number
}

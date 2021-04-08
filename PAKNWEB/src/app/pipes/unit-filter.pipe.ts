import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
	name: 'unitFilter',
	pure: false,
})
export class UnitFilterPipe implements PipeTransform {
	transform(items: any[], ...args: any[]): any {
		return items.filter((c) => c['unitLevel'] == args[0])
	}
}

@Pipe({
	name: 'Change',
	pure: false,
})
export class ChangePipe implements PipeTransform {
	transform(data: string, data2: string): any {
		return data.replace('\n', '<br>')
	}
}

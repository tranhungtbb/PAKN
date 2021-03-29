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

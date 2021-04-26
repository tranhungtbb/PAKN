import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
	name: 'getLocalUnitName',
	pure: false,
})
export class GetLocalUnitNamePipe implements PipeTransform {
	transform(items: any[], ...args: any[]): any {
		if (items == null || items.length == 0) return ''
		let item = items.find((c) => c['id'] == args[0])
		if (item == null || item == undefined) return ''
		return item.name
	}
}

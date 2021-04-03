import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
	name: 'getCateName',
	pure: false,
})
export class GetCatePipe implements PipeTransform {
	transform(items: any[], ...args: any[]): any {
		let item = items.find((c) => c.id == args[0])
		if (!item) return ''
		return item.name
	}
}

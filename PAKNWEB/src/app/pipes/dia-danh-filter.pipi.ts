import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
	name: 'diaDanhFilter',
	pure: false,
})
export class DiaDanhFilterPipe implements PipeTransform {
	transform(items: any[], ...args: any[]): any {
		return items.filter((c) => c['districtId'] == args[0])
	}
}

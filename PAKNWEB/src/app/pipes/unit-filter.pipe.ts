import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
	name: 'unitFilter',
	pure: false,
})
export class UnitFilterPipe implements PipeTransform {
	transform(items: any[], ...args: any[]): any {
		return items.filter((c) => c['unitLevel'] == args[0] && c.isActived == true)
	}
}

@Pipe({
	name: 'Change',
	pure: false,
})
export class ChangePipe implements PipeTransform {
	transform(data: string, data2: string): any {
		if(data){
			return data.split('\n').join('<br>')
		}
		return
		
	}
}

import { Pipe, PipeTransform } from '@angular/core'
import { ignoreElements } from 'rxjs-compat/operator/ignoreElements'
@Pipe({
	name: 'oembed',
})
export class OembedPipe implements PipeTransform {
	transform(htmlContent: any): any {
		if (!htmlContent) {
			return ''
		}
		const oembed = htmlContent.split('</oembed>')
		let body = ''
		oembed.forEach((item, index) => {
			if (index % 2 == 0) {
				body += oembed[index] + '</oembed>'
			} else {
				body += oembed[index]
			}
			const oembed1 = item.split('url="')[1]
			if (oembed1) {
				const oembed2 = oembed1.split('">')[0]
				if (oembed2) {
					const youtube = oembed2.split('https://www.youtube.com/watch?v=')[1]
					debugger
					if (youtube) {
						body += '<div class="iframe-container">' + '<iframe src="https://youtube.com/embed/' + youtube + '"></iframe></div>'
					}
				}
			}
		})
		return body
	}
}

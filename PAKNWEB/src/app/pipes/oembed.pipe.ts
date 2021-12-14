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
					if (youtube) {
						body += '<div class="iframe-container">' + '<iframe src="https://youtube.com/embed/' + youtube + '"></iframe></div>'
					}
				}
			}
		})
		let div = document.createElement('div')
		div.innerHTML = body
		let scripts = div.getElementsByTagName('oembed')
		var i = scripts.length
		while (i--) {
			scripts[i].parentNode.removeChild(scripts[i])
		}
		return div.innerHTML
	}
}

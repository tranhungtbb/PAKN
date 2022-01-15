import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ServiceInvokerService } from '../commons/service-invoker.service'
import { Observable } from 'rxjs'
import { Meta, Title } from '@angular/platform-browser';

@Injectable({
	providedIn: 'root',
})
export class MetaService {
	constructor(private title: Title, private meta: Meta) { }


	updateTitle(title: string) {
		this.title.setTitle(title);
	}

	updateOgUrl(url: string) {
		this.meta.updateTag({ name: 'og:url', content: url })
	}

	updateDescription(desc: string) {
		this.meta.updateTag({ name: 'description', content: desc })
	}
}

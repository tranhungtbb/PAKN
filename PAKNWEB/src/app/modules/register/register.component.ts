import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'

declare var $: any

@Component({
	selector: 'app-organization',
	templateUrl: './register.component.html',
	styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
	constructor(private router: Router) {}

	currentRouter: any[]

	ngOnInit() {
		//currentRouter.includes('doanh-nghiep')
		this.currentRouter = this.router.url.split('/')
	}
	onChangeTab() {
		this.currentRouter = this.router.url.split('/')
	}
}

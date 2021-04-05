import { Component, OnInit } from '@angular/core'
import { DepartmentService } from '../../services/department.service'
import { Router } from '@angular/router'
import { UserInfoStorageService } from '../../commons/user-info-storage.service'

@Component({
	selector: 'app-appfooter',
	templateUrl: './appfooter.component.html',
	styleUrls: ['./appfooter.component.css'],
})
export class AppfooterComponent implements OnInit {
	public treeNodes: Array<any> = []
	ten: any
	deptId: number
	constructor(private localStorage: UserInfoStorageService, private service: DepartmentService, private _router: Router) {}

	ngOnInit() {
		this.ten = this.localStorage.getUnitName()
	}
}

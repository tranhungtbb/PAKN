import { Component, OnInit, ViewChild } from '@angular/core'
import { ToastrService } from 'ngx-toastr'
import { SystemconfigService } from 'src/app/services/systemconfig.service'
import { DataService } from 'src/app/services/sharedata.service'
import { RESPONSE_STATUS, TYPE_CONFIG, TYPECONFIG } from 'src/app/constants/CONSTANTS'
import { Router } from '@angular/router'
import {SystemtConfig, ConfigSwitchboard, ConfigSMS, ConfigEmail} from 'src/app/models/systemtConfigObject'
declare var $: any
@Component({
	selector: 'app-system-config',
	templateUrl: './system-config.component.html',
	styleUrls: ['./system-config.component.css'],
})
export class SystemConfigComponent implements OnInit {
	constructor(private _service: SystemconfigService, private _toastr: ToastrService, private _shareData: DataService, private router : Router) {
		this.listType = TYPE_CONFIG
	}
	listData : any = [] 
	listType: any = []
	
	@ViewChild('table', { static: false }) table: any
	totalRecords: number = 0
	model : SystemtConfig = new SystemtConfig()
	config : any
	ngOnInit() {
		this.getList()
	}
	ngAfterViewInit() {
		this._shareData.seteventnotificationDropdown()
	}

	getList() {
		this._service.getAllOnPage({}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result.SYConfigGetAllOnPageBase.length > 0) {
					this.listData = response.result.SYConfigGetAllOnPageBase
					this.totalRecords = response.result.TotalCount
				}else{
					this.totalRecords = 0
					this.listData = []
				}
			} else {
				this.totalRecords = 0
				this.listData = []
				this._toastr.error(response.message)
			}
		}),
			(error) => {
				console.log(error)
				alert(error)
			}
	}

	redirectUpdate(id : number , type : number){
		if(type == TYPECONFIG.CONFIG_EMAIL){
			this.router.navigate(['/quan-tri/he-thong/cau-hinh-email', id])
		}else if(type == TYPECONFIG.CONFIG_SMS){
			this.router.navigate(['/quan-tri/he-thong/cau-hinh-sms', id])

		}else if(type == TYPECONFIG.CONFIG_SWITCHBOARD){
			this.router.navigate(['/quan-tri/he-thong/cau-hinh-switchboard', id])

		}else return

	}
	preDetail(id : number , type : number){
		this._service.syConfigGetById({Id : id}).subscribe((response) => {
			if (response.success == RESPONSE_STATUS.success) {
				if (response.result.SYConfigGetByID.length > 0) {
					this.model = {...response.result.SYConfigGetByID[0]}
					this.config = JSON.parse(this.model.content)
					if(this.model.type == TYPECONFIG.CONFIG_EMAIL){
						debugger
						let s = this.config.password.split('').map(element => {
							return '*'
						});
						this.config.password = s.join('')
					}
					$('#modalDetail').modal('show')
				}
			} else {
				this._toastr.error(response.message)
				return
			}
		}),
			(error) => {
				console.error(error)
				alert(error)
				return
			}
	}
}

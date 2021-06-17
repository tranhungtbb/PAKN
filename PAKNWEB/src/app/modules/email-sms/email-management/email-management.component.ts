import { Component, OnInit } from '@angular/core'

import { Router, ActivatedRoute } from '@angular/router'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { TreeviewItem, TreeviewConfig } from 'ngx-treeview'
import { TreeviewI18n } from 'ngx-treeview'
import { ToastrService } from 'ngx-toastr'
import { RESPONSE_STATUS, FILETYPE } from 'src/app/constants/CONSTANTS'
import { STATUS_HIS_SMS } from 'src/app/constants/CONSTANTS'
import { COMMONS } from 'src/app/commons/commons'
import { EmailManagementService } from 'src/app/services/email-management.service'
import { SMSManagementService } from 'src/app/services/sms-management'
import { smsManagementObject, smsManagementMapObject } from 'src/app/models/smsManagementObject'
import { SMSTreeviewI18n } from 'src/app/shared/sms-treeview-i18n'
import { EmailAttachmentObject, EmailBusinessObject, EmailIndividualObject, EmailObject } from 'src/app/models/emailManagementObject'
import { first } from 'rxjs/operators'
import { UploadFileService } from 'src/app/services/uploadfiles.service'
import { data } from 'jquery'

declare var $:any
declare var jquery:any

@Component({
	selector: 'app-email-management',
	templateUrl: './email-management.component.html',
	styleUrls: ['./email-management.component.css'],
})
export class EmailManagementComponent implements OnInit {
	constructor(
		private _toastr: ToastrService,
		private formBuilder: FormBuilder,
		private router: Router,
		private emailService: EmailManagementService,
		private activatedRoute: ActivatedRoute,
		private smsService: SMSManagementService,
		private fileService: UploadFileService
	) {}

	listStatus: any = [
		{ value: 1, text: 'Đang soạn thảo' },
		{ value: 2, text: 'Đã gửi' },
	]

	listCategory: any = [
		{ value: '1', text: 'Cá nhân' },
		{ value: '2', text: 'Doanh nghiệp' },
	]

	listHisStatus: any = [
		{ value: '0', text: 'Khởi tạo' },
		{ value: '1', text: 'Cập nhập' },
		{ value: '2', text: 'Đã gửi' },
	]

	listData: any[] = []
	query = {
		pageIndex: 1,
		pageSize: 20,
		title: '',
		unit: '',
		objectId: '',
		status: '',
		unitName:''
	}
	totalRecords = 0

	ngOnInit() {
		this.getPagedList();
		this.getAdministrativeUnits();
	}
	dataStateChange(){
		this.getPagedList();
	}
	onPageChange(event: any) {
		this.query.pageSize = event.rows
		this.query.pageIndex = event.first / event.rows + 1
		this.getPagedList()
	}
	
	//
	getPagedList(){

		let query = {...this.query}
		if(!query.unit)query.unit='';
		if(!query.objectId)query.objectId=''
		if(!query.status)query.status=''
		if(query.unitName){
			query.unitName = query.unitName.replace('-','').trim()
		}else query.unitName = ''

		this.emailService.getPagedList(query).subscribe(res=>{
			
			this.listData = res.result.Data;
			if(this.listData[0] && this.listData[0]!.rowNumber > 0){
				this.totalRecords = this.listData[0].rowNumber
			}
		});
	}

	emailId = 0
	confirm(id:number) {
		this.emailId = id
		$('#modalConfirm').modal('show')
	}

	onDelete() {
		$('#modalConfirm').modal('hide')
		this.emailService.Delete(this.emailId).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this._toastr.success(COMMONS.DELETE_SUCCESS)
				this.getPagedList()
				
			} else {
				this._toastr.error(COMMONS.DELETE_FAILED)
				this.getPagedList()
			}
		})
	}
	onSend(id: number) {
		$('#modalConfirmChangeStatus').modal('show')
		this.emailId = id
	}

	onUpdateStatusTypeSend() {
		$('#modalConfirmChangeStatus').modal('hide')
		this.emailService.SendEmail(this.emailId).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {

				this.getPagedList();
			} else {

			}
		})
	}
	AdministrativeUnits:any[]=[]
	getAdministrativeUnits() {
		this.smsService.GetListAdmintrative({ id: 37 }).subscribe((res) => {
			if (res.success == RESPONSE_STATUS.success) {
				this.AdministrativeUnits = res.result.CAAdministrativeUnitsGetDropDown
				// this.getSMSModelById()
			} else {
				this.AdministrativeUnits = []
			}
		})
	}

	///history list
	objectId = 0
	hisPagedList:any[] = []
	hisQuery:any ={
		objectId:0,
		content:'',
		createdBy:'',
		createdDate:'',
		status:'',
		pageIndex:1,
		pageSize:20
	}
	hisTotalRecords=0
	
	getHistory(id: number) {
		if (!id) return
		this.objectId = id;
		this.hisQuery.content = ''
		this.hisQuery.createdBy=''
		this.hisQuery.createdDate=''
		this.hisQuery.status=''
		this.hisQuery.pageIndex = 1;
		this.hisQuery.pageSize = 20;
		this.getHisData(id)
		$('#modalHisSMS').modal('show')
	}

	getHisData(id: Number){
		
		let query = {...this.hisQuery}
		query.objectId = id;
		if(!query.status) query.status = ''

		let date:any = document.querySelector('#fieldHisCratedDate')
		if(date)
			query.createdDate = date.value
		this.emailService.getHisPagedList(query).subscribe(res=>{
			this.hisPagedList =  res.result.Data;
			if(this.hisPagedList[0] && this.hisPagedList[0]!.rowNumber > 0){
				this.hisTotalRecords = this.hisPagedList[0].rowNumber
			}
		})
	}
	clearModelHis(){}
	dataStateChange2(){
		this.getHisData(this.objectId)
	}
	onPageChangeHis(event:any){
		this.hisQuery.pageSize = event.rows
		this.hisQuery.pageIndex = event.first / event.rows + 1
		this.getHisData(this.objectId)
	}
	

}

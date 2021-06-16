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
	}
	totalRecords = 0

	ngOnInit() {
		this.getPagedList();
	}
	dataStateChange(){
		this.getPagedList();
	}
	onPageChange(event: any) {
		this.query.pageSize = event.rows
		this.query.pageIndex = event.first / event.rows + 1
		this.getPagedList()
	}
	onSend(id:string){
		
	}
	//
	getPagedList(){

		let query = {...this.query}
		if(!query.unit)query.unit='';
		if(!query.objectId)query.objectId=''
		if(!query.status)query.status=''

		this.emailService.getPagedList(query).subscribe(res=>{
			console.log(res);
			this.listData = res.result.Data;
			if(this.listData[0].rowNumber > 0){
				this.totalRecords = this.listData[0].rowNumber
			}
		});
	}
}

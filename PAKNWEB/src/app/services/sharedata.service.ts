import { Injectable } from '@angular/core'
import { BehaviorSubject } from 'rxjs'

@Injectable({
	providedIn: 'root',
})
export class DataService {
	private messageSource = new BehaviorSubject(false)
	currentMessage = this.messageSource.asObservable()

	private objectReport = new BehaviorSubject({})
	getobjectReport = this.objectReport.asObservable()

	private objectBack = new BehaviorSubject({})
	getobjectBack = this.objectBack.asObservable()

	private notificationDropdown = new BehaviorSubject({})
	getnotificationDropdown = this.notificationDropdown.asObservable()

	private resolutionData = new BehaviorSubject({})
	getresolution = this.resolutionData.asObservable()

	private isLogin = new BehaviorSubject(true)
	getIsLogin = this.isLogin.asObservable()

	private url = new BehaviorSubject(null)
	getUrl = this.url.asObservable()

	private questionId = new BehaviorSubject(null)
	getQuestionId = this.questionId.asObservable()

	notificationObject = {
		notifications: [],
		totalRecords: 0,
	}

	resolutionObject = {
		type: 0,
		id: 0,
	}

	sendReportUrl: string = ''

	changeMessage(data: any) {
		this.messageSource.next(data)
	}

	setobjectsearch(data: any) {
		this.objectReport.next(data)
	}

	setobjectBack(data: any) {
		this.objectBack.next(data)
	}

	seteventnotificationDropdown() {
		var temp: any = this.isLogin
		if (temp.value === true) {
			//setTimeout(() => {
			//  this.notificationService.GetNotification({ PageSize: 10 }).subscribe((response) => {
			//    if (response.status == 1) {
			//      this.notificationObject.notifications = [];
			//      this.notificationObject.notifications = response.listThongBao;
			//      this.notificationObject.totalRecords = response.total;
			//      this.notificationDropdown.next(this.notificationObject);
			//    }
			//  })
			//}, 300)
		}
	}

	setresolutionData(data: any) {
		this.resolutionObject.type = data.type
		this.resolutionObject.id = data.id
		this.resolutionData.next(this.resolutionObject)
	}

	setIsLogin(data) {
		this.isLogin.next(data)
	}

	setUrl(url) {
		if (url != this.url.value) {
			this.seteventnotificationDropdown()
			this.url.next(url)
		}
	}

	setQuestionId(id) {
		this.questionId.next(id)
	}
}

import { Injectable } from '@angular/core'
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpHeaders, HttpErrorResponse } from '@angular/common/http'
import { Observable } from 'rxjs'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
// import { LoadingIndicatorService } from '../commons/loading-indicator.service';
import { finalize, catchError } from 'rxjs/operators'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'
import { environment } from '../../environments/environment'
import { AppSettings } from '../constants/app-setting'
import { Api } from '../constants/api'

@Injectable()
export class CustomHttpInterceptor implements HttpInterceptor {
	env = environment

	constructor(public storeageService: UserInfoStorageService, private _router: Router, private toastr: ToastrService) { }

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		let isShowLoading = request.headers.get('isShowLoading')
		if (
			isShowLoading != 'false' &&
			request.url != AppSettings.API_ADDRESS + Api.NotificationGetList &&
			request.url != AppSettings.API_ADDRESS + Api.NotificationUpdateIsViewed &&
			request.url != AppSettings.API_ADDRESS + Api.UserUpdateQBId &&
			request.url != AppSettings.API_ADDRESS + Api.UserGetAllByIdQb &&
			request.url != AppSettings.API_ADDRESS + Api.UnitGetChildrenDropdownByField &&
			request.url != AppSettings.API_ADDRESS + Api.StatisticsByUnitParentId &&
			request.url != AppSettings.API_ADDRESS + Api.GetMessages &&
			request.url != AppSettings.API_ADDRESS + Api.MRRecommendationCommentGetPageByParent &&
			request.url != AppSettings.API_ADDRESS + Api.MRRecommendationCommentGetOnPage &&
			request.url != AppSettings.API_ADDRESS + Api.UnitGetByGroup &&
			request.url != AppSettings.API_ADDRESS + Api.UnitGetByParentId &&
			request.url != AppSettings.API_ADDRESS + Api.CreateRoom &&
			request.url != AppSettings.API_ADDRESS + Api.UpdateStatusRoom


		) {
			this.env.isContentLoading = true
		}
		if (request.url != 'https://jsonip.com/' && request.url != AppSettings.API_ADDRESS + Api.GetFile) {
			let logAction = request.headers.get('logAction') && request.headers.get('logAction') != 'null' ? request.headers.get('logAction') : ''
			let logObject = request.headers.get('logObject') && request.headers.get('logObject') != 'null' ? request.headers.get('logObject') : ''
			let macAddress = request.headers.get('macAddress') && request.headers.get('macAddress') != 'null' ? request.headers.get('macAddress') : ''
			request = request.clone({
				headers: new HttpHeaders({
					'Access-Control-Allow-Origin': '*',
					Authorization: `Bearer ${this.storeageService.getAccessToken()}`,
					logAction: logAction,
					logObject: logObject,
					ipAddress: localStorage.getItem('IpAddress') && localStorage.getItem('IpAddress') != null ? localStorage.getItem('IpAddress') : '',
					macAddress: macAddress,
				}),
			})
			if (request.body instanceof FormData) {
				// not
			} else {
				request = request.clone({
					setHeaders: {
						'content-type': 'application/json',
					},
				});
			}

		}

		//console.log(request);
		return next.handle(request).pipe(
			catchError((err) => {
				console.log(err)
				if (err instanceof HttpErrorResponse) {
					if (err.status === 401) {
						this.env.isContentLoading = false
						this.storeageService.clear()
						window.location.href = '/cong-bo/trang-chu'
					} else if (err.status === 403) {
						this.env.isContentLoading = false
						this.storeageService.clear()
						this._router.navigate(['/forbidden'])
						this.toastr.error('Bạn không có quyền truy cập trang!')
					}
					if (err.status === 404) {
						this.env.isContentLoading = false
						//this.toastr.error("Không tìm thấy nội dung!");
					}
				}
				this.env.isContentLoading = false
				return Observable.of(err)
			}),
			finalize(() => {
				this.env.isContentLoading = false
			})
		) as any
	}
}

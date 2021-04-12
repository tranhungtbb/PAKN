import { Injectable } from '@angular/core'
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http'
import { Observable } from 'rxjs'
import { finalize, catchError, map } from 'rxjs/operators'
import { Router } from '@angular/router'
import { UserInfoStorageService } from '../commons/user-info-storage.service'
import { LoadingIndicatorService } from '../commons/loading-indicator.service'
import { ToastrService } from 'ngx-toastr'

@Injectable()
export class ResponseInterceptor implements HttpInterceptor {
	constructor(private _router: Router, private storeageService: UserInfoStorageService, private loadingService: LoadingIndicatorService, private toastr: ToastrService) {}

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		//const started = Date.now();
		//let ok: string;
		return next.handle(req).pipe(
			map((event: HttpEvent<any>) => {
				if (event instanceof HttpResponse) {
					return event
				} else {
					return new HttpResponse()
				}
			}),
			catchError((err) => {
				console.log(err)
				if (err instanceof HttpErrorResponse) {
					if (err.status === 401) {
						this.loadingService.display(false)
						this.storeageService.clearStoreage()
						this._router.navigate(['/dang-nhap'])
					} else if (err.status === 403) {
						this.loadingService.display(false)
						this._router.navigate(['/forbidden'])
						this.toastr.error('Bạn không có quyền truy cập trang!')
					}
					if (err.status === 404) {
						this.loadingService.display(false)
						this.toastr.error('Không tìm thấy nội dung!')
					}
				}
				return new Observable<HttpEvent<any>>(err)
			}),
			// Log when response observable either completes or errors
			finalize(() => {
				this.loadingService.display(false)
				//const elapsed = Date.now() - started;
				//const msg = `${req.method} "${req.urlWithParams}"
				//   ${ok} in ${elapsed} ms.`;
				//alert(msg);
			})
		) as any
	}
}

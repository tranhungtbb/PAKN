import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceInvokerService } from '../commons/service-invoker.service';
import { Observable, of } from 'rxjs';
import { AppSettings } from '../constants/app-setting';
import { Api } from '../constants/api';
import { catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      console.error(error); // log to console instead
      return of(result as T);
    };
  }
  constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService) { }

  getCatalogsByUnit(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GETCATALOGBYUNITID);
  }

  majorGetList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.MajorGetList);
  }

  stageGetList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.StageGetList);
  }

  recommendationsGetList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.RecommendationsGetList);
  }

  recommendationsTypeGetList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.RecommendationsTypeGetList);
  }

  resolutionTypeGetList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.ResolutionTypeGetList);
  }

  RecomendationFieldTreeGetList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.RecomendationFieldTreeGetList);
  }

  GetlistcapChaId(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.GetlistcapChaId);
  }

  recommendationsFieldGetList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.RecommendationsFieldGetList);
  }

  complaintLetterGetList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.ComplaintLetterGetList);
  }

  positionGetList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.PositionGetList);
  }

  positionGroupGetList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.PositionGroupGetList);
  }

  nationGetList(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.NationGetList);
  }

  nationGetById(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.NationGetById);
  }

  UpdateStatus(request: any): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.UpdateStatus);
  }

  DeleteCatalog(request): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.DeleteCatalog);
  }

  KhoaHdndGetList(request): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.KhoaHdndGetList);
  }

  CatalogCreate(request): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.CatalogCreate);
  }

  CatalogUpdate(request): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.CatalogUpdate);
  }

  CatalogGetById(request): Observable<any> {
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.CatalogGetById);
  }

  UnitGetList(request): Observable<any> {
    return this.serviceInvoker.post(request, AppSettings.API_ADDRESS + Api.UnitGetList);
  }

  ExportExcel(request): Observable<any> {
    return this.http.get(AppSettings.API_ADDRESS + Api.ExportCatalog, { responseType: 'blob', params: request }).pipe(tap(
      ),
        catchError(this.handleError<Blob>())
      );
  }

  getDoiTuongByNoiDung(request): Observable<any> {
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.GetDoiTuongByNoiDung);
  }
}

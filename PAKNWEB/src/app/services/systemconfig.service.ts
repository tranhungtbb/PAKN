import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceInvokerService } from '../commons/service-invoker.service';
import { Observable } from 'rxjs';
import { AppSettings } from '../constants/app-setting';
import { Api } from '../constants/api';
import { UserInfoStorageService } from '../commons/user-info-storage.service';

@Injectable({
  providedIn: 'root'
})
export class SystemconfigService {

  constructor(private http: HttpClient, private serviceInvoker: ServiceInvokerService, private storageService: UserInfoStorageService) { }

  getSystemConfig(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GET_SYSTEM_CONFIG);
  }

  getSystemConfigById(systemConfigId: number): Observable<any> {
    var Request = {
      SystemConfigId: systemConfigId
    }
    return this.serviceInvoker.post(Request, AppSettings.API_ADDRESS + Api.GET_SYSTEM_CONFIG_BY_ID);
  }

  updateSystemConfig(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.UPDATE_SYSTEM_CONFIG);
  }

  getCatalogValueByDepartmentId(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GETCATALOGBYUNITID);
  }

  getCatalogValueById(type: number, catalogValueId: number): Observable<any> {
    var Request = {
      Type: type,
      CatalogValueId: catalogValueId
    }
    return this.serviceInvoker.post(Request, AppSettings.API_ADDRESS + Api.GET_CATALOG_VALUE_BY_ID);
  }
  getDocumentTypeByUnitId(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GETDOCUMENTTYPEUNITID);
  }

  updateCatalogValue(data: any, type: number): Observable<any> {
    switch (type) {
      case 1:
        var RequestChucVu = {
          Type: type,
          ChucVu: data
        }
        return this.serviceInvoker.post(RequestChucVu, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
      case 2:
        var RequestTrainingProgram = {
          Type: type,
          TrainingProgram: data
        }
        return this.serviceInvoker.post(RequestTrainingProgram, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
      case 3:
        var RequestPaymentMethod = {
          Type: type,
          PaymentMethod: data
        }
        return this.serviceInvoker.post(RequestPaymentMethod, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);

      case 4:
        var RequestSource = {
          Type: type,
          Source: data
        }
        return this.serviceInvoker.post(RequestSource, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);

      case 5:
        var RequestFinancialType = {
          Type: type,
          FinancialType: data
        }
        return this.serviceInvoker.post(RequestFinancialType, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);

      case 6:
        var RequestProfileType = {
          Type: type,
          ProfileType: data
        }
        return this.serviceInvoker.post(RequestProfileType, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
      case 7:
        var RequestOrderType = {
          Type: type,
          OrderType: data
        }
        return this.serviceInvoker.post(RequestOrderType, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
      case 8:
        var RequestEquippingStudent = {
          Type: type,
          EquippingStudent: data
        }
        return this.serviceInvoker.post(RequestEquippingStudent, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
      case 9:
        var RequestProfession = {
          Type: type,
          Profession: data
        }
        return this.serviceInvoker.post(RequestProfession, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
      case 10:
        const uploadData = new FormData();
        if (data) {
          if (data.file) {
            for (var i = 0; i < data.file.length; i++) {
              uploadData.append('file' + i, data.file[i], data.file[i].name);
            }
          }
          uploadData.append('id', data.id.toString());
          uploadData.append('ma', data.ma.toString());
          uploadData.append('ten', data.ten.toString());
          if (data.moTa != null) {
            uploadData.append('moTa', data.moTa);
          }

          if (data.stt != null) {
            uploadData.append('stt', data.stt);
          } else {
            uploadData.append('stt', null);

          }
          uploadData.append('trangThai', data.trangThai.toString());
          // uploadData.append('stt',  data.stt?data.stt.trim():"");
          uploadData.append('loai', data.loai.toString());
          uploadData.append('nam', data.nam);
          uploadData.append('type', type.toString());
          uploadData.append('xoa', data.xoa);
          uploadData.append('userId', this.storageService.getUserId());
          uploadData.append('ipAddress', this.storageService.getIpAddress());
        }
        return this.http.post(AppSettings.API_ADDRESS + Api.UPDATE_FORM_CATALOG, uploadData);
      case 11:
        var RequestDormitory = {
          Type: type,
          Dormitory: data
        }
        return this.serviceInvoker.post(RequestDormitory, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
      case 12:
        var RequestDormitoryBed = {
          Type: type,
          DormitoryBed: data
        }
        return this.serviceInvoker.post(RequestDormitoryBed, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
      case 13:
        var RequestTuCachLuuTru = {
          Type: type,
          TuCachLuuTru: data
        }
        return this.serviceInvoker.post(RequestTuCachLuuTru, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
      case 14:
        var RequestNhomNguoiDung = {
          Type: type,
          NhomNguoiDung: data
        }
        return this.serviceInvoker.post(RequestNhomNguoiDung, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
      case 15:
        var RequestDormitoryCabinet = {
          Type: type,
          DormitoryCabinet: data
        }
        return this.serviceInvoker.post(RequestDormitoryCabinet, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
      case 16:
        var RequestConfigCode = {
          Type: type,
          MaHeThong: data
        }
        return this.serviceInvoker.post(RequestConfigCode, AppSettings.API_ADDRESS + Api.UPDATE_CATALOG_VALUE);
    }
  }

  GetDataCreateDocument(departmentId: number): Observable<any> {
    var Request = {
      DonViId: departmentId
    }
    return this.serviceInvoker.post(Request, AppSettings.API_ADDRESS + Api.GET_DATA_CREATE_DOCUMENT);
  }

  GetDataUpdateDocument(departmentId: number, Id: number): Observable<any> {
    var Request = {
      DonViId: departmentId,
      CatalogValueId: Id
    }
    return this.serviceInvoker.post(Request, AppSettings.API_ADDRESS + Api.GET_DATA_UPDATE_DOCUMENT);
  }
  createCatalogValue(data: any, type: number): Observable<any> {
    switch (type) {
      case 1:
        var RequestChucVu = {
          Type: type,
          ChucVu: data
        }
        return this.serviceInvoker.post(RequestChucVu, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);

      case 2:
        var RequestTrainingProgram = {
          Type: type,
          TrainingProgram: data
        }
        return this.serviceInvoker.post(RequestTrainingProgram, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);

      case 3:
        var RequestPaymentMethod = {
          Type: type,
          PaymentMethod: data
        }
        return this.serviceInvoker.post(RequestPaymentMethod, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);

      case 4:
        var RequestSource = {
          Type: type,
          Source: data
        }
        return this.serviceInvoker.post(RequestSource, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);

      case 5:
        var RequestFinancialType = {
          Type: type,
          FinancialType: data
        }
        return this.serviceInvoker.post(RequestFinancialType, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);

      case 6:
        var RequestProfileType = {
          Type: type,
          ProfileType: data
        }
        return this.serviceInvoker.post(RequestProfileType, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);
      case 7:
        var RequestOrderType = {
          Type: type,
          OrderType: data
        }
        return this.serviceInvoker.post(RequestOrderType, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);
      case 8:
        var RequestEquippingStudent = {
          Type: type,
          EquippingStudent: data
        }
        return this.serviceInvoker.post(RequestEquippingStudent, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);
      case 9:
        var RequestProfession = {
          Type: type,
          Profession: data
        }
        return this.serviceInvoker.post(RequestProfession, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);
      case 10:
        const uploadData = new FormData();
        if (data) {
          if (data.file) {
            for (var i = 0; i < data.file.length; i++) {
              uploadData.append('file' + i, data.file[i], data.file[i].name);
            }
          }
          uploadData.append('id', data.id.toString());
          uploadData.append('ma', data.ma.toString());
          uploadData.append('ten', data.ten.toString());
          if (data.moTa != null) {
            uploadData.append('moTa', data.moTa);
          }
          uploadData.append('trangThai', data.trangThai.toString());
          if (data.stt != null) {
            uploadData.append('stt', data.stt);
          } else {
            uploadData.append('stt', "");

          }
          uploadData.append('loai', data.loai.toString());
          uploadData.append('nam', data.nam);
          uploadData.append('type', type.toString());
          uploadData.append('xoa', data.xoa);
          uploadData.append('userId', this.storageService.getUserId());
          uploadData.append('ipAddress', this.storageService.getIpAddress());
        }
        return this.http.post(AppSettings.API_ADDRESS + Api.CREATE_FORM_CATALOG, uploadData);
      case 11:
        var RequestDormitory = {
          Type: type,
          Dormitory: data
        }
        return this.serviceInvoker.post(RequestDormitory, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);
      case 12:
        var RequestDormitoryBed = {
          Type: type,
          DormitoryBed: data
        }
        return this.serviceInvoker.post(RequestDormitoryBed, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);
      case 13:
        var RequestTuCachLuuTru = {
          Type: type,
          TuCachLuuTru: data
        }
        return this.serviceInvoker.post(RequestTuCachLuuTru, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);
      case 14:
        var RequestNhomNguoiDung = {
          Type: type,
          NhomNguoiDung: data
        }
        return this.serviceInvoker.post(RequestNhomNguoiDung, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);
      case 15:
        var RequestDormitoryCabinet = {
          Type: type,
          DormitoryCabinet: data
        }
        return this.serviceInvoker.post(RequestDormitoryCabinet, AppSettings.API_ADDRESS + Api.CREATE_CATALOG_VALUE);
    }
  }

  deleteCatalogValue(data: any, type: number): Observable<any> {
    switch (type) {
      case 1:
        var RequestChucVu = {
          Type: type,
          ChucVu: data
        }
        return this.serviceInvoker.post(RequestChucVu, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);

      case 2:
        var RequestTrainingProgram = {
          Type: type,
          TrainingProgram: data
        }
        return this.serviceInvoker.post(RequestTrainingProgram, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);

      case 3:
        var RequestPaymentMethod = {
          Type: type,
          PaymentMethod: data
        }
        return this.serviceInvoker.post(RequestPaymentMethod, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);

      case 4:
        var RequestSource = {
          Type: type,
          Source: data
        }
        return this.serviceInvoker.post(RequestSource, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);

      case 5:
        var RequestFinancialType = {
          Type: type,
          FinancialType: data
        }
        return this.serviceInvoker.post(RequestFinancialType, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);

      case 6:
        var RequestProfileType = {
          Type: type,
          ProfileType: data
        }
        return this.serviceInvoker.post(RequestProfileType, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);
      case 7:
        var RequestOrderType = {
          Type: type,
          OrderType: data
        }
        return this.serviceInvoker.post(RequestOrderType, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);
      case 8:
        var RequestEquippingStudent = {
          Type: type,
          EquippingStudent: data
        }
        return this.serviceInvoker.post(RequestEquippingStudent, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);

      case 9:
        var RequestProfession = {
          Type: type,
          Profession: data
        }
        return this.serviceInvoker.post(RequestProfession, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);
      case 10:
        const uploadData = new FormData();
        if (data) {
          if (data.file) {
            for (var i = 0; i < data.file.length; i++) {
              uploadData.append('file' + i, data.file[i], data.file[i].name);
            }
          }
          uploadData.append('id', data.id.toString());
          uploadData.append('ma', data.ma.toString());
          uploadData.append('ten', data.ten.toString());
          if (data.moTa != null) {
            uploadData.append('moTa', data.moTa);
          }
          uploadData.append('trangThai', data.trangThai.toString());
          if (data.stt != null) {
            uploadData.append('stt', data.stt);
          } else {
            uploadData.append('stt', "");

          }
          uploadData.append('loai', data.loai.toString());
          uploadData.append('nam', data.nam);
          uploadData.append('type', type.toString());
          uploadData.append('xoa', data.xoa);
          uploadData.append('userId', this.storageService.getUserId());
          uploadData.append('ipAddress', this.storageService.getIpAddress());
        }
        return this.http.post(AppSettings.API_ADDRESS + Api.DELETE_FORM_CATALOG, uploadData);
      case 11:
        var RequestDormitory = {
          Type: type,
          Dormitory: data
        }
        return this.serviceInvoker.post(RequestDormitory, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);
      case 12:
        var RequestDormitoryBed = {
          Type: type,
          DormitoryBed: data
        }
        return this.serviceInvoker.post(RequestDormitoryBed, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);
      case 13:
        var RequestTuCachLuuTru = {
          Type: type,
          TuCachLuuTru: data
        }
        return this.serviceInvoker.post(RequestTuCachLuuTru, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);
      case 14:
        var RequestNhomNguoiDung = {
          Type: type,
          NhomNguoiDung: data
        }
        return this.serviceInvoker.post(RequestNhomNguoiDung, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);
      case 15:
        var RequestDormitoryCabinet = {
          Type: type,
          DormitoryCabinet: data
        }
        return this.serviceInvoker.post(RequestDormitoryCabinet, AppSettings.API_ADDRESS + Api.DELETE_CATALOG_VALUE);
    }
  }

  getFileSupport(data: any): Observable<any> {
    return this.serviceInvoker.get(data, AppSettings.API_ADDRESS + Api.getFileSupport);
  }

  getDormitoryTree(data: any): Observable<any> {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GET_DORMITORY_TREE);
  }

  getDropdownProfession(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GET_DDL_PROFESSION);

  }

  getDropdownCatalogValue(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GET_DDL_CATALOG_VALUE);

  }

  getUserForCreateByDepartment(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GetUserForCreateByDepartment);

  }

  getNhomNguoiDungNguoiDung(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.GetNhomNguoiDungNguoiDung);
  }

  createNguoiDungNhomNguoiDung(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.CreateNguoiDungNhomNguoiDung);
  }

  DeleteNguoiDungNhomNguoiDung(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.DeleteNguoiDungNhomNguoiDung);
  }

  GetCodeConfigByCode(Code: string, UnitId: number): Observable<any> {
    var request = {
      Code: Code,
      UnitId: UnitId
    };
    return this.serviceInvoker.get(request, AppSettings.API_ADDRESS + Api.GetCodeConfigByCode);
  }

  getLstEmailTemplate(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.EmailTemplateGetList);
  }

  emailTemplateUpdateTrangThai(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.EmailTemplateUpdateTrangThai);
  }

  emailTemplateDelete(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.EmailTemplateDelete);
  }

  emailTemplateGetById(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.EmailTemplateGetById);
  }

  emailTemplateCreate(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.EmailTemplateCreate);
  }

  getLstEmailTemplateByEmailConfiguration(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.EmailTemplateGetListEmailConfiguration);
  }

  cauHinhEmailDropdown(data: any) {
    return this.serviceInvoker.post(data, AppSettings.API_ADDRESS + Api.CauHinhEmailDropdown);
  }
}

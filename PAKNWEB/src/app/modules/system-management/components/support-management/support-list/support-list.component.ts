import { Component, OnInit } from '@angular/core';
import { SystemconfigService } from '../../../../../services/systemconfig.service';
import { UserInfoStorageService } from '../../../../../commons/user-info-storage.service';
import { AppSettings } from '../../../../../constants/app-setting';
import { UploadFileService } from '../../../../../services/uploadfiles.service';
import { ToastrService } from 'ngx-toastr';
import { saveAs as importedSaveAs } from "file-saver";

declare var $: any;

@Component({
  selector: 'app-support-list',
  templateUrl: './support-list.component.html',
  styleUrls: ['./support-list.component.css']
})
export class SupportListComponent implements OnInit {

  constructor(private localStorage: UserInfoStorageService, private _systemConfigService: SystemconfigService, private filesService: UploadFileService,
    private toastr: ToastrService) { }
  urlSupperAdmin: Array<string> = [];
  urlAdmin: string = '';
  urlUser: string = '';
  ngOnInit() {
    var request = {
      UserId: 1
    }
    this._systemConfigService.getFileSupport(request).
      subscribe(data => {
        if (data.listFile) {
          if (data.listFile.length > 0) {
            this.urlSupperAdmin = data.listFile;
            $('#cv').attr('src', AppSettings.API_DOWNLOADFILES + encodeURI(data.listFile[0]));
            $('#ldcb').attr('src', AppSettings.API_DOWNLOADFILES + encodeURI(data.listFile[1]));
            $('#qtht').attr('src', AppSettings.API_DOWNLOADFILES + encodeURI(data.listFile[2]));
            $('#db').attr('src', AppSettings.API_DOWNLOADFILES + encodeURI(data.listFile[3]));

          }
        }
      })
  }

  DownloadFile(index) {
    let fileName: string = '';
    var lstsplit = this.urlSupperAdmin[index].split('/');
    fileName = lstsplit[lstsplit.length - 1];
    var request = {
      filePath: this.urlSupperAdmin[index],
      Name: fileName
    }
    this.filesService.downloadFileSupport(request).subscribe(
      response => {
        if (response != undefined) {
          var blob = new Blob([response], { type: response.type });
          importedSaveAs(blob, fileName);
        }
      }, (error) => {
        this.toastr.error("Không tìm thấy file trên hệ thống");
      });
  }
}

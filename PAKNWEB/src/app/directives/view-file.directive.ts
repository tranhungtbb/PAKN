import { Directive, Output, EventEmitter, HostListener, ElementRef, Renderer2, Input } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ViewFileDialogComponent } from './viewfile-dialog/viewfile-dialog.component';
// import { AppSettings } from '../constants/app-setting';
import { UploadFileService } from '../services/uploadfiles.service';

import { ToastrService } from 'ngx-toastr';

@Directive({
  selector: '[appViewFile]'
})
export class ViewFileDirective {

  @Input() linkfile: string;
  @Input() namefile: string;
  //@Input() DocId: number;
  @Input() type: number;
  @Output() EventAfterView: EventEmitter<any> = new EventEmitter();


  constructor(public dialog: MatDialog, private el: ElementRef, renderer: Renderer2,
    private files: UploadFileService, private toast: ToastrService,
  ) {
  }

  @HostListener('click', ['$event'])
  onClick(e) {
    if (this.linkfile != undefined && (this.type != 1 && this.type != 2)) {
      const dialogRef = this.dialog.open(ViewFileDialogComponent, {
        data: {
          link: encodeURI(this.linkfile),
          name: encodeURI(this.namefile),
          listfile: [],
          path: encodeURI(this.linkfile),
          // type: this.DocType
        }
      });
      //} else if (this.DocId != undefined && this.DocType != undefined) {
      //  var request = {
      //    DocId: this.DocId,
      //    Type: this.DocType
      //  }
      //  this.files.getFilebyDocIdType(request).subscribe((data) => {
      //    if (data.files.length > 0) {
      //      for (var i = 0; i <= data.files.length - 1; i++) {
      //        data.files[i].duongDan = encodeURI(data.files[i].duongDan);
      //      }

      //      const dialogRef = this.dialog.open(ViewFileDialogComponent, {
      //        data: {
      //          link: null,
      //          listfile: data.files,
      //          type: this.DocType
      //        }
      //      });
      //    } else {
      //      this.toast.warning("Bản ghi không có file đính kèm!");
      //    }
      //  })

    } else if (this.linkfile != undefined && (this.type == 1 || this.type == 2)) {
      var request = {
        Linkfile: this.linkfile,
       
      }
      this.files.getEncryptedPath(request).subscribe((result) => {
        if (result.status == 1) {
          console.log(result);

          const dialogRef = this.dialog.open(ViewFileDialogComponent, {
            data: {
              link: encodeURI(result.pathDecrypt),
              name: encodeURI(this.namefile),
              path: encodeURI(this.linkfile),
              listfile: [],
              // type: this.DocType
            }
          });
        } else {
          console.log(result.message)
        }
        
      });
    }
  }
}


import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterImage'
})
export class FilterImagePipe implements PipeTransform {

  transform(files: any[]): any {
    // filter file kiểu image
    return files.filter(x => x.fileType == 4)
  }

}

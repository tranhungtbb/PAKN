import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ArrayUtilsService {
  public convertToSearchDropDownFormat(array: any[]) : any[] {
    for (let item of array) {
      item.value = +item.value;
      item.type = 'Tất cả';
    }
    return array;
  };

  public convertToSearchDropDownFormatUser(array: any[]): any[] {
    for (let item of array) {
      item.value = +item.ma;
      item.type = 'Tất cả';
    }
    return array;
  };

  public convertToSearchDropDownFormatUsers(array: any[]): any[] {
    for (let item of array) {
      item.ma = +item.ma;
      item.type = 'Tất cả';
    }
    return array;
  };
}

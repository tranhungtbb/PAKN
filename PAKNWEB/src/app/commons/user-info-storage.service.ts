import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserInfoStorageService {

  userName: string;
  accessToken: string;
  permissions: string;
  isSaveLogin: boolean;
  unitName: string;
  role: number;

  constructor() { }
  setKeyRemember(key: string): void {
    localStorage.setItem('KeyRework', key);
  }
  setReturnUrl(url: string): void {
    localStorage.setItem('ReturnUrl', url);
  }
  setUserId(id: string): void {
    localStorage.setItem('userId', id);
  }

  setAccessToken(token): void {
    localStorage.setItem('accessToken', token);
  }

  setSaveLogin(isSavelogin: boolean): void {
    localStorage.setItem('isSaveLogin', String(isSavelogin));
  }

  setPermissions(permissions: string): void {
    localStorage.setItem('permissions', permissions);
  }

  setFunctions(functions: string): void {
    localStorage.setItem('functions', functions);
  }

  setPermissionCategories(cate: string): void {
    localStorage.setItem('permissionCategories', cate);
  }

  setIsHaveToken(value: boolean): void {
    localStorage.setItem('IsHaveToken', String(value));
  }

  setIpAddress(value: string): void {
    localStorage.setItem('IpAddress', value);
  }

  setRole(value): void {
    localStorage.setItem('Role', value);
  }

  setFullName(value): void {
    localStorage.setItem('FullName', value);
  }

  setUnitName(value): void {
    localStorage.setItem('unitName', value);
  }
  
  getKeyRemember(): string {
    return localStorage.getItem('KeyRework'); 
  }

  getUnitName(): string {
    return localStorage.getItem('unitName');
  }

  getReturnUrl(): string {
    return localStorage.getItem('ReturnUrl');
  }

  getAccessToken(): string {
    return localStorage.getItem('accessToken');
  }

  getSaveLogin(): boolean {
    return localStorage.getItem('isSaveLogin') === "true";
  }

  getPermissions(): string {
    return localStorage.getItem('permissions');
  }

  getFunctions(): string {
    return localStorage.getItem('functions');
  }

  getPermissionCategories(): string {
    return localStorage.getItem('permissionCategories');
  }

  getIsHaveToken(): boolean {
    return (localStorage.getItem('IsHaveToken')) === "true";
  }

  getIpAddress(): string {
    return (localStorage.getItem('IpAddress'));
  }

  getFullName(): string {
    return (localStorage.getItem('FullName'));
  }
  
  getRole() {
    var role = (localStorage.getItem('Role'));
    if (role == "null")
      return null;
    else
      return role;
  }

  clearStoreage(): void {
    this.setAccessToken('');
    this.setFunctions('');
    this.setIsHaveToken(false);
    this.setIpAddress('');
    this.setPermissionCategories('');
    this.setPermissions('');
    this.setSaveLogin(false);
    this.setUserId('');
    this.setRole('');
    this.setFullName('');
    localStorage.clear();
  }

  checkRoleChuyenVien() {
    let r = this.getRole();
    if (r == "0")
      return true;
    return false;
  }
}

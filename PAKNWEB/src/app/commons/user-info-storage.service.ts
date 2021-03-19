import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserInfoStorageService {

  userName: string;
  accessToken: string;
  permissions: string;
  userId: string;
  isSaveLogin: boolean;
  isSuperAdmin: boolean;
  isTongHop: boolean;
  deparmentId: number;
  unitId: number;
  accountId: number;
  unitName: string;
  role: number;

  constructor() { }

  setAccountId(id: string): void {
    localStorage.setItem('accountId', id);
  }

  setEmail(args): void {
    localStorage.setItem('email', args);
  }

  setReturnUrl(url: string): void {
    localStorage.setItem('ReturnUrl', url);
  }
  setUserId(id: string): void {
    localStorage.setItem('userId', id);
  }

  setKeyRemember(key: string): void {
    localStorage.setItem('KeyRework', key);
  }

  setUnitName(unitName: string): void {
    localStorage.setItem('unitName', unitName);
  }

  setUserName(name: string): void {
    localStorage.setItem('userName', name);
  }

  setAccessToken(token): void {
    localStorage.setItem('accessToken', token);
  }

  setSaveLogin(isSavelogin: boolean): void {
    localStorage.setItem('isSaveLogin', String(isSavelogin));
  }

  setIsSuperAdmin(isSuperAdmin: boolean): void {
    localStorage.setItem('isSuperAdmin', String(isSuperAdmin));
  }

  setDeparmentId(deparmentId: number): void {
    localStorage.setItem('deparmentId', String(deparmentId));
  }

  setUnitId(unitId: number): void {
    localStorage.setItem('unitId', String(unitId));
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
  setYear(value): void {
    localStorage.setItem('Year', value);
  }
  getUserId(): string {
    return localStorage.getItem('userId');
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

  getUserName(): string {
    return localStorage.getItem('userName');
  }

  getAccessToken(): string {
    return localStorage.getItem('accessToken');
  }

  getSaveLogin(): boolean {
    return localStorage.getItem('isSaveLogin') === "true";
  }

  getIsSuperAdmin(): boolean {
    return localStorage.getItem('isSuperAdmin') === "true";
  }

  getDeparmentId(): number {
    return localStorage.getItem('deparmentId') as any;
  }

  getUnitId(): number {
    return (localStorage.getItem('unitId')) as any;
  }

  getAccountId(): number {
    return (localStorage.getItem('accountId')) as any;
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

  getEmail(): string {
    return (localStorage.getItem('email'));
  }
  getYear(): number {
    return (localStorage.getItem('Year')) as any;;
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
    this.setAccountId('');
    this.setFunctions('');
    this.setIsHaveToken(false);
    this.setIpAddress('');
    this.setIsSuperAdmin(false);
    this.setPermissionCategories('');
    this.setPermissions('');
    this.setSaveLogin(false);
    this.setUnitId(0);
    this.setDeparmentId(0);
    this.setUnitName('');
    this.setUserId('');
    this.setUserName('');
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

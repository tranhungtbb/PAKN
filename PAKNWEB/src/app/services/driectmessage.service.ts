// import { Subscription } from 'rxjs';

import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { HubConnection } from '@aspnet/signalr';
import { Store } from '@ngrx/store';
// import * as directMessagesActions from './store/directmessages.action';
// import { OnlineUser } from '../models/online-user';
// import * as signalR from '@aspnet/signalr';
import { UserInfoStorageService } from '../commons/user-info-storage.service';
declare var $: any;

@Injectable()
export class DirectMessagesService {
  private _hubConnection: HubConnection;
  private headers: HttpHeaders;

  private connection: any;  

  constructor(
    private store: Store<any>,
    public storeageService: UserInfoStorageService
  ) {
    this.headers = new HttpHeaders();
    this.headers = this.headers.set('Access-Control-Allow-Origin', '*');
    this.headers = this.headers.set('Access-Control-Allow-Credentials', 'true',);
    this.headers = this.headers.set('Authorization', `Bearer ${this.storeageService.getAccessToken()}`)
    this.headers = this.headers.set('Content-Type', 'application/json');
    this.headers = this.headers.set('Accept', 'application/json');

    this.init();
  }
  sendDirectMessage(message: string, userId: string): string {

    this._hubConnection.invoke('SendDirectMessage', message, userId);
    return message;
  }

  leave(): void {
    this._hubConnection.invoke('Leave');
  }

  join(): void {
    console.log('send join');
    this._hubConnection.invoke('Join');
  }

  private init() { 
    this.initHub();
  }

  private initHub() {

    //const url = 'http://localhost/SV.QLKN.WebApi/usersdm';
    //const urlnew = 'http://localhost:8080/signalr'
    

    //this._hubConnection = new signalR.HubConnectionBuilder()
    //  .withUrl(urlnew)
    //  .configureLogging(signalR.LogLevel.Information)
    //  .build();

    //this._hubConnection.start().catch(err => console.error(err.toString()));

    //this._hubConnection.on('NewOnlineUser', (onlineUser: OnlineUser) => {
    //  console.log('NewOnlineUser received');
    //  console.log(onlineUser);
    //  this.store.dispatch(new directMessagesActions.ReceivedNewOnlineUser(onlineUser));
    //});

    //this._hubConnection.on('OnlineUsers', (onlineUsers: OnlineUser[]) => {
    //  console.log('OnlineUsers received');
    //  console.log(onlineUsers);
    //  this.store.dispatch(new directMessagesActions.ReceivedOnlineUsers(onlineUsers));
    //});

    //this._hubConnection.on('Joined', (onlineUser: OnlineUser) => {
    //  console.log('Joined received');
    //  this.store.dispatch(new directMessagesActions.JoinSent());
    //  console.log(onlineUser);
    //});

    //this._hubConnection.on('SendDM', (message: string, onlineUser: OnlineUser) => {
    //  console.log('SendDM received');
    //  this.store.dispatch(new directMessagesActions.ReceivedDirectMessage(message, onlineUser));
    //});

    //this._hubConnection.on('Send', (message: string, onlineUser: OnlineUser) => {
    //  console.log('SendDM received');
    //  this.store.dispatch(new directMessagesActions.ReceivedDirectMessage(message, onlineUser));
    //});

    //this._hubConnection.on('UserLeft', (name: string) => {
    //  console.log('UserLeft received');
    //  this.store.dispatch(new directMessagesActions.ReceivedUserLeft(name));
    //});
  }
  private proxy: any;
  private proxyName: string = 'message'; 

  private connectionToHub() {
    const urlnew = 'http://localhost:8080/signalr'
    this.connection = $.hubConnection(urlnew);

  }
}

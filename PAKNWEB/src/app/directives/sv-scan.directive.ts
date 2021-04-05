import { HttpClient } from '@angular/common/http';
import { Directive, ElementRef, EventEmitter, HostListener, Input, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subscription } from 'rxjs';
import { launchUri } from '../../assets/lauchUriApp';
import { DirectMessage } from '../models/direct-message';
import { OnlineUser } from '../models/online-user';
import * as directMessagesAction from '../services/store/directmessages.action';
import { DirectMessagesState } from '../services/store/directmessages.state';
// declare var jquery: any;
declare var $: any;


@Directive({
  selector: '[svScan]'
})

export class SvScanDirective {
  @Output() valueChange: EventEmitter<any> = new EventEmitter<any>();
  @Input() MaVanBan: string;
  private element: HTMLElement;
  public async: any;
  onlineUsers: OnlineUser[];
  onlineUser: OnlineUser;
  directMessages: DirectMessage[];
  selectedOnlineUserName = '';
  dmState$: Observable<DirectMessagesState>;
  dmStateSubscription: Subscription;
  isAuthorizedSubscription: Subscription;
  isAuthorized: boolean;
  connected: boolean;
  message = '';
  pathfile: string = '';
  ConnectionId: string = '';
  //
  private proxy: any;
  private proxyName: string = 'Message';
  private connection: any;
  // create the Event Emitter  
  public messageReceived: EventEmitter<any>;
  public connectionEstablished: EventEmitter<Boolean>;
  public connectionExists: Boolean;
  public tryConnect: number = 0;

  constructor(private el: ElementRef, private toat: ToastrService, private httpclient: HttpClient, private store: Store<any>) {
    this.element = el.nativeElement;
    const urlnew = 'http://localhost:1139'
    this.connectionEstablished = new EventEmitter<Boolean>();
    this.messageReceived = new EventEmitter<any>();
    this.connectionExists = false;
    // create hub connection
    $.connection.hub.url = urlnew;
    this.connection = $.hubConnection(urlnew);


    this.connection.reconnecting(function () {
      this.disconnect();
    });

    // create new proxy as name already given in top  
    this.proxy = this.connection.createHubProxy(this.proxyName);
    // register on server events  
    this.registerOnServerEvents();
    // call the connecion start method to start the connection to send and receive events.  
    //this.startConnection();

  }

  @HostListener("click", ["$event"])
  onClick(event) {
    //

    let that: any = this;

    var Uri = "ScanApp://" + this.MaVanBan;
    launchUri(Uri, function () {
      if (!that.connectionExists) {
        setTimeout(() => {
          that.startConnection();
        }, 4000);
      }
    }, function () {
      $("#DownloadScanapp").modal('show');
      that.connectionEstablished.emit(false);
    }, function () {
      that.toat.error("Trình duyệt không hỗ trợ vui lòng sử dụng trình duyệt Chrome hoặc FireFox để được hỗ trợ tốt hơn");
      that.connectionEstablished.emit(false);
    });

  }

  public sendDm(): void {
    this.store.dispatch(new directMessagesAction.SendDirectMessageAction(this.message, this.onlineUser.userName));
  }

  ngOnInit() {
  }

  ngOnDestroy(): void {

    this.disconnect();
  }

  selectChat(onlineuserUserName: string): void {
    this.selectedOnlineUserName = onlineuserUserName
  }

  sendMessage() {
    console.log('send message to:' + this.selectedOnlineUserName + ':' + this.message);
    this.store.dispatch(new directMessagesAction.SendDirectMessageAction(this.message, this.selectedOnlineUserName));
  }

  getUserInfoName(directMessage: DirectMessage) {
    if (directMessage.fromOnlineUser) {
      return directMessage.fromOnlineUser.userName;
    }

    return '';
  }

  disconnect() {
    //this.store.dispatch(new directMessagesAction.Leave());
    this.connection.stop();
    this.connectionExists = false;
  }

  connect() {
    this.store.dispatch(new directMessagesAction.Join());
  }


  public sendTime() {
    // server side hub method using proxy.invoke with method name pass as param  
    this.proxy.invoke('Send');
  }
  // check in the browser console for either signalr connected or not  
  private startConnection(): void {
    this.connection.start({ transport: ['webSockets', 'longPolling'] }).done((data: any) => {
      console.log('Kết nối');
      this.connectionEstablished.emit(true);
      this.connectionExists = true;
    }).fail((error: any) => {
      $("#DownloadScanapp").modal('show');
      this.connectionEstablished.emit(false);
    });
  }
  // Try connection
  private startConnect(): void {
    this.connection.start({ transport: ['webSockets', 'longPolling'] }).done((data: any) => {
      console.log('Kết nối');
      this.connectionEstablished.emit(true);
      this.connectionExists = true;
    }).fail((error: any) => {
      this.connectionEstablished.emit(false);
    });
  }

  private registerOnServerEvents(): void {
    this.proxy.on('addMessage', (data) => {
      this.valueChange.emit(data);
      this.connectionExists = false;
      this.connectionEstablished.emit(false);
      this.disconnect();
    });
  }
}

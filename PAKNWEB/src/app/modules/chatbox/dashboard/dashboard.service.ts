import {EventEmitter, Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  public components = {
    chatsClicked: false, // For displaying OneToOne and Group Chats
    publicChatClicked: false, // For displaying Public Chats
    createGroupClicked: false, // For creating OneToOne and Group Chats
    onChatClick: false, // For displaying messages ( Dialog Component )
    welcomeChat: true, // Display default Welcome Chat screen
    updateDialog: false, // For displaying update dialog
    deleteDialog : false
  };

  componentsEvent: EventEmitter<any> = new EventEmitter();

  constructor() {
  }

  public showComponent(components: Object) {
    if(!this.components.onChatClick && !this.components.updateDialog && !this.components.deleteDialog){
      this.components.welcomeChat = true
    }
    Object.entries(components).forEach(([key, value]) => {
      this.components[key] = value;
    });
    this.componentsEvent.emit(this.components);
  }

}

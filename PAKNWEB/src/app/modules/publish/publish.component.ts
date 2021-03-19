import { Component, OnInit, OnChanges } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-publish',
  templateUrl: './publish.component.html',
  styleUrls: ['./publish.component.css']
})
export class PublishComponent implements OnInit, OnChanges {
  activeUrl: string = "";
  constructor(private _router: Router) { }

  ngOnInit() {
    let splitRouter = this._router.url.split("/");
    if (splitRouter.length > 2) {
      this.activeUrl = splitRouter[2];
    }
  }
  ngOnChanges(){
    let splitRouter = this._router.url.split("/");
    if (splitRouter.length > 2) {
      this.activeUrl = splitRouter[2];
    }else{
      this.activeUrl = ""
    }
  }
  routingMenu(pageRouting: string){
    this.activeUrl = pageRouting;
    this._router.navigate(['../cong-bo/'+pageRouting]);
  }

}

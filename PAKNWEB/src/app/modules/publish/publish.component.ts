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
    
    this.loadScript('assets/dist/js/custom.min.js');
    this.loadScript('assets/dist/js/deznav-init.js');
    this.loadScript('assets/dist/vendor/waypoints/jquery.waypoints.min.js');
    this.loadScript('assets/dist/vendor/jquery.counterup/jquery.counterup.min.js');
    this.loadScript('assets/dist/vendor/apexchart/apexchart.js');
    this.loadScript('assets/dist/vendor/peity/jquery.peity.min.js');
    this.loadScript('assets/dist/js/plugins-init/piety-init.js');
    this.loadScript('assets/dist/js/dashboard/dashboard-1.js');
    this.loadScript('assets/dist/js/owl.carousel.min.js');
    this.loadScript('assets/dist/js/sd-js.js');
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


  public loadScript(url: string) {
    $('script[src="' + url + '"]').remove();
    $('<script>').attr('src', url).appendTo('body');
  }

}

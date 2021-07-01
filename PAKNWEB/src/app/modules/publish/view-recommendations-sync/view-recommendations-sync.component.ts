import { Component, OnInit, ViewChild } from '@angular/core';
import {Router,ActivatedRoute} from '@angular/router'
import {RecommandationSyncService} from 'src/app/services/recommandation-sync.service'
import {ViewRightComponent} from 'src/app/modules/publish/view-right/view-right.component'



@Component({
  selector: 'app-view-recommendations-sync',
  templateUrl: './view-recommendations-sync.component.html',
  styleUrls: ['./view-recommendations-sync.component.css']
})
export class ViewRecommendationsSyncComponent implements OnInit {

  constructor(
    private _RecommandationSyncService: RecommandationSyncService,
    private _router:Router,
    private _activatedRoute:ActivatedRoute
  ) {}
  @ViewChild(ViewRightComponent, { static: true }) viewRightComponent: ViewRightComponent
  type = 0;
  id = 0;
  model:any = {}

  ngOnInit() {
    this._activatedRoute.params.subscribe((params) => {
			this.type = params['type']
      this.id = params['id']
      this.getData(this.type,this.id);
    })
  }

  getData(type:any, id:any){
    this._RecommandationSyncService.getDetail({
      src:type,
      id
    }).subscribe(res=>{
      console.log(res);
      this.model = res.result.Data;
    })
  }

}

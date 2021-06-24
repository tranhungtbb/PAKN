import { Component, OnInit } from '@angular/core';
import {WeatherService} from 'src/app/services/weather.service'
import { Subscription, timer } from 'rxjs';
import { map, share } from 'rxjs/operators';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent implements OnInit {

  constructor(
    private _WeatherService:WeatherService
  ) {}


  subscription: Subscription;
  curentDate = new Date()
  data = {};
  tempCr = {};
  geoName = ''
  
  geoLocation = {}

  ngOnInit() {
    
    this.subscription = timer(0, 1000)
      .pipe(
        map(() => new Date()),
        share()
      ).subscribe(time => {
        this.curentDate = time;
      });

      
      this.doPromise(this);
      
  }

  private doPromise(seft:any){
    var promise = new Promise<any>((resolve, reject) => {
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition((position) => {
          console.log(position);
          if(!position) return;
          let {latitude,longitude} = position.coords;
          seft.getData(latitude,longitude);
        });
      } else {
       return null;
      }
    });
  }

  private cb(res){
    if(!res) return;
    this.data =res;
    let {main} = res;

    this.tempCr = {
      temp: (main.temp - 32) / 1.8,
      feels_like: (main.feels_like - 32) / 1.8,
      min: (main.temp_min - 32) / 1.8,
      max: (main.temp_max - 32) / 1.8
    }

    this.geoName = res.name;
  }

  getData(lat:any, long:any){
    this._WeatherService.getByGeographic(lat,long).subscribe(this.cb)
  }

}



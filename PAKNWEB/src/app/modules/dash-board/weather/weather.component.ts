import { Component, OnInit } from '@angular/core';
import {WeatherService} from 'src/app/services/weather.service'
import { Subscription, timer } from 'rxjs';
import { map, share } from 'rxjs/operators';
import { UrlResolver } from '@angular/compiler';

declare var $:any
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
  data:any = {};
  tempCr:any = {};
  weather:any={iconPath:''}
  geoName = ''
  
  geoLocation = {}
  storeItem:string = null
  ngOnInit() {
    const seft = this

    this.storeItem = localStorage.getItem('localWeather');
    if(this.storeItem){
      let data = JSON.parse(this.storeItem)

      let timeBet = Date.parse(new Date().toString()) - data.time

      if(timeBet > 1800000)
        this.getWeather(this);
      else{
        this.data = data
        let {main} = data;
        this.weather.iconPath = '/assets/dist/icons/weather-icons/openweather/'+this.data.weather[0].icon+'.svg'

        this.tempCr = {
          temp: Math.round(main.temp - 273.15),
          feels_like: Math.round(main.feels_like - 273.15),
          min: Math.round(main.temp_min - 273.15),
          max: Math.round(main.temp_max - 273.15)
        }
      }
        
    }else
      this.getWeather(this);
    
    this.subscription = timer(0, 1000)
      .pipe(
        map(() => new Date()),
        share()
      ).subscribe(time => {
        this.curentDate = time;
      });
      
  }

  reloadData(){
    this.getWeather(this)
  }

  getData(lat:any, long:any, appid:string=null){
    //this._WeatherService.getByGeographic(lat,long).subscribe(this.cb)
    let url = new URL('http://api.openweathermap.org/data/2.5/weather?appid=3203582b0e1e98b17f97b639bcef2350&lat=20.985009&lon=105.783966');
    if(appid)
      url.searchParams.set('appid',appid)
    url.searchParams.set('lat',lat)
    url.searchParams.set('lon',long)

    $.get(url.href,this.cb)

  }

  private getWeather(seft:any){
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition((position) => {
        if(!position) return;
        let {latitude,longitude} = position.coords;
        seft.getData(latitude,longitude);
      });
    } else {
     return null;
    }
  }

  

  private cb(res){
    if(!res) return;
    this.data =res;
    let {main} = res;
    this.tempCr = {
      temp: Math.round(main.temp - 273.15),
      feels_like: Math.round(main.feels_like - 273.15),
      min: Math.round(main.temp_min - 273.15),
      max: Math.round(main.temp_max - 273.15)
    }
    ///
    //this.weather.iconPath = 'http://openweathermap.org/img/wn/'+this.data.weather[0].icon+'.png';
    this.weather = {iconPath:'assets/dist/icons/weather-icons/openweather/'+this.data.weather[0].icon+'.svg'}
    this.data.time = Date.parse(new Date().toString());
    localStorage.setItem('localWeather',JSON.stringify(this.data));
    console.log(this.data)
  }

}



import { Component, OnInit } from '@angular/core';
import {WeatherService} from 'src/app/services/weather.service'
import { Subscription, timer } from 'rxjs';
import { map, share } from 'rxjs/operators';
import { ConstantPool, UrlResolver } from '@angular/compiler';
import { jsonpFactory } from '@angular/http/src/http_module';

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
        this.weather.iconPath = '/assets/dist/icons/weather-icons/openweather/'+this.data.weather[0].icon+'.svg'
        this.parserData(data);
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
    // this._WeatherService.getByGeographic(lat,long).subscribe(this.cb)
    this._WeatherService.getByGeographic$(lat,long).subscribe(res=>{
      if(res){
        let jobject = JSON.parse(res.result.Data);
        this.cb(jobject);
      }
    })
  }
  getDataByQ(){
    this._WeatherService.getCurrent().subscribe(res=>{
      if(res){
        let jobject = JSON.parse(res.result.Data);
        this.cb(jobject);
      }
    })
  }

  private getWeather(seft:any){
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition((position) => {
        if(!position) {
          seft.getDataByQ()
        };
        let {latitude,longitude} = position.coords;
        seft.getData(latitude,longitude);
      },(err)=>{
        seft.getDataByQ()
      });
    } else {
      seft.getDataByQ()
    }
  }


  
  private parserData(res:any){
    this.data =res;
    let {main} = res;
    this.tempCr = {
      temp: Math.round(main.temp - 273.15),
      feels_like: Math.round(main.feels_like - 273.15),
      min: Math.round(main.temp_min - 273.15),
      max: Math.round(main.temp_max - 273.15)
    }
  }

  private cb(res){
    if(!res) return;
    this.parserData(res);
    ///
    //this.weather.iconPath = 'http://openweathermap.org/img/wn/'+this.data.weather[0].icon+'.png';
    this.weather = {iconPath:'assets/dist/icons/weather-icons/openweather/'+this.data.weather[0].icon+'.svg'}
    this.data.time = Date.parse(new Date().toString());
    localStorage.setItem('localWeather',JSON.stringify(this.data));
    console.log(this.data)
  }

}



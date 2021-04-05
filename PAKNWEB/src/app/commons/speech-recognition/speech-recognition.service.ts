import { Injectable, NgZone } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import * as _ from "lodash";

interface IWindow extends Window {
    webkitSpeechRecognition: any;
  SpeechRecognition: any;
  SpeechGrammarList: any;
  webkitSpeechGrammarList: any;
  SpeechRecognitionEvent: any;
  webkitSpeechRecognitionEvent: any;
}

@Injectable()
export class SpeechRecognitionService {
    speechRecognition: any;
  
  constructor(private zone: NgZone) {
 
    }

    record(): Observable<string> {
        return Observable.create(observer => {
            const { webkitSpeechRecognition }: IWindow = <IWindow>window;
            this.speechRecognition = new webkitSpeechRecognition();
            this.speechRecognition.continuous = true;
            //this.speechRecognition.interimResults = false;
            this.speechRecognition.lang = 'vi-VN';
          this.speechRecognition.maxAlternatives = 1;
          
            this.speechRecognition.onresult = speech => {
                let term: string = "";
                if (speech.results) {
                    var result = speech.results[speech.resultIndex];
                    var transcript = result[0].transcript;
                  if (result.isFinal) {
                    if (result[0].confidence < 0.3) {
                    }
                    else {
                      term = _.trim(transcript);
                    }
                  } 
                }
                this.zone.run(() => {
                    observer.next(term);
                });
            };

            this.speechRecognition.onerror = error => {
                observer.error(error);
            };

            this.speechRecognition.onend = () => {
                observer.complete();
            };

            this.speechRecognition.start();
        });
    }

    DestroySpeechObject() {
        if (this.speechRecognition)
            this.speechRecognition.stop();
    }

}

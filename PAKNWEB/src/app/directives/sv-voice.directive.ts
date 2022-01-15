import { Directive, ElementRef, HostListener, Input } from '@angular/core'
// import { Observable } from 'rxjs';
import { SpeechRecognitionService } from '../commons/speech-recognition/speech-recognition.service'
import { ToastrService } from 'ngx-toastr'

// declare var jquery: any;
declare var $: any

@Directive({
	selector: '[svVoice]',
})
export class SvVoiceDirective {
	@Input() target: string
	@Input() index: number
	private element: HTMLElement
	private ison: number = 0

	constructor(private el: ElementRef, private speechRecognitionService: SpeechRecognitionService, private toat: ToastrService) {
		this.element = el.nativeElement
	}

	@HostListener('click', ['$event'])
	onClick(event) {
		let data = $('#' + this.target).val()
		this.ison += 1
		if (this.ison == 1) {
			$(this.element).children()
				.attr("src", "assets/dist/img/app-images/mic-animate.gif");
			// .attr('class', 'bi bi-mic-fill')
			// $(this.element).next().html('Đang nhận diện giọng nói')
			//$(this.element).children().removeClass("fa-microphone").addClass("fa-microphone-slash");
			//this.toat.info("Bắt đầu nhận diện giọng nói");
			this.speechRecognitionService.record().subscribe(
				//Bắt đầu
				(value) => {
					if (this.index != undefined && !this.target.includes(this.index.toString())) {
						this.target = this.target + this.index
					}
					try {
						$('#' + this.target)[0].dispatchEvent(this.createEvent('focus'))
					} catch { }
					if (data) {
						$('#' + this.target).val(data + ' ' + value)
					} else {
						$('#' + this.target).val(value)
					}

					try {
						$('#' + this.target)[0].dispatchEvent(this.createEvent('change'))
						$('#' + this.target)[0].dispatchEvent(this.createEvent('input'))
					} catch { }

					// $("#" + this.target)[0].dispatchEvent(this.createEvent('blur'));
				},
				//Lỗi
				(err) => {
					console.log(err)
				},
				//Hoàn thành
				() => { }
			)
		} else {
			$(this.element).children().attr("src", "assets/dist/img/app-images/mic.gif");
			// .attr('class', 'bi bi-mic-mute-fill')
			this.speechRecognitionService.DestroySpeechObject()
			// $(this.element).next().html('')

			this.ison = 0
			//this.toat.success("Kết thúc nhận diện giọng nói");
		}
	}

	createEvent(name) {
		var event = document.createEvent('Event')
		event.initEvent(name, true, true)
		return event
	}

	//@HostListener('mousedown', ['$event'])
	//onMousedown(event) {
	//  this.speechRecognitionService.record()
	//    .subscribe(
	//      //Bắt đầu
	//      (value) => {
	//        $(this.element).prev().val(value);
	//      },
	//      //Lỗi
	//      (err) => {
	//        console.log(err);

	//      },
	//      //Hoàn thành
	//      () => {
	//        console.log("xong");
	//      });
	//}

	//@HostListener('mouseup')
	//onMouseup() {
	//  this.speechRecognitionService.DestroySpeechObject();
	//}
}

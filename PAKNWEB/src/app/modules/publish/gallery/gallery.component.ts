import { Component, OnInit, ViewChild } from '@angular/core'
import { Router } from '@angular/router'
import { DomSanitizer } from '@angular/platform-browser'
import { Lightbox } from 'ngx-lightbox'

declare var $: any

@Component({
	selector: 'app-gallery',
	templateUrl: './gallery.component.html',
	styleUrls: ['./gallery.component.css'],
})
export class GalleryComponent implements OnInit {
	constructor(private _router: Router, private sanitizer: DomSanitizer, private _lightbox: Lightbox) {}

	_albums: Array<Album> = [
		{
			src: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(117).jpg',
			caption: 'Image 1 caption here',
			thumb: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(117).jpg',
		},
		{
			src: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(98).jpg',
			caption: 'Image 2 caption here',
			thumb: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(98).jpg',
		},
		{
			src: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(131).jpg',
			caption: 'Image 3 caption here',
			thumb: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(131).jpg',
		},
		{
			src: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(123).jpg',
			caption: 'Image 4 caption here',
			thumb: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(123).jpg',
		},

		{
			src: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(118).jpg',
			caption: 'Image 1 caption here',
			thumb: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(118).jpg',
		},
		{
			src: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(128).jpg',
			caption: 'Image 2 caption here',
			thumb: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(128).jpg',
		},
		{
			src: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(132).jpg',
			caption: 'Image 3 caption here',
			thumb: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(132).jpg',
		},
		{
			src: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(115).jpg',
			caption: 'Image 4 caption here',
			thumb: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(115).jpg',
		},
		{
			src: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(133).jpg',
			caption: 'Image 4 caption here',
			thumb: 'https://mdbootstrap.com/img/Photos/Horizontal/Nature/12-col/img%20(133).jpg',
		},
	]

	ngOnInit() {}

	ngAfterViewInit() {}

	open(index: number): void {
		// open lightbox
		this._lightbox.open(this._albums, index)
	}

	close(): void {
		// close lightbox programmatically
		this._lightbox.close()
	}
}
export interface Album {
	src: string
	caption?: string
	thumb: string
}

'use strict'

// The editor creator to use.
import { ClassicEditorBase } from '@ckeditor/ckeditor5-build-classic'
import SimpleUploadAdapter from '@ckeditor/ckeditor5-upload/src/adapters/simpleuploadadapter'
import ImageResize from '@ckeditor/ckeditor5-image/src/imageresize'

export default class ClassicEditor extends ClassicEditorBase {}
ClassicEditor.builtinPlugins = [SimpleUploadAdapter,ImageResize]

ClassicEditor.defaultConfig = {
	toolbar: ['heading', '|', 'bold', 'italic', 'custombutton'],
	language: 'en',
	placeholder: 'Enunciado de la pregunta.',
	language: 'es',
	image: {
		toolbar: [
			'imageStyle:alignLeft', 'imageStyle:alignCenter', 'imageStyle:alignRight',
			'|',
			'imageStyle:full',
			'imageStyle:side',
			'|',
			'imageTextAlternative'
		],
		styles: [
			'full',
			'side',
			'alignLeft', 'alignCenter', 'alignRight'
		],
		resizeOptions: [
			{
				name: 'imageResize:original',
				label: 'Original',
				value: null
			},
			{
				name: 'imageResize:50',
				label: '50%',
				value: '50'
			},
			{
				name: 'imageResize:75',
				label: '75%',
				value: '75'
			}
		],
	},
}

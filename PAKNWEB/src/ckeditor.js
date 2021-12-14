'use strict'

// The editor creator to use.
import { ClassicEditorBase } from '@ckeditor/ckeditor5-build-classic'
import SimpleUploadAdapter from '@ckeditor/ckeditor5-upload/src/adapters/simpleuploadadapter'

export default class ClassicEditor extends ClassicEditorBase {}
ClassicEditor.builtinPlugins = [SimpleUploadAdapter]

ClassicEditor.defaultConfig = {
	toolbar: ['heading', '|', 'bold', 'italic', 'custombutton'],
	language: 'en',
}

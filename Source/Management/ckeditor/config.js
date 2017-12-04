/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.filebrowserImageBrowseUrl = "angularFileManager/Index.aspx?listtype=UserFiles";
    config.enterMode = CKEDITOR.ENTER_BR;    // CKEDITOR.ENTER_P
    config.extraPlugins = 'sourcedialog,codemirror';
    config.toolbarGroups = [
            { name: 'document', groups: ['mode', 'document'] },
            { name: 'editing', groups: ['find', 'selection'] },
            { name: 'clipboard', groups: ['clipboard', 'undo'] },
            '/',
            { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
            { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'] },
            { name: 'links', groups: ['links'] },
            { name: 'insert', groups: ['insert'] },
            '/',
            { name: 'styles', groups: ['styles'] },
            { name: 'colors', groups: ['colors'] },
            { name: 'tools', groups: ['tools'] },
            { name: 'others', groups: ['others'] },
            { name: 'about', groups: ['about'] }
    ];
    config.removeButtons = 'Sourcedialog,Save,Templates,NewPage,Print,Flash,Smiley,SpecialChar,Iframe';
};

/**
 * @license Copyright (c) 2003-2023, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    config.allowedContent = {
        script: true,
        $1: {
            // This will set the default set of elements
            elements: CKEDITOR.dtd,
            attributes: true,
            styles: true,
            classes: true
        }
    };
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	 config.uiColor = '#AADC6E';
	//config.language = 'bg';
	config.skin = 'moono-lisa';
	config.toolbar = 'Full'; // Use the predefined "Full" toolbar
	config.extraPlugins = 'colorbutton,font,style'; // Add extra plugins
};

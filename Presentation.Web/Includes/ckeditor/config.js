/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */


CKEDITOR.plugins.add('wordpagebreak', {
    icons: 'wordpagebreak',
    init: function (editor) {

        var pluginName = 'wordpagebreak';

        editor.addCommand(pluginName, {
            exec: function (editor) {
                //var html = '<br class="wordpagebreak" clear="all" ' +
                //               'style="mso-special-character: line-break; ' +
                //                      'page-break-before: always">';
                html = '<p>...</p>';
                var element = CKEDITOR.dom.element.createFromHtml(html);
                editor.insertElement(element);
            }
        });

        editor.ui.addButton(pluginName, {
            label: 'Salto de P\u00E1gina',
            icon: 'wordpagebreak',
            command: pluginName,
            toolbar: 'insert'
        });
    }
});

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.extraPlugins = 'wordpagebreak';
    config.removePlugins = 'elementspath';
    config.resize_enabled = false;

    config.toolbar =
    [
        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript', 'RemoveFormat'] },
        { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
        { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
        { name: 'clipboard', items: ['PasteText', 'Undo', 'Redo'] },
        { name: 'insert', items: ['SpecialChar'] },
        { name: 'insert', items: ['Image'] }
        //{ name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'] }
        //{ name: 'tools', items: ['Maximize',  'wordpagebreak'] }

       // { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
        //{ name: 'colors', items: ['TextColor', 'BGColor'] },
        //{ name: 'tools', items: ['Maximize', 'ShowBlocks', '-', 'About'] }
    ];

    //config.height = '350px';

    //config.format_p = { element: "p", name: "Normal", styles: { 'color': 'red' } };

    //config.format_tags = 'p';

    //config.font_defaultLabel = 'Arial';
    //config.contentsCss = ["body {font-family:Arial; font-size: 13px;}p{-webkit-margin-before:0;-webkit-margin-after:0;}"];

    //config.contentsCss = ["body {font-family:Arial;} p{-webkit-margin-before:0;-webkit-margin-after:0;}"];

    //config.font_defaultLabel = 'Arial';

    //config.format_p = { element: "p", name: "Normal", styles: { 'color': 'red' } };

    //config.contentsCss = ".cke_editable{font-size: 10px; font-family:Arial;}";
    //config.font_defaultLabel = 'Arial';
    //fontSize_defaultLabel: '10px'
    //config.fontSize_defaultLabel = '20px';

    //config.colorButton_foreStyle = {
    //    element: 'font',
    //    attributes: { 'color': '#(color)' }
    //};



    //config.extraCss += "body{font-family:Tahoma;}";

    //config.fontSize_style =
    //    {
    //        element: 'span',
    //        styles: { 'font-size': '#(10)' },
    //        overrides: [{ element: 'font', attributes: { 'size': null } }]
    //    };
};


//CKEDITOR.replace('PronunciamientoReivindicacion',
//    {
//        contentsCss: ".cke_editable{font-size: 10px; font-family:Arial;}",
//        font_defaultLabel: 'Arial', fontSize_defaultLabel: '10px'
//    });


//var style = new CKEDITOR.style({ element: 'p', attributes: { 'font-size': '8px' } });
//CKEDITOR['applyStyle'](style);

//var styleCKEDITOR = new CKEDITOR.style({ element: 'img', attributes: { 'class': 'foo' } });

//CKEDITOR.on('instanceReady', function (ev) {
//    CKEDITOR['applyStyle'](style);

//});

//CKEDITOR.on('instanceReady', function (ev) {
//    ev.editor.dataProcessor.htmlFilter.addRules({
//        elements: {
//            p: function (e) { e.attributes.style = 'font-size:8px; font-family:Tahoma;'; }
//        }
//    });
//})

//CKEDITOR.config.font_defaultLabel = 'Arial';
//CKEDITOR.config.fontSize_defaultLabel = '5';

//CKEDITOR.config.font_defaultLabel = 'Georgia';
//CKEDITOR.config.fontSize_defaultLabel = '20';











//CKEDITOR.editorConfig = function (config) {
//    // Define changes to default configuration here.
//    // For complete reference see:
//    // http://docs.ckeditor.com/#!/api/CKEDITOR.config

//    config.toolbar =
//    [
//        //{ name: 'document', items: ['Source', '-', 'Save', 'NewPage', 'DocProps', 'Preview', 'Print', '-', 'Templates'] },
//        //{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
//        //{ name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll', '-', 'SpellChecker', 'Scayt'] },
//        { name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'] },
//        '/',
//        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
//        { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl'] },
//        //{ name: 'links', items: ['Link', 'Unlink', 'Anchor'] },f
//        { name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'] },
//        '/',
//        //{ name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
//        //{ name: 'colors', items: ['TextColor', 'BGColor'] },
//        //{ name: 'tools', items: ['Maximize', 'ShowBlocks', '-', 'About'] }
//    ];

//    config.toolbar_Basic =
//    [
//        ['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', '-', 'About']
//    ];

//    //config.toolbar = [
//    //['Bold', 'Italic', 'Underline', 'Specialchar']
//    //];

//    //config.toolbar = [
//    //	{ name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Undo', 'Redo', 'Specialchars'] }
//    //]

//    //// Toolbar groups configuration.
//    //config.toolbarGroups = [
//    //	//{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] }
//    //]

//    //// The toolbar groups arrangement, optimized for a single toolbar row.
//    //config.toolbarGroups = [
//    //	//{ name: 'document',	   groups: [ 'mode', 'document', 'doctools' ] },
//    //	//{ name: 'clipboard',   groups: [ 'clipboard', 'undo' ] },
//    //	//{ name: 'editing',     groups: [ 'find', 'selection', 'spellchecker' ] },
//    //	//{ name: 'forms' },
//    //	{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
//    //	{ name: 'paragraph',   groups: [ 'list', 'indent', 'blocks', 'align', 'bidi' ] },
//    //	//{ name: 'links' },
//    //	{ name: 'insert', groups: ["insert"] },
//    //	//{ name: 'styles', groups: ["styles"] },
//    //	//{ name: 'colors' },
//    //	//{ name: 'tools' },
//    //	//{ name: 'others' }
//    //	//{ name: 'about' }
//    //];

//    //config.extraPlugins = 'symbol';

//    config.height = '350px';
//    //config.removeButtons = '';

//    //config.specialChars = ['&quot;', '&rsquo;', ['&custom;', 'Custom label']];
//    //config.specialChars = config.specialChars.concat(['&quot;', ['&rsquo;', 'Custom label']]);

//    // The default plugins included in the basic setup define some buttons that
//    // are not needed in a basic editor. They are removed here.
//    //config.removeButtons = 'Cut,Copy,Paste,Undo,Redo,Anchor,Underline,Strike,Subscript,Superscript,Link,Unlink';
//    //config.removeButtons = 'Cut,Copy,Paste,Undo,Redo,Anchor,Strike,Link,Unlink';
//    // Dialog windows are also simplified.
//    //config.removeDialogTabs = 'link:advanced';
//};

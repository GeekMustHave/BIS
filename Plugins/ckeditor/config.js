
CKEDITOR.editorConfig = function (config) {
    config.toolbar = [
		{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
		{ name: 'editing', items: ['Scayt'] },
		{ name: 'links', items: ['Link', 'Anchor'] },
		{ name: 'insert', items: ['Table', 'HorizontalRule', 'SpecialChar'] },
		/*{ name: 'tools', items: ['Maximize'] },*/

		{ name: 'basicstyles', items: ['Bold', 'Italic', 'Strike', '-', 'RemoveFormat'] },
		{ name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote'] },
		{ name: 'styles', items: ['Styles', 'Format'] }
	];

    // Define changes to default configuration here. For example:
    config.language = 'en';
    
    CKEDITOR.on('instanceReady', function (ev) {
        var writer = ev.editor.dataProcessor.writer;
        // The character sequence to use for every indentation step.
        writer.indentationChars = '  ';

        var dtd = CKEDITOR.dtd;
        // Elements taken as an example are: block-level elements (div or p), list items (li, dd), and table elements (td, tbody).
        for (var e in CKEDITOR.tools.extend({}, dtd.$block, dtd.$listItem, dtd.$tableContent)) {
            ev.editor.dataProcessor.writer.setRules(e, {
                // Indicates that an element creates indentation on line breaks that it contains.
                indent: false,
                // Inserts a line break before a tag.
                breakBeforeOpen: false,
                // Inserts a line break after a tag.
                breakAfterOpen: false,
                // Inserts a line break before the closing tag.
                breakBeforeClose: false,
                // Inserts a line break after the closing tag.
                breakAfterClose: false
            });
        }

        for (var e in CKEDITOR.tools.extend({}, dtd.$list, dtd.$listItem, dtd.$tableContent)) {
            ev.editor.dataProcessor.writer.setRules(e, {
                indent: true
            });
        }             
    });
};
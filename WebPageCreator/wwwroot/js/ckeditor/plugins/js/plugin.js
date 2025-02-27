CKEDITOR.plugins.add('js', {
    init: function (editor) {
        editor.on('contentDom', function () {
            var iframe = editor.document.getWindow().$;
            var script = iframe.createElement('script');
            script.type = 'text/javascript';
            script.text = editor.getData();
            iframe.getElementsByTagName('head')[0].appendChild(script);
        });
    }
});
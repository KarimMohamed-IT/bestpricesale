CKEDITOR.plugins.add('slider', {
    requires: 'widget,dialog',
    icons: 'slider',
    init: function (editor) {
        // Register the dialog that will be used to edit the slider.
        CKEDITOR.dialog.add('sliderDialog', this.path + 'dialogs/slider.js');

        // Define the widget.
        editor.widgets.add('slider', {
            // Define what elements will be recognized as a slider widget.
            allowedContent: 'div(!slideshow-container){*}[*](*); img[!src]{*}(*); a[!class]{*}(*);',
            requiredContent: 'div(slideshow-container)',
            upcast: function (element) {
                return element.name == 'div' && element.hasClass('slideshow-container');
            },
            init: function () {
                // Initialize widget data from the element.
                var images = [];
                var imgs = this.element.find('img');
                for (var i = 0; i < imgs.count(); i++) {
                    images.push({ src: imgs.getItem(i).getAttribute('src') });
                }
                this.setData('images', images);
            },
            data: function () {
                // Rebuild the widget's HTML based on widget data.
                var images = this.data.images || [];
                var html = '<div class="slideshow-container">';
                for (var i = 0; i < images.length; i++) {
                    html += '<div class="mySlides fade">' +
                        '<div class="numbertext">' + (i + 1) + ' / ' + images.length + '</div>' +
                        '<img src="' + images[i].src + '" style="width:100%">' +
                        '<div class="text">Caption ' + (i + 1) + '</div>' +
                        '</div>';
                }
                html += '<a class="prev" onclick="plusSlides(-1)">❮</a>' +
                    '<a class="next" onclick="plusSlides(1)">❯</a>' +
                    '</div>';
                this.element.setHtml(html);
            },
            dialog: 'sliderDialog'
        });

        // Add a toolbar button to insert a slider.
        editor.ui.addButton('Slider', {
            label: 'Insert Slider',
            command: 'slider',
            toolbar: 'insert'
        });
    }
});

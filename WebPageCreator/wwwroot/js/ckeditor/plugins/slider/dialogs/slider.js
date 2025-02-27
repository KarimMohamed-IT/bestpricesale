CKEDITOR.dialog.add('sliderDialog', function (editor) {
    return {
        title: 'Edit Slider',
        minWidth: 400,
        minHeight: 200,
        contents: [
            {
                id: 'tab-basic',
                label: 'Basic Settings',
                elements: [
                    {
                        type: 'textarea',
                        id: 'images',
                        label: 'Image URLs (one per line)',
                        setup: function (widget) {
                            if (widget.data.images) {
                                var urls = widget.data.images.map(function (img) {
                                    return img.src;
                                }).join('\n');
                                this.setValue(urls);
                            }
                        },
                        commit: function (widget) {
                            var lines = this.getValue().split('\n');
                            var images = [];
                            for (var i = 0; i < lines.length; i++) {
                                if (lines[i].trim()) {
                                    images.push({ src: lines[i].trim() });
                                }
                            }
                            widget.setData('images', images);
                        }
                    }
                ]
            }
        ],
        onOk: function () {
            this.commitContent(this.getParentEditor().widgets.selected[0]);
        }
    };
});

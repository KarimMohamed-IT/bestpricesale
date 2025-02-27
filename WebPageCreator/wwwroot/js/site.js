// Initial load

// Initialize CKEditor with the slider plugin.
const editor = CKEDITOR.replace('editor', {
    allowedContent: true, // Disable filtering.
    extraPlugins: 'slider,colorbutton,font,justify',
    protectedSource: [/<script[\s\S]*?<\/script>/gi],
    extraAllowedContent: { script: true },
    toolbar: [
        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike'] },
        { name: 'paragraph', items: ['NumberedList', 'BulletedList', 'Blockquote'] },
        { name: 'links', items: ['Link', 'Unlink'] },
        { name: 'insert', items: ['Image', 'Table', 'HorizontalRule', 'SpecialChar', 'Slider'] },
        { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
        { name: 'colors', items: ['TextColor', 'BGColor'] },
        { name: 'tools', items: ['Maximize', 'Source'] }
    ],
    height: 500
});

//// Sync content with the IFrame
//editor.on('change', function () {
//    const content = editor.getData();
//    const preview = document.getElementById('livePreview');
//    preview.srcdoc = content;




//    const iframeDocument = editor.document.$; // Access the iframe's document.
//      iframeDocument.head.appendChild(script);
//    const script = iframeDocument.createElement('script');
//    script.src = "../js/JSInject.js"; // Path to your slider JS file.
//    script.type = 'text/javascript';
//    iframeDocument.head.appendChild(script);
//});



// A simple debounce function to limit how often we fire the AJAX call.
//function debounce(func, wait) {
//    let timeout;
//    return function () {
//        const context = this, args = arguments;
//        clearTimeout(timeout);
//        timeout = setTimeout(() => func.apply(context, args), wait);
//    };
//}

function getToken() {
    return $('input[name="__RequestVerificationToken"]').val();
}


async function loadTemplate() {
    const templateName = document.getElementById('templateDropdown').value;
    if (!templateName) return;

    try {
        // Fetch the template content from your Razor Page handler.
        let src = `/PageEditor?handler=LoadTemplate&name=${encodeURIComponent(templateName)}`;
        const response = await fetch(src);
        if (!response.ok) throw new Error('Template not found.');
        const content = await response.text();

        // Set the editor data with the loaded template
        editor.setData(content);

        // Wait a little to ensure the editor updates before injecting scripts
        setTimeout(() => {
            injectScripts();
        }, 100); // Small delay to ensure content is fully loaded before injecting scripts

    } catch (error) {
        console.error(error);
    }
}

editor.on('mode', function () {
    if (editor.mode != 'source') {
        injectScripts();
    }
});

function injectScripts() {
    var content = editor.getData();
    var parser = new DOMParser();
    var doc = parser.parseFromString(content, 'text/html');

    var scripts = doc.querySelectorAll('script'); // Get all script elements
    var cleanedContent = content; // Keep the content as is for now

    const iframeDocument = editor.document.$;
    const iframeHtml = $(iframeDocument).children('html')[0];

    // Remove existing scripts to avoid duplication
    $(iframeHtml).find('script').remove();

    // Inject each script separately
    scripts.forEach(script => {
        var newScript = iframeDocument.createElement('script');
        newScript.type = script.type || 'text/javascript';

        if (script.src) {
            // If script has a src, copy it and set as external script
            newScript.src = script.src;
        } else {
            // If inline script, copy the text content
            newScript.textContent = script.textContent;
        }

        iframeDocument.body.appendChild(newScript);
    });

    // Set the cleaned content without modifying scripts inside it
    CKEDITOR.instances.editor.setData(cleanedContent);
}


// Update textarea on form submission
document.getElementById('pageForm').addEventListener('submit', function (e) {
    CKEDITOR.instances.editor.updateElement();
});

// Full-screen mode
document.getElementById('fullscreenBtn').addEventListener('click', function () {
    CKEDITOR.instances.editor.execCommand('maximize');
});

// Preview button
document.getElementById('previewBtn').addEventListener('click', function () {
    const content = CKEDITOR.instances.editor.getData();
    const previewWindow = window.open();
    previewWindow.document.write(content);
    previewWindow.document.close();
});





editor.on('instanceReady', function () {
    document.getElementById("cke_notifications_area_editor").remove();
});




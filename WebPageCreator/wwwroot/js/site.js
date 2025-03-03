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

function getToken() {
    return $('input[name="__RequestVerificationToken"]').val();
}


function confirmDelete(title) {
    if (confirm("Are you sure you want to delete this page?")) {
        window.location.href = `/Index?handler=DeletePageAsync?slug=${title}`;
    }
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

//editor.on('mode', function () {
//    if (editor.mode != 'source') {
//        injectScripts();
//    }
//});

//function injectScripts() {
//    var content = editor.getData();
//    var parser = new DOMParser();
//    var doc = parser.parseFromString(content, 'text/html');

//    var scripts = doc.querySelectorAll('script'); // Get all script elements
//    var cleanedContent = content; // Keep the content as is for now

//    const iframeDocument = editor.document.$;
//    const iframeHtml = $(iframeDocument).children('html')[0];

//    // Remove existing scripts to avoid duplication
//    $(iframeHtml).find('script').remove();

//    // Inject each script separately
//    scripts.forEach(script => {
//        var newScript = iframeDocument.createElement('script');
//        newScript.type = script.type || 'text/javascript';

//        if (script.src) {
//            // If script has a src, copy it and set as external script
//            newScript.src = script.src;
//        } else {
//            // If inline script, copy the text content
//            newScript.textContent = script.textContent;
//        }

//        iframeDocument.body.appendChild(newScript);
//    });

//    // Set the cleaned content without modifying scripts inside it
//    CKEDITOR.instances.editor.setData(cleanedContent);
//}


//function injectScripts() {
//    const iframeDocument = editor.document.$;
//    const content = editor.getData();

//    // Clear existing scripts
//    const existingScripts = iframeDocument.querySelectorAll('script');
//    existingScripts.forEach(script => script.remove());

//    // Create parser with script preservation
//    const doc = new DOMParser().parseFromString(content, 'text/html');
//    const scripts = doc.querySelectorAll('script');

//    scripts.forEach(script => {
//        const newScript = iframeDocument.createElement('script');
//        newScript.textContent = script.textContent;

//        // Copy attributes
//        Array.from(script.attributes).forEach(attr => {
//            newScript.setAttribute(attr.name, attr.value);
//        });

//        iframeDocument.body.appendChild(newScript);
//    });

//    // Reinitialize dynamic components
//    if (typeof initSliders === 'function') {
//        initSliders();
//    }
//}

async function loadVersion() {
    const versionId = document.getElementById('versionDropdown').value;
    if (!versionId) return;

    try {
        const response = await fetch(`/PageEditor?handler=LoadVersion&versionId=${versionId}`);
        const content = await response.text();
        editor.setData(content);
        injectScripts();
    } catch (error) {
        console.error('Version load failed:', error);
    }
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




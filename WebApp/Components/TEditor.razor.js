export function TEditor_Init(dotNetRef, instanceId, openGallery) {
    tinymce.init({
        selector: '#' + instanceId, //multiple instances
        menu: {
            custom: { title: 'File Gallery', items: 'fileGallery'}
        },
        menubar: 'edit insert view format custom',
        toolbar: 'undo redo | styles | bold italic | alignleft aligncenter alignright alignjustify | outdent indent | code | fullscreen | fileGalleryButton',
        plugins: 'link image table fullscreen code paste',
        contextmenu: 'link image table',
        height: 300,
        paste_block_drop: false, //not work
        paste_data_images: false, // block paste and drag drop
        automatic_uploads: false,
        images_upload_handler: handleEditorUpload, // handle file upload
        images_upload_credentials: true,
        setup: (editor) => {
            if (openGallery) {
                editor.ui.registry.addMenuItem('fileGallery', {
                    text: 'Select File',
                    icon: 'gallery',
                    onAction: () => dotNetRef.invokeMethodAsync(openGallery),
                });

                editor.ui.registry.addButton('fileGalleryButton', {
                    icon: 'gallery',
                    onAction: () => dotNetRef.invokeMethodAsync(openGallery),
                });
            }

            editor.on('init', () => {
                editor.getContainer().style.transition = 'border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out';
                editor.getContainer().style.border = 'none';
            });

            editor.on('focus', () => {
                editor.getContainer().style.boxShadow = '0 0 0 .2rem rgba(0, 123, 255, .25)';
                editor.getContainer().style.borderColor = '#80bdff';
            });

            editor.on('blur', () => {
                editor.getContainer().style.boxShadow = '';
                editor.getContainer().style.borderColor = '';
            });

            editor.on('change', () => {
                // update textarea
                editor.save();
                dotNetRef.invokeMethodAsync('TEditorChanaged', editor.getContent());
            });
        }
    });

    function handleEditorUpload(blobInfo, success, failure, progress) {
        console.log('blobInfo', blobInfo);
        failure("Upload file inside editor doesn't support.");
        //dotNetRef.invokeMethodAsync('UploadHandler', blobInfo.base64(), blobInfo.filename())
        //    .then((data) => {
        //        success(data);
        //    });
    }

    return {
        insertImage: (instanceId, imageUrl) => {
            const editor = tinymce.get(instanceId);
            if (!editor || !imageUrl) return;
            editor.insertContent('<img width="200" height="auto" src="' + imageUrl + '" />');
        },
    };
}

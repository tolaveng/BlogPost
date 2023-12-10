export function TEditor_Init(dotNetRef, instanceId) {
    tinymce.init({
        selector: '#' + instanceId, //multiple instances
        menubar: 'edit insert view format',
        toolbar: 'undo redo | styles | bold italic | alignleft aligncenter alignright alignjustify | outdent indent | code | fullscreen',
        plugins: 'link image table fullscreen code',
        contextmenu: 'link image table',
        height: 300,
        setup: (editor) => {
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
}

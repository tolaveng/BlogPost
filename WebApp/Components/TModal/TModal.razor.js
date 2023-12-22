export function TModal_init(dotNetRef) {
    const dotNet = dotNetRef;

    return {
        show: function (modalRef) {
            const modal = bootstrap.Modal.getOrCreateInstance(modalRef);
            modal.show();
            // handle modal close
            modalRef.addEventListener('hidden.bs.modal', evt => {
                dotNet.invokeMethodAsync('OnModalCloseJs');
            });
        },
        close: (modalRef) => {
            const modal = bootstrap.Modal.getOrCreateInstance(modalRef);
            modal.hide();
            dotNet.invokeMethodAsync('OnModalCloseJs');
        }
    }
}
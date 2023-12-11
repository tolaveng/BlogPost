export const TModal = {
    show: function (modalRef) {
        const modal = bootstrap.Modal.getOrCreateInstance(modalRef);
        modal.show();
    },
    close: (modalRef) => {
        const modal = bootstrap.Modal.getOrCreateInstance(modalRef);
        modal.hide();
    }
}
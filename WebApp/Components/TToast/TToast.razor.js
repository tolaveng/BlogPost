export function TToast_init(containerRef, templateRef) {
    const getToastClass = (type) => {
        switch (type) {
            case 'warn':
                return 'text-bg-warning';
            case 'danger':
            case 'error':
                return 'text-bg-danger';
            case 'success':
                return 'text-bg-success';
            case 'primary':
                return 'text-bg-primary';
            default:
                return 'text-bg-info';
        }
    };
    // create a global can invoke from window object
    window.TToast = {
        containerRef: containerRef,
        templateRef: templateRef,
        show: (title, message, type) => {
            //const node = templateRef.cloneNode(true);
            let innerHtml = templateRef.innerHTML;
            innerHtml = innerHtml.replace("{title}", title);
            innerHtml = innerHtml.replace("{message}", message);

            // create a new toast
            const template = document.createElement('template');
            template.innerHTML = innerHtml;

            const node = template.content.children[0];
            node.id = "TToast_alert_" + new Date().getTime();
            containerRef.appendChild(node);

            // change color scheme
            const headerNode = containerRef.querySelector('.toast-header');
            const closeBtn = containerRef.querySelector('.btn-close');
            if (headerNode) {
                headerNode.classList.add(getToastClass(type));
                closeBtn.classList.add('btn-close-white');
            }

            // using bootstrap to show toast
            node.addEventListener('hidden.bs.toast', () => {
                console.log('toast close');
                node.remove();
            })
            const toastBootstrap = bootstrap.Toast.getOrCreateInstance(node);
            toastBootstrap.show();
        }
    };
}

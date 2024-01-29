const headerMapByIdentifier = new Map();

function scrollEventListener(e) {
    const headers = document.getElementsByTagName('tnt-header');

    for (const head of headers) {
        if (head && head.classList) {
            if (window.scrollY > 0) {
                if (!head.classList.contains('tnt-elevation-2')) {
                    head.classList.add('tnt-elevation-2');
                }
            }
            else {
                head.classList.remove('tnt-elevation-2');
            }
        }
    }
}

export function onLoad(element = null, dotNetObjectRef = null) {
    window.addEventListener('scroll', scrollEventListener);

    if (!customElements.get('tnt-layout')) {
        customElements.define('tnt-layout', class extends HTMLElement { });
    }

    if (!customElements.get('tnt-side-nav')) {
        customElements.define('tnt-side-nav', class extends HTMLElement { });
    }

    if (!customElements.get('tnt-header')) {
        customElements.define('tnt-header', class extends HTMLElement { });
    }

    if (!customElements.get('tnt-body')) {
        customElements.define('tnt-body', class extends HTMLElement { });
    }

    if (!customElements.get('tnt-footer')) {
        customElements.define('tnt-footer', class extends HTMLElement { });
    }
}

export function onUpdate(element = null, dotNetObjectRef = null) {
}

export function onDispose(element = null, dotNetObjectRef = null) {
    window.removeEventListener('scroll', scrollEventListener);
}
const pageScriptInfoBySrc = new Map();

function registerPageScriptElement(src) {
    if (!src) {
        throw new Error('Must provide a non-empty value for the "src" attribute.');
    }

    let pageScriptInfo = pageScriptInfoBySrc.get(src);

    if (pageScriptInfo) {
        pageScriptInfo.referenceCount++;
    } else {
        pageScriptInfo = { referenceCount: 1, module: null };
        pageScriptInfoBySrc.set(src, pageScriptInfo);
        initializePageScriptModule(src, pageScriptInfo);
    }
}

function unregisterPageScriptElement(src) {
    if (!src) {
        return;
    }

    const pageScriptInfo = pageScriptInfoBySrc.get(src);
    if (!pageScriptInfo) {
        return;
    }

    pageScriptInfo.referenceCount--;
}

async function initializePageScriptModule(src, pageScriptInfo) {
    // If the path is relative, normalize it by by making it an absolute URL
    // with document's the base HREF.
    if (src.startsWith("./")) {
        src = new URL(src.substr(2), document.baseURI).toString();
    }

    const module = await import(src);

    if (pageScriptInfo.referenceCount <= 0) {
        // All page-script elements with the same 'src' were
        // unregistered while we were loading the module.
        return;
    }

    pageScriptInfo.module = module;
    module.onLoad?.();
    module.onUpdate?.();
}

function onEnhancedLoad() {
    // Start by invoking 'onDispose' on any modules that are no longer referenced.
    for (const [src, { module, referenceCount }] of pageScriptInfoBySrc) {
        if (referenceCount <= 0) {
            module?.onDispose?.();
            pageScriptInfoBySrc.delete(src);
        }
    }

    // Then invoke 'onUpdate' on the remaining modules.
    for (const { module } of pageScriptInfoBySrc.values()) {
        module?.onUpdate?.();
    }
}

function setupPageScriptElement() {
    customElements.define('tnt-page-script', class extends HTMLElement {
        static observedAttributes = ['src'];

        // We use attributeChangedCallback instead of connectedCallback
        // because a page-script element might get reused between enhanced
        // navigations.
        attributeChangedCallback(name, oldValue, newValue) {
            if (name !== 'src') {
                return;
            }

            this.src = newValue;
            unregisterPageScriptElement(oldValue);
            registerPageScriptElement(newValue);
        }

        disconnectedCallback() {
            unregisterPageScriptElement(this.src);
        }
    });
}
export function afterWebStarted(blazor) {
    setupPageScriptElement();
    blazor.addEventListener('enhancedload', onEnhancedLoad);
}

window.TnTComponents = {
    customAttribute: "tntid",
    enableRipple: (element) => {
        function setRippleOffset(e) {
            const boundingRect = element.getBoundingClientRect();
            const x = e.clientX - boundingRect.left - (boundingRect.width / 2);
            const y = e.clientY - boundingRect.top - (boundingRect.height / 2);
            element.style.setProperty('--ripple-offset-x', `${x}px`);
            element.style.setProperty('--ripple-offset-y', `${y}px`);
        }

        if (element) {
            element.addEventListener('click', setRippleOffset);
        }
    },
    downloadFileFromStream: async (fileName, contentStreamReference) => {
        const arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName ?? '';
        anchorElement.click();
        anchorElement.remove();
        URL.revokeObjectURL(url);
    },
    downloadFromUrl: (fileName, url) => {
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName ?? '';
        anchorElement.click();
        anchorElement.remove();
    }
}
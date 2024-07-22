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
    TnTComponents.setupRipple();
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
    TnTComponents.setupRipple();
}


function ripple(e) {

    // Setup
    let posX = this.offsetLeft;
    let posY = this.offsetTop;
    let buttonWidth = this.offsetWidth;
    let buttonHeight = this.offsetHeight;

    // Add the element
    let ripple = document.createElement('span');

    this.appendChild(ripple);


    // Make it round!
    if (buttonWidth >= buttonHeight) {
        buttonHeight = buttonWidth;
    } else {
        buttonWidth = buttonHeight;
    }

    // Get the center of the element
    var x = e.pageX - posX - buttonWidth / 2;
    var y = e.pageY - posY - buttonHeight / 2;


    ripple.style.width = `${buttonWidth}px`;
    ripple.style.height = `${buttonHeight}px`;
    ripple.style.top = `${y}px`;
    ripple.style.left = `${x}px`;

    ripple.classList.add('tnt-rippling');

    setTimeout(() => {
        this.removeChild(ripple);
    }, 500);

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
        TnTComponents.downloadFileFromBlob(fileName, blob);
    },
    downloadFromUrl: async (fileName, url) => {
        const blob = await fetch(url).then(r => r.blob())
        TnTComponents.downloadFileFromBlob(fileName, blob);
    },
    downloadFileFromBlob: (fileName, blob) => {
        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName ?? '';
        anchorElement.click();
        anchorElement.remove();
        URL.revokeObjectURL(url);
    },
    getBoundingClientRect: (element) => {
        if (element && element.getBoundingClientRect) {
            return element.getBoundingClientRect();
        }
        return null;
    },
    setBoundingClientRect: (element, boundingClientRect) => {
        if (element && element.style && boundingClientRect) {
            element.style.top = boundingClientRect.top + 'px';
            element.style.left = boundingClientRect.left + 'px';
            element.style.width = boundingClientRect.width + 'px';
            element.style.height = boundingClientRect.height + 'px';
        }
    },
    hideElement: (element) => {
        if (element && element.style) {
            element.style.display = 'none';
        }
    },
    showElement: (element) => {
        if (element && element.style) {
            element.style.display = 'revert';
        }
    },
    setOpacity: (element, opacity) => {
        if (element && element.style) {
            element.style.opacity = `${opacity}`;
        }
    },
    getCurrentLocation: () => {
        return window.location.href;
    },

    setupRipple: () => {
        const elements = document.querySelectorAll('.tnt-ripple');

        

        elements.forEach(element => {
            element.removeEventListener('click', ripple);
            element.addEventListener('click', ripple, false);
        });
    }
}
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
function getCoords(elem) { // crossbrowser version
    var box = elem.getBoundingClientRect();

    var body = document.body;
    var docEl = document.documentElement;

    var scrollTop = window.scrollY || docEl.scrollTop || body.scrollTop;
    var scrollLeft = window.scrollX || docEl.scrollLeft || body.scrollLeft;

    var clientTop = docEl.clientTop || body.clientTop || 0;
    var clientLeft = docEl.clientLeft || body.clientLeft || 0;

    var top = box.top + scrollTop - clientTop;
    var left = box.left + scrollLeft - clientLeft;

    return { top: Math.round(top), left: Math.round(left) };
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
    ripple.style.pointerEvents = 'none';


    // Make it round!
    if (buttonWidth >= buttonHeight) {
        buttonHeight = buttonWidth;
    } else {
        buttonWidth = buttonHeight;
    }

    // Get the center of the element
    const coords = getCoords(e.target);
    var x = e.pageX - coords.left - buttonWidth / 2;
    var y = e.pageY - coords.top - buttonHeight / 2;


    ripple.style.width = `${buttonWidth}px`;
    ripple.style.height = `${buttonHeight}px`;
    ripple.style.top = `${y}px`;
    ripple.style.left = `${x}px`;

    ripple.classList.add('tnt-rippling');

    setTimeout(() => {
        this.removeChild(ripple);
    }, 500);

}

const isNumericInput = (event) => {
    const key = event.keyCode;
    return ((key >= 48 && key <= 57) || // Allow number line
        (key >= 96 && key <= 105) // Allow number pad
    );
};

const isModifierKey = (event) => {
    const key = event.keyCode;
    return (event.shiftKey === true || key === 35 || key === 36) || // Allow Shift, Home, End
        (key === 8 || key === 9 || key === 13 || key === 46) || // Allow Backspace, Tab, Enter, Delete
        (key > 36 && key < 41) || // Allow left, up, right, down
        (
            // Allow Ctrl/Command + A,C,V,X,Z
            (event.ctrlKey === true || event.metaKey === true) &&
            (key === 65 || key === 67 || key === 86 || key === 88 || key === 90)
        )
};


window.TnTComponents = {
    customAttribute: "tntid",
    openDialog: (dialogId) => {
        console.log('openDialog');
        const dialog = document.getElementById(dialogId);
        if (dialog) {
            dialog.show();
        }
    },
    closeDialog: (dialogId) => {
        const dialog = document.getElementById(dialogId);
        if (dialog) {
            dialog.close();
        }
    },
    openModalDialog: (dialogId) => {
        const dialog = document.getElementById(dialogId);
        if (dialog) {
            dialog.showModal();
        }
    },
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
    enforcePhoneFormat: (event) => {
        // Input must be of a valid number format or a modifier key, and not longer than ten digits
        if (!isNumericInput(event) && !isModifierKey(event)) {
            event.preventDefault();
        }
    },
    enforceCurrencyFormat: (event) => {
        // Input must be of a valid number format or a modifier key, and not longer than ten digits
        if (!isNumericInput(event) && !isModifierKey(event) && event.keyCode != 188 && event.keyCode != 190 && event.keyCode != 110) {
            event.preventDefault();
        }
    },
    formatToCurrency: (event) => {
        if (isModifierKey(event)) { return; }

        const numberRegex = new RegExp('[0-9.]', 'g');
        let numbers = '';
        let result;
        while ((result = numberRegex.exec(event.target.value)) != null) {
            numbers += result.toString();
        }

        // Create our number formatter.
        const formatter = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
        });
        let formatted = formatter.format(numbers);
        if (!event.target.value.includes('.')) {
            formatted = formatted.substring(0, formatted.length - 3);
        }
        else {
            const cents = event.target.value.split('.')[1];
            formatted = formatted.substring(0, formatted.length - 3) + '.' + cents.substring(0, 2);
        }

        event.target.value = formatted;
    },
    formatToPhone: (event) => {
        if (isModifierKey(event)) { return; }

        const input = event.target.value.replace(/\D/g, '').substring(0, 10); // First ten digits of input only
        const areaCode = input.substring(0, 3);
        const middle = input.substring(3, 6);
        const last = input.substring(6, 10);

        if (input.length > 6) { event.target.value = `(${areaCode}) ${middle}-${last}`; }
        else if (input.length > 3) { event.target.value = `(${areaCode}) ${middle}`; }
        else if (input.length > 0) { event.target.value = `(${areaCode}`; }
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
    },
    toggleAccordion: (event) => {
        if (event && event.target) {
            const element = event.target;
            if (element && element.parentElement) {
                const parent = element.parentElement;
                if (parent && parent.querySelector) {
                    const accordion = parent.parentElement;


                    let content = parent.querySelector('.tnt-accordion-content');
                    if (content) {
                        if (content.classList.contains('tnt-hidden')) {
                            if (accordion && accordion.getAttribute) {
                                if (accordion.getAttribute('tnt-one-expanded') != null ? true : false) {
                                    accordion.querySelectorAll('.tnt-accordion-content').forEach(ele => {
                                        ele.classList.add('tnt-hidden');
                                    });
                                }
                            }
                            content.classList.remove('tnt-hidden');
                        }
                        else {
                            content.classList.add('tnt-hidden');
                        }
                    }
                }
            }
        }
    },
    bodyScrollListener: (event) => {
        const headers = document.getElementsByClassName('tnt-header');

        for (const head of headers) {
            if (head && head.classList && event.target) {
                if (event.target.scrollTop > 0) {
                    if (!head.classList.contains('tnt-elevation-2')) {
                        head.classList.add('tnt-elevation-2');
                    }
                }
                else {
                    head.classList.remove('tnt-elevation-2');
                }
            }
        }
    },
    toggleSideNav: (event) => {
        const sideNavs = document.getElementsByClassName('tnt-side-nav-toggle-indicator');

        for (const nav of sideNavs) {
            if (nav && nav.querySelector) {
                const toggler = nav.querySelector('.tnt-toggle-indicator');
                if (toggler && toggler.classList) {
                    if (toggler .classList.contains('tnt-toggle')) {
                        toggler.classList.remove('tnt-toggle');
                    }
                    else {
                        toggler.classList.add('tnt-toggle');
                    }
                }
            }
        }
    },
    toggleSideNavGroup: (event) => {
        const indicator = event.target.parentElement.querySelector('.tnt-side-nav-group-toggle-indicator');

        if (indicator) {
            const toggler = indicator.querySelector('.tnt-toggle-indicator');
            if (toggler && toggler.classList) {
                if (toggler.classList.contains('tnt-toggle')) {
                    toggler.classList.remove('tnt-toggle');
                }
                else {
                    toggler.classList.add('tnt-toggle');
                }
            }
        }
    }
}
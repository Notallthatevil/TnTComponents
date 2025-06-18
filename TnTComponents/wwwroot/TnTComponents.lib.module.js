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

    let body = document.querySelector('.tnt-body');
    if (body) {
        const bodyPadding = parseInt(getComputedStyle(body).paddingBottom, 10);
        const resizeObserver = new ResizeObserver(entries => {
            const hasFooter = document.querySelector('.tnt-footer');
            const fillRemaining = document.querySelectorAll('.tnt-fill-remaining');

            for (const fills of fillRemaining) {
                if (entries[0].target.scrollHeight > entries[0].target.clientHeight) {
                    break;
                }

                var rect = fills.getBoundingClientRect();
                const style = getComputedStyle(fills);
                let height = window.innerHeight - rect.top - bodyPadding;

                const margin = style.marginBottom;
                if (margin) {
                    height = height - parseInt(margin, 10);
                }

                if (hasFooter) {
                    height = height - hasFooter.getBoundingClientRect().height;
                }

                fills.style.height = `${height}px`
            }

        });
        resizeObserver.observe(body);
    }
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
    TnTComponents.rippleEffect(e);
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
    addHidden: (element) => {
        if (element && element.classList && !element.classList.contains('tnt-hidden')) {
            element.classList.add('tnt-hidden');
        }
    },
    /**
     * Returns the color value for a given TnTColor enum variable name as a string.
     * @param {string} colorName - The TnTColor enum variable name (e.g., 'Primary', 'OnPrimaryContainer').
     * @returns {string|null} The color value as defined in CSS variables (e.g., 'var(--tnt-primary)'), or null if not found.
     */
    getColorValueFromEnumName: function(colorName) {
        if (!colorName || typeof colorName !== 'string') return null;

        // Convert PascalCase or camelCase to kebab-case (e.g., 'OnPrimaryContainer' -> 'on-primary-container')
        const kebab = colorName.replace(/(?<=.)([A-Z])/g, '-$1').toLowerCase();
        // Compose the CSS variable name
        const cssVar = `--tnt-color-${kebab}`;
        // Try to get the value from the root element
        let value = getComputedStyle(document.documentElement).getPropertyValue(cssVar);
        if (!value) return null;
        value = value.trim();

        // If value is in rgb(X, Y, Z) or rgba(X, Y, Z, A) format (commas or spaces), convert to hex
        // Support both rgb(224,224,255) and rgb(224 224 255)
        const rgbRegex = /^rgb\s*\(\s*(\d{1,3})[ ,]+(\d{1,3})[ ,]+(\d{1,3})\s*\)$/i;
        const rgbaRegex = /^rgba\s*\(\s*(\d{1,3})[ ,]+(\d{1,3})[ ,]+(\d{1,3})[ ,]+(0|1|0?\.\d+)\s*\)$/i;
        let match = value.match(rgbRegex);
        if (match) {
            // Convert rgb to hex
            const r = parseInt(match[1], 10).toString(16).padStart(2, '0');
            const g = parseInt(match[2], 10).toString(16).padStart(2, '0');
            const b = parseInt(match[3], 10).toString(16).padStart(2, '0');
            return `#${r}${g}${b}`;
        }
        match = value.match(rgbaRegex);
        if (match) {
            // Convert rgba to hex (ignore alpha for hex, or append as 2-digit hex if needed)
            const r = parseInt(match[1], 10).toString(16).padStart(2, '0');
            const g = parseInt(match[2], 10).toString(16).padStart(2, '0');
            const b = parseInt(match[3], 10).toString(16).padStart(2, '0');
            // Optionally include alpha as hex
            // const a = Math.round(parseFloat(match[4]) * 255).toString(16).padStart(2, '0');
            return `#${r}${g}${b}`;
        }
        return value;
    },
    openDialog: (dialogId) => {
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

            dialog.addEventListener('cancel', e => {
                e.preventDefault();
                e.stopPropagation();
            });
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

        let cultureCode = event.target.getAttribute('cultureCode');
        if (!cultureCode) {
            cultureCode = 'en-US';
        }

        let currencyCode = event.target.getAttribute('currencyCode');
        if (!currencyCode) {
            currencyCode = 'USD';
        }

        // Create our number formatter.
        const formatter = new Intl.NumberFormat(cultureCode, {
            style: 'currency',
            currency: currencyCode,
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
            element.appendChild(document.createElement('tnt-ripple-effect'));
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
    toggleSideNav: (event) => {
        const sideNavs = document.getElementsByClassName('tnt-side-nav-toggle-indicator');

        for (const nav of sideNavs) {
            if (nav && nav.querySelector) {
                const toggler = nav.querySelector('.tnt-toggle-indicator');
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
    },
    rippleEffect: (e) => {
        // Setup
        let posX = e.target.offsetLeft;
        let posY = e.target.offsetTop;
        let buttonWidth = e.target.offsetWidth;
        let buttonHeight = e.target.offsetHeight;

        // Add the element
        let ripple = document.createElement('span');

        let target = e.target;
        const rippleEffect = e.target.querySelector(':scope > tnt-ripple-effect');
        if (rippleEffect) {
            target = rippleEffect;
        }

        target.appendChild(ripple);
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
            if (target.contains(ripple)) {
                target.removeChild(ripple);
            }
        }, 500);
    }
}
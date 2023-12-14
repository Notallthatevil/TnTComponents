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
    customAttribute: "tnt-custom-identifier",

    getBoundingRect: (element) => { return element.getBoundingClientRect(); },
    getOffsetPosition: function (element) {
        var x = {
            offsetLeft: element.offsetLeft,
            offsetTop: element.offsetTop,
            offsetHeight: element.offsetHeight,
            offsetWidth: element.offsetWidth
        };
        return x;
    },

    getScrollPosition: (element) => { return element.scrollTop; },

    getWindowHeight: () => { return window.innerHeight; },

    MediaCallbacks: {},
    MediaCallback: function (query, dotNetObjectRef) {
        if (dotNetObjectRef) {
            function callback(event) {
                dotNetObjectRef.invokeMethodAsync('OnChanged', event.matches)
            };
            var query = matchMedia(query);
            this.MediaCallbacks[dotNetObjectRef._id] = function () {
                query.removeListener(callback);
            }
            query.addListener(callback);
            return query.matches;
        } else {
            dotNetObjectRef = query;
            if (this.MediaCallbacks[dotNetObjectRef._id]) {
                this.MediaCallbacks[dotNetObjectRef._id]();
                delete this.MediaCallbacks[dotNetObjectRef._id];
            }
        }
    },

    registeredTnTTabViews: {},

    registerTnTTabView: function (viewIdentifier, children) {
        if (viewIdentifier && children) {
            const tabView = document.querySelector(`[${viewIdentifier}]`);

            let headerArea = tabView.querySelector('.tnt-tab-header');
            let contentArea = tabView.querySelector('.tnt-tab-content');
            let activeIndicator = tabView.querySelector(":scope > #tnt-tab-active-indicator");

            function updateActiveIndicator(headerElement = null) {
                if (!headerElement) {
                    for (const e of headerArea.children) {
                        if (e.classList.contains('active')) {
                            headerElement = e;
                            break;
                        }
                    }
                }

                if (!headerElement) {
                    return;
                }
                const boundingRect = headerElement.getBoundingClientRect();
                const parentScrollLeft = headerElement.parentElement.scrollLeft;
                const diff = boundingRect.left + parentScrollLeft - headerElement.offsetLeft;
                if (tabView.classList.contains('primary')) {
                    let headerElementWidth = headerElement.clientWidth / 2;
                    activeIndicator.style.left = `${(boundingRect.left + headerElementWidth) - (activeIndicator.clientWidth / 2) - diff}px`;
                }
                else if (tabView.classList.contains('secondary')) {
                    activeIndicator.style.left = `${boundingRect.left - diff}px`;
                    activeIndicator.style.width = `${headerElement.clientWidth}px`;
                }
            }

            const resizeObserver = new ResizeObserver((_) => {
                updateActiveIndicator();
            });

            resizeObserver.observe(tabView);
            this.registeredTnTTabViews[viewIdentifier] = resizeObserver;
            headerArea.addEventListener('scroll', (_) => updateActiveIndicator());

            function setContent(contentTemplate, headerElement, forceSet = false) {
                if ((headerElement.classList.contains('active') && forceSet === false) || headerElement.disabled) {
                    return false;
                }

                for (const e of headerArea.children) {
                    e.classList.remove('active');
                }

                const contentClone = contentTemplate.content.cloneNode(true);
                contentArea.innerHTML = '';
                contentArea.appendChild(contentClone);
                headerElement.classList.add('active');

                updateActiveIndicator(headerElement);
                return true;
            }

            let width = 100 / children.length;
            if (width < 8) {
                width = 8;
            }

            let activeSet = false;

            children.forEach((pair, index) => {
                const clone = pair.headerTemplate.content.cloneNode(true);

                let headerItem = Array.prototype.slice.call(clone.childNodes)[0];
                headerArea.appendChild(clone);

                headerItem.style.width = `${width}%`;
                headerItem.addEventListener('click', (e) => {
                    setContent(pair.contentTemplate, headerItem);
                    this.ripple(headerItem, e);
                });

                if (headerItem.classList.contains('active') || index === 0) {
                    if (setContent(pair.contentTemplate, headerItem, true)) {
                        activeSet = true;
                    }
                }
            });

            if (!activeSet) {
                for (const e of headerArea.children) {
                    if (!e.disabled) {
                        e.click();
                        activeSet = true;
                        break;
                    }
                }
            }

            if (!activeSet) {
                activeIndicator.style.display = "none";
            }
        }
    },

    registerTnTTabViewDispose: function (tabViewIdentifier) {
        if (this.registeredTnTTabViews[viewIdentifier]) {
            this.registeredTnTTabViews[viewIdentifier].disconnect();
            delete this.registeredTnTTabViews[viewIdentifier];
        }
    },

    removeFocus: (element) => { element.blur(); },

    remToPx: (rem) => { return rem * parseFloat(getComputedStyle(document.documentElement).fontSize); },

    ripple: (element, event) => {
        const circle = document.createElement("span");
        const diameter = Math.max(element.clientWidth, element.clientHeight);
        const radius = diameter / 2;

        const boundingRect = element.getBoundingClientRect();

        circle.style.width = circle.style.height = `${diameter}px`;
        circle.style.left = `${event.clientX - boundingRect.left - radius}px`;
        circle.style.top = `${event.clientY - boundingRect.top - radius}px`;
        circle.classList.add("tnt-ripple");

        const ripple = element.getElementsByClassName("tnt-ripple")[0];

        if (ripple) {
            ripple.remove();
        }

        element.appendChild(circle);
    },

    scrollElementIntoView: (element) => { element.scrollIntoView(); },
    setBoundingRect: function (element, boundingRect) {
        if (boundingRect.x != null) {
            element.style.x = boundingRect.x + "px";
        }

        if (boundingRect.y) {
            element.style.y = boundingRect.y + "px";
        }

        if (boundingRect.width) {
            element.style.width = boundingRect.width + "px";
        }

        if (boundingRect.height) {
            element.style.height = boundingRect.height + "px";
        }

        if (boundingRect.top) {
            element.style.top = boundingRect.top + "px";
        }

        if (boundingRect.right) {
            element.style.right = boundingRect.right + "px";
        }

        if (boundingRect.bottom) {
            element.style.bottom = boundingRect.bottom + "px";
        }

        if (boundingRect.left) {
            element.style.left = boundingRect.left + "px";
        }
    },

    setFocus: (element) => { element.focus(); },

    setScrollPosition: (element, position) => { element.scrollTop = position; },

    WindowClickCallbacks: {},
    WindowClickCallbackRegister: function (element, dotNetObjectRef) {
        if (dotNetObjectRef) {
            function callback(event) {
                if (!element.contains(event.target)) {
                    dotNetObjectRef.invokeMethodAsync('OnClick')
                }
            };

            this.WindowClickCallbacks[dotNetObjectRef._id] = function () {
                window.removeEventListener('click', callback);
            }

            window.addEventListener('click', callback);
        }
    },

    WindowClickCallbackDeregister: function (dotNetObjectRef) {
        if (this.WindowClickCallbacks[dotNetObjectRef._id]) {
            this.WindowClickCallbacks[dotNetObjectRef._id]();
            delete this.WindowClickCallbacks[dotNetObjectRef._id];
        }
    }
}
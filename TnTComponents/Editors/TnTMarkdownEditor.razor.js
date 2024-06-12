import * as _ from "https://unpkg.com/easymde/dist/easymde.min.js";
import * as __ from "https://cdn.jsdelivr.net/highlight.js/latest/highlight.min.js";

const markdownEditorsMap = new Map();
const elementDotNetRefMap = new Map();

export function onLoad(element, dotNetElementRef) {
    if (!customElements.get('tnt-markdown-editor')) {
        customElements.define('tnt-markdown-editor', class extends HTMLElement {
            static observedAttributes = [TnTComponents.customAttribute];

            // We use attributeChangedCallback instead of connectedCallback
            // because a page-script element might get reused between enhanced
            // navigations.
            attributeChangedCallback(name, oldValue, newValue) {
                if (name !== TnTComponents.customAttribute) {
                    return;
                }

                if (elementDotNetRefMap.get(oldValue)) {
                    elementDotNetRefMap.set(newValue, elementDotNetRefMap.get(oldValue));
                    elementDotNetRefMap.delete(newValue);
                }

                let easyMDE = null;

                if (markdownEditorsMap.get(oldValue)) {
                    easyMDE = markdownEditorsMap.get(oldValue).mde;
                    markdownEditorsMap.delete(oldValue);
                }

                if (easyMDE === null) {
                    let child = this.querySelector('textarea');
                    easyMDE = new EasyMDE({ element: child });
                    easyMDE.codemirror.on("change", function () {
                        var text = easyMDE.value();
                        const dotNetRef = elementDotNetRefMap.get(newValue);
                        if (dotNetRef) {
                            dotNetRef.invokeMethodAsync("UpdateValue", text, easyMDE.options.previewRender(text));
                        }
                    });
                }

                markdownEditorsMap.set(newValue, {
                    element: this,
                    mde: easyMDE
                });
            }

            disconnectedCallback() {
                let attribute = this.getAttribute(TnTComponents.customAttribute);
                if (markdownEditorsMap.get(attribute)) {
                    markdownEditorsMap.delete(attribute);
                }
            }
        });
    }
}

export function onUpdate(element, dotNetElementRef) {
    if (element && dotNetElementRef) {
        const key = element.getAttribute(TnTComponents.customAttribute);

        if (elementDotNetRefMap.get(key)) {
            elementDotNetRefMap.delete(key);
        }
        elementDotNetRefMap.set(key, dotNetElementRef);
    }
}

export function onDispose(element, dotNetElementRef) {
    if (element) {
        const key = element.getAttribute(TnTComponents.customAttribute);
        elementDotNetRefMap.delete(key);
    }
}
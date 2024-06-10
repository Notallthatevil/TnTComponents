import * as _ from "https://unpkg.com/easymde/dist/easymde.min.js";
import * as __ from "https://cdn.jsdelivr.net/highlight.js/latest/highlight.min.js";

const markdownEditorsMap = new Map();

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

                let easyMDE = null;

                if (markdownEditorsMap.get(oldValue)) {
                    easyMDE = markdownEditorsMap.get(oldValue).mde;
                    markdownEditorsMap.delete(oldValue);
                }

                if (easyMDE === null) {
                    let child = this.querySelector('textarea');
                    easyMDE = new EasyMDE({ element: child });
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

}

export function onDispose(element, dotNetElementRef) {
}
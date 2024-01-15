export function onLoad(element, dotNetObjRef) {
    if (!customElements.get('tnt-button')) {
        customElements.define('tnt-button', class extends HTMLElement {
            static observedAttributes = [TnTComponents.customAttribute];

            // We use attributeChangedCallback instead of connectedCallback
            // because a page-script element might get reused between enhanced
            // navigations.
            attributeChangedCallback(name, oldValue, newValue) {
                if (name !== TnTComponents.customAttribute) {
                    return;
                }

                TnTComponents.enableRipple(this);
            }

        });
    }
}

export function onUpdate(element, dotNetObjRef) {

}

export function onDispose(element, dotNetObjRef) {

}
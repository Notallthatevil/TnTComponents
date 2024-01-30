
function toggleTheme(e) {
    const themeCollection = document.getElementsByTagName('tnt-design-theme');
    if (themeCollection && themeCollection.length > 0) {
        const theme = themeCollection[0];

        if (theme.hasAttribute('is-dark')) {
            theme.removeAttribute('is-dark');
        }
        else {
            theme.setAttribute('is-dark', 'true');
        }

        theme.createTheme();
    }
}

export function onLoad(element, dotNetElementRef) {
    if (!customElements.get('tnt-theme-toggle')) {
        customElements.define('tnt-theme-toggle', class extends HTMLElement {
            static observedAttributes = [TnTComponents.customAttribute];

            // We use attributeChangedCallback instead of connectedCallback
            // because a page-script element might get reused between enhanced
            // navigations.
            attributeChangedCallback(name, oldValue, newValue) {
                if (name !== TnTComponents.customAttribute) {
                    return;
                }

                this.addEventListener('click', toggleTheme);
            }
        });
    }
}

export function onUpdate(element, dotNetElementRef) {

}

export function onDispose(element, dotNetElementRef) {

}
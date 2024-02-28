function getCurrentTheme() {
    const themeCollection = document.getElementsByTagName('tnt-design-theme');
    if (themeCollection && themeCollection.length > 0) {
        const theme = themeCollection[0].getAttribute('theme');
        if (theme === 'system') {
            if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
                return 'dark';
            }
        }

        if (theme === 'dark') {
            return 'dark';
        }

        return 'light';
    }
    return null;
}

function toggleTheme(e) {
    const themeCollection = document.getElementsByTagName('tnt-design-theme');
    if (themeCollection && themeCollection.length > 0) {
        const theme = themeCollection[0];

        if (theme) {
            const currentTheme = getCurrentTheme();
            if (currentTheme === 'dark') {
                theme.setAttribute('theme', 'light');
            }
            else {
                theme.setAttribute('theme', 'dark');
            }

            theme.createTheme();
            const toggler = e.target.closest('tnt-theme-toggle');
            if (toggler) {
                toggler.updateIcon();
            }
        }
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
                this.updateIcon();

            }

            updateIcon() {
                this.innerHTML = '';
                let element = document.createElement('span');
                element.className = 'tnt-icon material-icons';
                element.style.userSelect = 'none';
                element.style.cursor = 'pointer';

                const currentTheme = getCurrentTheme();

                if (currentTheme === 'dark') {
                    element.innerHTML = 'light_mode';
                }
                else if (currentTheme === 'light') {
                    element.innerHTML = 'dark_mode';
                }

                this.appendChild(element);
            }
        });
    }
}

export function onUpdate(element, dotNetElementRef) {
}

export function onDispose(element, dotNetElementRef) {
}
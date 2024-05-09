function getCurrentTheme() {
    const darkThemeStyle = document.getElementById('tnt-theme-design-dark');
    const lightThemeStyle = document.getElementById('tnt-theme-design-light');
    if (darkThemeStyle && lightThemeStyle) {
        const darkThemeMedia = darkThemeStyle.getAttribute('media');
        if (darkThemeMedia && darkThemeMedia.includes) {
            if (darkThemeMedia.includes('prefers-color-scheme')) {
                if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
                    return 'dark';
                }
                else {
                    return 'light';
                }
            }
            else if (darkThemeMedia.includes('not all')) {
                return 'light';
            }
            else {
                return 'dark';
            }
        }
    }
    return null;
}

function toggleTheme(e) {
    const darkThemeStyle = document.getElementById('tnt-theme-design-dark');
    const lightThemeStyle = document.getElementById('tnt-theme-design-light');

    if (darkThemeStyle && lightThemeStyle) {
        const currentTheme = getCurrentTheme();
        if (currentTheme === 'dark') {
            darkThemeStyle.setAttribute('media', 'not all');
            lightThemeStyle.setAttribute('media', 'all');
        }
        else {
            darkThemeStyle.setAttribute('media', 'all');
            lightThemeStyle.setAttribute('media', 'not all');
        }

        const toggler = e.target.closest('tnt-theme-toggle');
        if (toggler) {
            toggler.updateIcon();
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
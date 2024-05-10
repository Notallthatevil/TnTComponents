const localStorageKey = 'TnTComponentsStoredThemeKey';
const prefersDark = 'DARK';
const prefersLight = 'LIGHT';
const prefersSystem = 'SYSTEM';

function getStoredTheme() {
    let storedTheme = localStorage.getItem(localStorageKey);
    if (storedTheme) {
        return storedTheme;
    }
    else {
        return null;
    }
}

function setStoredTheme(theme) {
    localStorage.setItem(localStorageKey, theme);
}

function systemPrefersDark() {
    return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches
}

function updateThemeAttributes() {
    const darkThemeStyle = document.getElementById('tnt-theme-design-dark');
    const lightThemeStyle = document.getElementById('tnt-theme-design-light');
    if (darkThemeStyle && lightThemeStyle) {
        const storedTheme = getStoredTheme();
        if (storedTheme === prefersDark || ((storedTheme === prefersSystem || storedTheme == null) && systemPrefersDark())) {
            darkThemeStyle.setAttribute('media', 'all');
            lightThemeStyle.setAttribute('media', 'not all');
            return prefersDark;
        }
        else {
            darkThemeStyle.setAttribute('media', 'not all');
            lightThemeStyle.setAttribute('media', 'all');
            return prefersLight;
        }
    }
    return null;
}

function themeSelected(e) {
    if (e && e.target) {
        setStoredTheme(e.target.value);
        const currentTheme = updateThemeAttributes();
        if (e.target.parentElement && e.target.parentElement.updateIcon) {
            e.target.parentElement.updateIcon(currentTheme);
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

                const currentTheme = updateThemeAttributes();

                this.updateIcon(currentTheme);
                this.initSelect();
            }

            updateIcon(currentTheme) {
                let iconElement = this.querySelector('span.tnt-theme-toggle-icon');
                if (iconElement) {
                    if (currentTheme === prefersDark) {
                        iconElement.innerHTML = 'light_mode';
                    }
                    else if (currentTheme === prefersLight) {
                        iconElement.innerHTML = 'dark_mode';
                    }
                }
            }

            initSelect() {
                let selectElement = this.querySelector('select.tnt-theme-select');
                if (selectElement) {
                    selectElement.addEventListener('change', themeSelected);
                }
            }
        });
    }
}

export function onUpdate(element, dotNetElementRef) {
}

export function onDispose(element, dotNetElementRef) {
}
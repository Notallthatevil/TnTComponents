export class TnTDesignTheme extends HTMLElement {
    createTheme() {
        let css = new CSSStyleSheet();

        const theme = this.getAttribute('theme');
        const attributes = this.attributes;
        let isDark = null;
        if (theme === 'system' || theme === 'dark') {
            isDark = 'true';
        }

        let rules = ':root { ';

        for (let i = 0; i < attributes.length; ++i) {
            const attr = attributes[i];
            if (theme === 'system') {
                if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
                    isDark = 'true';
                }
            }
            if (!isDark && attr.name.includes('dark')) {
                continue;
            } else if (isDark != undefined && isDark != null && isDark === 'true' && attr.name.includes('light')) {
                continue;
            }

            if (!isNaN(attr.value) && !isNaN(parseFloat(attr.value))) {
                rules += `--tnt-${attr.name}: ${parseFloat(attr.value)}rem;`;
            }
            else {
                const name = attr.name.replace('-dark', '').replace('-light', '');
                rules += `--tnt-color-${name}: ${attr.value}; `;
            }
        }
        rules += '}';

        css.insertRule(rules);
        document.adoptedStyleSheets = [css];
    }
}

export function onLoad() {
    if (!customElements.get('tnt-design-theme')) {
        customElements.define('tnt-design-theme', TnTDesignTheme);
    }

    const themeCollection = document.getElementsByTagName('tnt-design-theme');
    if (themeCollection && themeCollection.length > 0) {
        const theme = themeCollection[0];

        theme.createTheme();
    }
}
export function onLoad() {
    const themeCollection = document.getElementsByTagName('tnt-design-theme');
    if (themeCollection && themeCollection.length > 0) {
        const theme = themeCollection[0];
        let css = new CSSStyleSheet();

        const isDark = theme.getAttribute('is-dark');
        const attributes = theme.attributes;

        let rules = ':root { ';

        for (let i = 0; i < attributes.length; ++i) {
            const attr = attributes[i];

            if (!isDark && attr.name.includes('dark')) {
                continue;
            } else if (isDark != undefined && isDark != null && isDark === true && attr.name.includes('light')) {
                continue;
            }

            const name = attr.name.replace('-dark', '').replace('-light', '');
            rules += `--tnt-color-${name}: ${attr.value}; `;
        }
        rules += '}';

        css.insertRule(rules);

        document.adoptedStyleSheets = [css];
    }
}
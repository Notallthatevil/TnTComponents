export function onLoad() {
    const themeCollection = document.getElementsByTagName('tnt-design-theme');
    if (themeCollection && themeCollection.length > 0) {
        const theme = themeCollection[0];
        let css = new CSSStyleSheet();

        const isDark = theme.getAttribute('is-dark');
        const attributes = theme.attributes;

        for (let i = 0; i < attributes.length; ++i) {
            const attr = attributes[i];
            if ((isDark != undefined && attr.name.includes('-light')) ||
                (isDark == und && attr.name.includes('-dark'))) {
                continue;
            }

            const name = attr.name.replace('-dark', '').replace('-light', '');

            css.insertRule(`:root { --tnt-color-${name}: ${attr.value}; }`);
        }

        document.adoptedStyleSheets = [css];
    }
}
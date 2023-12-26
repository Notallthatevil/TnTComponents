export function onUpdate() {
    const url = new URL(import.meta.url);
    const className = url.searchParams.get("class");
    const elementConstraint = url.searchParams.get("elementConstraint");
    document.addEventListener('click', function (e) {
        const elem = e.target.closest(`.${className}`);
        if (elem) {
            if (!elementConstraint || elementConstraint.includes(elem.tagName.toLowerCase())) {
                TnTComponents.ripple(elem, e);
            }
        }
    });
}

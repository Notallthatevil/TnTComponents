export function onUpdate() {
    const url = new URL(import.meta.url);
    const className = url.searchParams.get("class");
    document.addEventListener('click', function (e) {
        const elem = e.target.closest(`.${className}`);
        if (elem) {
            TnTComponents.ripple(elem, e);
        }
    });
}

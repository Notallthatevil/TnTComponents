
export function onUpdate() {
    const url = new URL(import.meta.url);
    const tabViewIdentifier = url.searchParams.get("tabviewidentifier");

    if (tabViewIdentifier) {
        const tabView = document.querySelector(`[${tabViewIdentifier}]`);

        if (tabView) {
            const childHeaders = tabView.querySelectorAll('#tnt-tab-header');
            const childContent = tabView.querySelectorAll('#tnt-tab-content');

            let zipped = [];

            for (let i = 0, head; head = childHeaders[i]; ++i) {
                zipped.push({ headerTemplate: head, contentTemplate: childContent[i] });
            }

            TnTComponents.registerTnTTabView(tabViewIdentifier, zipped);
        }
    }
}

export function onDispose() {
    const url = new URL(import.meta.url);
    const tabViewIdentifier = url.searchParams.get("tabviewidentifier");
    TnTComponents.registerTnTTabViewDispose(tabViewIdentifier);
}
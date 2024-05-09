function findClosestScrollContainer(element) {
    while (element) {
        const style = getComputedStyle(element);
        if (style.overflowY !== 'visible') {
            return element;
        }
        element = element.parentElement;
    }
    return null;
}
function infiniteScollingDispose(observer) {
    observer.disconnect();
}
function isValidTableElement(element) {
    if (element === null) {
        return false;
    }
    return ((element instanceof HTMLTableElement && element.style.display === '') || element.style.display === 'table')
        || ((element instanceof HTMLTableSectionElement && element.style.display === '') || element.style.display === 'table-row-group');
}

export function onLoad(element, dotNetRef) {
    const options = {
        root: findClosestScrollContainer(element),
        rootMargin: '0px',
        threshold: 0,
    };

    if (isValidTableElement(element.parentElement)) {
        element.style.display = 'table-row';
    }

    const observer = new IntersectionObserver(async (entries) => {
        for (const entry of entries) {
            if (entry.isIntersecting) {
                observer.unobserve(element);
                await dotNetObjRef.invokeMethodAsync("LoadMoreItems");
            }
        }
    }, options);

    observer.observe(element);

    return {
        dispose: () => infiniteScollingDispose(observer),
        onNewItems: () => {
            observer.unobserve(element);
            observer.observe(element);
        },
    };
}

export function onUpdate(element, dotNetRef) {
}

export function onDispose(element, dotNetRef) {
}
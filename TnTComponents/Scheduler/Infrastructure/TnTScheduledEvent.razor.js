export function onLoad(element, dotNetRef) {
    if (element && dotNetRef) {
        element.resizeObserver = new ResizeObserver((_) => {
            dotNetRef.invokeMethodAsync("SizeAndPositionElement");
        });
        const scheduler = element.closest('.tnt-scheduler');
        element.resizeObserver.observe(scheduler);
    }
}

export function onUpdate(element, dotNetRef) {
    
}

export function onDispose(element, dotNetRef) {
    if (element && element.resizeObserver && element.resizeObserver.disconnect) {
        element.resizeObserver.disconnect();
    }
}
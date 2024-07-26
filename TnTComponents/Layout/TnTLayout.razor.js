export function onLoad(element = null, dotNetObjectRef = null) {
    window.addEventListener('scroll', TnTComponents.headerScrollListener);
}

export function onUpdate(element = null, dotNetObjectRef = null) {
}

export function onDispose(element = null, dotNetObjectRef = null) {
    window.removeEventListener('scroll', TnTComponents.headerScrollListener);
}
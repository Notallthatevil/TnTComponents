let externalClickCallbacks = {};
let mouseDownTargets = {};

export function externalClickCallbackRegister(element, dotNetObjectRef) {
    if (dotNetObjectRef) {
        const id = dotNetObjectRef._id;
        // If already registered for this id, deregister first to avoid duplicate handlers
        if (externalClickCallbacks[id]) {
            externalClickCallbacks[id]();
            delete externalClickCallbacks[id];
        }
        function onMouseDown(event) {
            mouseDownTargets[id] = event.target;
        }
        function onClick(event) {
            const downTarget = mouseDownTargets[id];
            // If element is gone, or mousedown was outside, treat as external click
            if (!element || !element.contains || !downTarget || !element.contains(downTarget)) {
                dotNetObjectRef.invokeMethodAsync('OnClick');
            }
            // Clean up stored target
            delete mouseDownTargets[id];
        }
        externalClickCallbacks[id] = function () {
            window.removeEventListener('mousedown', onMouseDown);
            window.removeEventListener('click', onClick);
            delete mouseDownTargets[id];
        };
        window.addEventListener('mousedown', onMouseDown);
        window.addEventListener('click', onClick);
    }
}

export function externalClickCallbackDeregister(dotNetObjectRef) {
    if (externalClickCallbacks[dotNetObjectRef._id]) {
        externalClickCallbacks[dotNetObjectRef._id]();
        delete externalClickCallbacks[dotNetObjectRef._id];
    }
}
export function onLoad(element, dotnNetRef) {
}

export function onUpdate(element, dotnNetRef) {
}

export function onDispose(element, dotnNetRef) {
}
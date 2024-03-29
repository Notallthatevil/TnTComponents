let externalClickCallbacks = {};
export function externalClickCallbackRegister(element, dotNetObjectRef) {
    if (dotNetObjectRef) {
        function callback(event) {
            if (!element.contains(event.target)) {
                dotNetObjectRef.invokeMethodAsync('OnClick')
            }
        };

        externalClickCallbacks[dotNetObjectRef._id] = function () {
            window.removeEventListener('click', callback);
        }

        window.addEventListener('click', callback);
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
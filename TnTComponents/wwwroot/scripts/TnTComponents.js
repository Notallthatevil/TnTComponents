window.TnTComponents = {
    getBoundingRect: (element) => { return element.getBoundingClientRect(); },

    getScrollPosition: (element) => { return element.scrollTop; },

    getWindowHeight: () => { return window.innerHeight; },

    MediaCallbacks: {},
    MediaCallback: function (query, dotNetObjectRef) {
        if (dotNetObjectRef) {
            function callback(event) {
                dotNetObjectRef.invokeMethodAsync('OnChanged', event.matches)
            };
            var query = matchMedia(query);
            this.MediaCallbacks[dotNetObjectRef._id] = function () {
                query.removeListener(callback);
            }
            query.addListener(callback);
            return query.matches;
        } else {
            dotNetObjectRef = query;
            if (this.MediaCallbacks[dotNetObjectRef._id]) {
                this.MediaCallbacks[dotNetObjectRef._id]();
                delete this.MediaCallbacks[dotNetObjectRef._id];
            }
        }
    },

    removeFocus: (element) => { element.blur(); },

    remToPx: (rem) => { return rem * parseFloat(getComputedStyle(document.documentElement).fontSize); },

    scrollElementIntoView: (element) => { element.scrollIntoView(); },

    setFocus: (element) => { element.focus(); },

    setScrollPosition: (element, position) => { element.scrollTop = position; },

    WindowClickCallbacks: {},
    WindowClickCallbackRegister: function (element, dotNetObjectRef) {
        if (dotNetObjectRef) {
            function callback(event) {
                if (!element.contains(event.target)) {
                    dotNetObjectRef.invokeMethodAsync('OnClick')
                }
            };

            this.WindowClickCallbacks[dotNetObjectRef._id] = function () {
                window.removeEventListener('click', callback);
            }

            window.addEventListener('click', callback);
        }
    },

    WindowClickCallbackDeregister: function (dotNetObjectRef) {
        if (this.WindowClickCallbacks[dotNetObjectRef._id]) {
            this.WindowClickCallbacks[dotNetObjectRef._id]();
            delete this.WindowClickCallbacks[dotNetObjectRef._id];
        }
    }

}
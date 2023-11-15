window.TnTComponents = {
    createResizableColumns: (tableElement) => {
        const cols = tableElement.querySelectorAll('th');
        [].forEach.call(cols, function (column) {
            const resizer = column.lastChild;

            resizer.style.height = `${column.offsetHeight}px`;

            let x = 0;
            let w = 0;

            const mouseMoveHandler = function (e) {
                const dx = e.clientX - x;
                column.style.width = `${w + dx}px`;
            };

            const mouseUpHandler = function (e) {
                resizer.classList.remove('tnt-grid-resizing');
                document.removeEventListener('mousemove', mouseMoveHandler);
                document.removeEventListener('mouseup', mouseUpHandler);
            };

            const mouseDownHandler = function (e) {
                x = e.clientX;

                const styles = window.getComputedStyle(column);
                w = parseInt(styles.width, 10);

                document.addEventListener('mousemove', mouseMoveHandler);
                document.addEventListener('mouseup', mouseUpHandler);

                resizer.classList.add('tnt-grid-resizing');
            };

            resizer.addEventListener('mousedown', mouseDownHandler);
        });
    },

    getBoundingRect: (element) => { return element.getBoundingClientRect(); },
    getOffsetPosition: function (element) {
        var x = {
            offsetLeft: element.offsetLeft,
            offsetTop: element.offsetTop,
            offsetHeight: element.offsetHeight,
            offsetWidth: element.offsetWidth
        };
        return x;
    },

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
    setBoundingRect: function (element, boundingRect) {
        if (boundingRect.x != null) {
            element.style.x = boundingRect.x + "px";
        }

        if (boundingRect.y) {
            element.style.y = boundingRect.y + "px";
        }

        if (boundingRect.width) {
            element.style.width = boundingRect.width + "px";
        }

        if (boundingRect.height) {
            element.style.height = boundingRect.height + "px";
        }

        if (boundingRect.top) {
            element.style.top = boundingRect.top + "px";
        }

        if (boundingRect.right) {
            element.style.right = boundingRect.right + "px";
        }

        if (boundingRect.bottom) {
            element.style.bottom = boundingRect.bottom + "px";
        }

        if (boundingRect.left) {
            element.style.left = boundingRect.left + "px";
        }
    },

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
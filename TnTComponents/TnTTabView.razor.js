class TnTTabView extends HTMLElement {
    constructor() {
        super();
        this.activeIndex = -1;
    }

    connectedCallback() {
        let headerArea = this.querySelector(':scope > div > #tnt-tab-header-area');
        let contentArea = this.querySelector(':scope > #tnt-tab-content-area');
        let activeIndicator = this.querySelector(":scope > #tnt-tab-active-indicator");

        let headers = [...headerArea.children];
        let content = [...contentArea.children];

        const count = headers.length;
        const self = this;

        function updateActiveIndicator() {
            if (self.activeIndex === -1) {
                return;
            }
            activeIndicator.style.display = 'block';
            const headerElement = headers[self.activeIndex];

            const boundingRect = headerElement.getBoundingClientRect();
            const parentScrollLeft = headerElement.parentElement.scrollLeft;
            const diff = boundingRect.left + parentScrollLeft - headerElement.offsetLeft;
            if (self.classList.contains('primary')) {
                let headerElementWidth = headerElement.clientWidth / 2;
                activeIndicator.style.left = `${(boundingRect.left + headerElementWidth) - (activeIndicator.clientWidth / 2) - diff}px`;
            }
            else if (self.classList.contains('secondary')) {
                activeIndicator.style.left = `${boundingRect.left - diff}px`;
                activeIndicator.style.width = `${headerElement.clientWidth}px`;
            }
        }

        function setContent(index) {
            self.activeIndex = index;
            function setActive(element, active) {
                if (active) {
                    if (!element.classList.contains('active')) {
                        element.classList.add('active')
                    }
                }
                else {
                    element.classList.remove('active');
                }
            }

            for (let i = 0; i < count; ++i) {
                setActive(headers[i], i === index);
                setActive(content[i], i === index);

                if (i === index) {
                    updateActiveIndicator();
                }
            }
        }

        const resizeObserver = new ResizeObserver((_) => {
            updateActiveIndicator();
        });
        resizeObserver.observe(this);

        headerArea.addEventListener('scroll', (_) => updateActiveIndicator());

        headers.forEach((head, index) => {
            if ((this.activeIndex === -1 || head.classList.contains('active')) && !head.disabled) {
                this.activeIndex = index;
                setContent(index);
            }
            head.addEventListener('click', (e) => {
                setContent(index);
                TnTComponents.ripple(head, e);
            });
        });
    }

    disconnectedCallback() {
    }

    adoptedCallback() {
    }

    attributeChangedCallback(name, oldValue, newValue) {
    }

    //initalizeTabView(children) {
    //    if (children) {
    //        let headerArea = this.querySelector(':scope > #tnt-tab-header-area');
    //        let contentArea = this.querySelector(':scope > #tnt-tab-content-area');
    //        let activeIndicator = this.querySelector(":scope > #tnt-tab-active-indicator");
    //
    //        function updateActiveIndicator(headerElement = null) {
    //            if (!headerElement) {
    //                for (const e of headerArea.children) {
    //                    if (e.classList.contains('active')) {
    //                        headerElement = e;
    //                        break;
    //                    }
    //                }
    //            }
    //
    //            if (!headerElement) {
    //                return;
    //            }
    //            const boundingRect = headerElement.getBoundingClientRect();
    //            const parentScrollLeft = headerElement.parentElement.scrollLeft;
    //            const diff = boundingRect.left + parentScrollLeft - headerElement.offsetLeft;
    //            if (this.classList.contains('primary')) {
    //                let headerElementWidth = headerElement.clientWidth / 2;
    //                activeIndicator.style.left = `${(boundingRect.left + headerElementWidth) - (activeIndicator.clientWidth / 2) - diff}px`;
    //            }
    //            else if (this.classList.contains('secondary')) {
    //                activeIndicator.style.left = `${boundingRect.left - diff}px`;
    //                activeIndicator.style.width = `${headerElement.clientWidth}px`;
    //            }
    //        }
    //
    //        function setContent(contentTemplate, headerElement, forceSet = false) {
    //            if ((headerElement.classList.contains('active') && forceSet === false) || headerElement.disabled) {
    //                return false;
    //            }
    //
    //            for (const e of headerArea.children) {
    //                e.classList.remove('active');
    //            }
    //
    //            const contentClone = contentTemplate.content.cloneNode(true);
    //            contentArea.innerHTML = '';
    //            contentArea.appendChild(contentClone);
    //            headerElement.classList.add('active');
    //
    //            updateActiveIndicator(headerElement);
    //            return true;
    //        }
    //
    //        const resizeObserver = new ResizeObserver((_) => {
    //            updateActiveIndicator();
    //        });
    //
    //        resizeObserver.observe(this);
    //        headerArea.addEventListener('scroll', (_) => updateActiveIndicator());
    //
    //
    //        let width = 100 / children.length;
    //        if (width < 8) {
    //            width = 8;
    //        }
    //
    //        let activeSet = false;
    //
    //        children.forEach((pair, index) => {
    //            const clone = pair.headerTemplate.content.cloneNode(true);
    //
    //            let headerItem = Array.prototype.slice.call(clone.childNodes)[0];
    //            headerArea.appendChild(clone);
    //
    //            headerItem.style.width = `${width}%`;
    //            headerItem.addEventListener('click', (e) => {
    //                setContent(pair.contentTemplate, headerItem);
    //                TnTComponents.ripple(headerItem, e);
    //            });
    //
    //            if (headerItem.classList.contains('active') || index === 0) {
    //                if (setContent(pair.contentTemplate, headerItem, true)) {
    //                    activeSet = true;
    //                }
    //            }
    //        });
    //
    //        if (!activeSet) {
    //            for (const e of headerArea.children) {
    //                if (!e.disabled) {
    //                    e.click();
    //                    activeSet = true;
    //                    break;
    //                }
    //            }
    //        }
    //
    //        if (!activeSet) {
    //            activeIndicator.style.display = "none";
    //        }
    //    }
    //}
}

export function onLoad() {
    customElements.define('tnt-tab-view', TnTTabView);
}

export function onUpdate() {

}

export function onDispose() {
}
const tabViewsByIdentifier = new Map();

class TnTTabView extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
        this.activeIndex = -1;
    }

    connectedCallback() {
    }

    disconnectedCallback() {
        let identifier = this.getAttribute(TnTComponents.customAttribute);
        if (tabViewsByIdentifier.get(identifier)) {
            tabViewsByIdentifier.delete(identifier);
        }
    }

    adoptedCallback() {
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            if (tabViewsByIdentifier.get(oldValue)) {
                tabViewsByIdentifier.delete(oldValue);
            }
            tabViewsByIdentifier.set(newValue, this);
        }
    }

    initTabView() {
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

        function setContent(index) {
            self.activeIndex = index;
            for (let i = 0; i < count; ++i) {
                setActive(headers[i], i === index);
                setActive(content[i], i === index);

                if (i === index) {
                    updateActiveIndicator();
                }
            }
        }

        function reset() {
            self.activeIndex = -1;
            activeIndicator.style.display = 'none';
            for (let i = 0; i < count; ++i) {
                setActive(headers[i], false);
                setActive(content[i], false);
            }
        }

        reset();

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
}

export function onLoad() {
    if (!customElements.get('tnt-tab-view')) {
        customElements.define('tnt-tab-view', TnTTabView);
    }
}

export function onUpdate() {
    for (const [identifier, tntTabView] of tabViewsByIdentifier) {
        tntTabView.initTabView();
    }
}

export function onDispose() {
}
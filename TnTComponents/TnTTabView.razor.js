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
        let contentTemplates = [...this.querySelectorAll("#tnt-tab-child-template")];

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

        function reset() {
            if (activeIndicator && activeIndicator.style) {
                activeIndicator.style.display = 'none';
            }
            self.activeIndex = -1;
            if (contentArea) {
                contentArea.innerHTML = '';
            }
            headers.forEach((head) => {
                if (head && head.classList) {
                    head.classList.remove('active');
                }
            })
        }

        function setContent(index) {
            reset();
            self.activeIndex = index;
            headers[self.activeIndex]?.classList.add('active');

            let template = contentTemplates[self.activeIndex]?.content;
            if (template) {
                contentArea?.appendChild(template.cloneNode(true));
            }
            updateActiveIndicator();
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
    for (const [_, tntTabView] of tabViewsByIdentifier) {
        tntTabView.initTabView();
    }
}


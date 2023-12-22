const tabViewsByIdentifier = new Map();

class TnTTabView extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
        this.activeIndex = -1;
        this.resizeObserver = null;
        this.interactive = false;
    }

    disconnectedCallback() {
        let identifier = this.getAttribute(TnTComponents.customAttribute);
        if (tabViewsByIdentifier.get(identifier)) {
            tabViewsByIdentifier.delete(identifier);
        }
        this.mutationObserver.disconnect();
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            if (tabViewsByIdentifier.get(oldValue)) {
                tabViewsByIdentifier.delete(oldValue);
            }
            tabViewsByIdentifier.set(newValue, this);
        }
    }

    getActiveHeader() {
        let activeHeader = null;
        let headerArea = this.querySelector(':scope > div > #tnt-tab-header-group');
        if (headerArea) {
            let headers = [...headerArea.children];

            headers.forEach((head, index) => {
                if (this.activeIndex == index) {
                    activeHeader = head;
                }
            });
        }

        return activeHeader;
    }

    update() {
        const self = this;
        let tabChildren = [...this.querySelectorAll(':scope > #tnt-tab-child')];
        let headerArea = this.querySelector(':scope > div > #tnt-tab-header-group');

        if (!this.mutationObserver) {
            this.mutationObserver = new MutationObserver((mutationList, observer) => {
                self.activeIndex = -1;
                for (const mutation of mutationList) {
                    if (mutation.type === 'childList' || mutation.type === 'attributes') {
                        let headers = [...headerArea.children];
                        headers.some((head, index) => {
                            if (head.classList.contains('active') && !head.disabled) {
                                self.activeIndex = index;
                                return true;
                            }
                            else if (!head.disabled && self.activeIndex === -1) {
                                self.activeIndex = index;
                            }
                        });

                        break;
                    }
                }
                observer.disconnect();
                self.update();
            });
        }

        if (this.resizeObserver) {
            this.resizeObserver.disconnect();
        }

        this.resizeObserver = new ResizeObserver((_) => {
            updateActiveIndicator(this, this.getActiveHeader());
        });
        this.resizeObserver.observe(this);

        if (headerArea) {
            this.mutationObserver.observe(headerArea, { childList: true, attributes: true, attributeFilter: ['name'] });
            headerArea.addEventListener('scroll', (_) => updateActiveIndicator(self, self.getActiveHeader()));

            let headers = [...headerArea.children];
            if (headers.length === tabChildren.length && !this.interactive) {
                function setHeaderActive() {
                    let activeHead = null;
                    headers.forEach((head, index) => {
                        if (index === self.activeIndex) {
                            if (!head.classList.contains('active')) {
                                head.classList.add('active');
                            }
                            activeHead = head;
                        }
                        else {
                            head.classList.remove('active');
                        }
                    })

                    updateActiveIndicator(self, activeHead);
                }

                function setNewTabContent() {
                    tabChildren.forEach((child, index) => {
                        if (index === self.activeIndex) {
                            child.hidden = false;
                        }
                        else {
                            child.hidden = true;
                        }
                    });
                }

                function selectActiveTab() {
                    setHeaderActive();
                    setNewTabContent();
                }

                function headClicked(e) {
                    let index = e.currentTarget.selectIndex;
                    if (index !== self.activeIndex) {
                        self.activeIndex = index;
                        selectActiveTab();
                    }
                }

                headers.forEach((head, index) => {
                    head.removeEventListener('click', headClicked);
                    head.addEventListener('click', headClicked);
                    head.selectIndex = index;

                    if (self.activeIndex === -1 && head.classList.contains('active') && !head.disabled) {
                        self.activeIndex = index;
                    }
                });
                selectActiveTab();
            }
        }
    }
}

export function ensureInteractive(tabViewElement) {
    tabViewElement.interactive = true;
}
export function updateActiveIndex(tabViewElement, newIndex) {
    if (tabViewElement) {
        tabViewElement.activeIndex = newIndex;
    }
}

export function updateActiveIndicator(tabViewElement, headerElement) {
    let activeIndicator = tabViewElement.querySelector(":scope > #tnt-tab-active-indicator");
    if (!headerElement || !activeIndicator) {
        if (activeIndicator) {
            activeIndicator.style.display = 'none';
        }
        return;
    }
    activeIndicator.style.display = 'block';

    const boundingRect = headerElement.getBoundingClientRect();
    const parentScrollLeft = headerElement.parentElement.scrollLeft;
    const diff = boundingRect.left + parentScrollLeft - headerElement.offsetLeft;
    if (tabViewElement.classList.contains('primary')) {
        let headerElementWidth = headerElement.clientWidth / 2;
        activeIndicator.style.left = `${(boundingRect.left + headerElementWidth) - (activeIndicator.clientWidth / 2) - diff}px`;
    }
    else if (tabViewElement.classList.contains('secondary')) {
        activeIndicator.style.left = `${boundingRect.left - diff}px`;
        activeIndicator.style.width = `${headerElement.clientWidth}px`;
    }
}

export function onLoad() {
    if (!customElements.get('tnt-tab-view')) {
        customElements.define('tnt-tab-view', TnTTabView);
    }
}

export function onUpdate() {
    for (const [_, tntTabView] of tabViewsByIdentifier) {
        tntTabView.update();
    }
}
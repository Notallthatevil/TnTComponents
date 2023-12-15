const tabViewsByIdentifier = new Map();

class TnTTabView extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
        this.activeIndex = -1;
        this.resizeObserver = null;
        this.interactive = false;
    }

    connectedCallback() {
        this.update();
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
        this.update();
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
        let templates = [...this.querySelectorAll(':scope > #tnt-tab-child-template')];
        let headerArea = this.querySelector(':scope > div > #tnt-tab-header-group');

        if (this.resizeObserver) {
            this.resizeObserver.disconnect();
        }
        
        this.resizeObserver = new ResizeObserver((_) => {
            updateActiveIndicator(this, this.getActiveHeader());
        });
        this.resizeObserver.observe(this);

        if (headerArea) {
            headerArea.addEventListener('scroll', (_) => updateActiveIndicator(self, self.getActiveHeader()));

            let headers = [...headerArea.children];
            if (headers.length === templates.length && !this.interactive) {

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
                    if (self.activeIndex > -1 && self.activeIndex < templates.length) {
                        let content = templates[self.activeIndex].content;
                        if (content) {
                            let currentContent = self.querySelector(':scope > #tnt-tab-child-content');
                            if (currentContent) {
                                self.removeChild(currentContent);
                            }
                            self.appendChild(content.cloneNode(true));
                        }
                    }
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



                self.activeIndex = -1;

                headers.forEach((head, index) => {
                    if ((self.activeIndex === -1 || head.classList.contains('active')) && !head.disabled) {
                        self.activeIndex = index;
                        selectActiveTab();
                    }

                    head.removeEventListener('click', headClicked);
                    head.addEventListener('click', headClicked);
                    head.selectIndex = index;
                });
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
    //for (const [_, tntTabView] of tabViewsByIdentifier) {
    //    tntTabView.initTabView();
    //}
}


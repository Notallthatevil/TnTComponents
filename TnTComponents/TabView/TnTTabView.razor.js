const tabViewsByIdentifier = new Map();

const regexSearch = /tnt-fg-color-[a-z-]+/;
export class TnTTabView extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
        this.activeIndex = -1;
        this.tabViews = [];
        this.resizeObserver = null;
    }

    disconnectedCallback() {
        let identifier = this.getAttribute(TnTComponents.customAttribute);
        if (tabViewsByIdentifier.get(identifier)) {
            tabViewsByIdentifier.delete(identifier);
        }
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            if (tabViewsByIdentifier.get(oldValue)) {
                tabViewsByIdentifier.delete(oldValue);
            }
            tabViewsByIdentifier.set(newValue, this);
            this.update().then(() => {
                this.classList.add('ready');
            });
        }

    }

    async update() {
        this.tabViews = [];
        this.querySelectorAll('tnt-tab-child').forEach((element, index) => {
            this.tabViews.push(element);
            if ((this.activeIndex === -1 || this.activeIndex === index) && !element.classList.contains('tnt-disabled')) {
                this.activeIndex = index;
                element.style.display = 'block';
            }
            else {
                element.style.display = 'none';
            }
        });

        let self = this;

        this.querySelectorAll('div > span > button').forEach((button, index) => {
            TnTComponents.enableRipple(button);

            function setActiveTab(e) {
                if (self) {
                    const headerButtons = self.querySelectorAll(":scope > div > span > button");

                    if (index >= 0 && self.tabViews.length > index) {
                        self.tabViews[self.activeIndex].style.display = 'none';

                        const parentClasses = e.target.parentNode.className.match(regexSearch);
                        headerButtons[self.activeIndex].className = headerButtons[self.activeIndex].className.replace(regexSearch, '');
                        if (parentClasses && parentClasses.length > 0) {
                            headerButtons[self.activeIndex].classList.add(parentClasses[0]);
                        }

                        self.tabViews[index].style.display = 'block';
                        self.activeIndex = index;
                        self.updateActiveIndicator();
                    }
                }
            }

            button.addEventListener('click', setActiveTab);
        });

        this.updateActiveIndicator();

        if (this.resizeObserver) {
            this.resizeObserver.disconnect();
        }
        this.resizeObserver = new ResizeObserver((_) => {
            self.updateActiveIndicator();
        });
        this.resizeObserver.observe(this);
    }

    getActiveHeader() {
        let headerButtons = this.querySelectorAll(':scope > div > span > button');
        if (headerButtons && this.activeIndex >= 0 && headerButtons.length > this.activeIndex) {
            return headerButtons[this.activeIndex];
        }

        return null;
    }

    async updateActiveIndicator() {
        const activeHeader = this.getActiveHeader();
        let activeIndicator = this.querySelector(":scope > div > span:last-child");
        if (!activeHeader || !activeIndicator) {
            if (activeIndicator) {
                activeIndicator.style.display = 'none';
            }
            return;
        }
        activeIndicator.style.display = 'block';
        const bgClass = activeIndicator.className.match('tnt-bg-color-[a-z-]+');
        if (bgClass && bgClass.length > 0) {
            activeHeader.className = activeHeader.className.replace(regexSearch, '');
            activeHeader.classList.add(bgClass[0].replace('-bg-', '-fg-'));
        }

        const boundingRect = activeHeader.getBoundingClientRect();
        const parentScrollLeft = activeHeader.parentElement.scrollLeft;
        const diff = boundingRect.left + parentScrollLeft - activeHeader.offsetLeft;
        if (!this.classList.contains('tnt-alternative')) {
            const headerElementWidth = activeHeader.clientWidth / 2;
            activeIndicator.style.left = `${(boundingRect.left + headerElementWidth) - (activeIndicator.clientWidth / 2) - diff}px`;
        }
        else {
            activeIndicator.style.left = `${boundingRect.left - diff}px`;
            activeIndicator.style.width = `${activeHeader.clientWidth}px`;
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

export function onLoad(element, dotNetRef) {
    if (!customElements.get('tnt-tab-view')) {
        customElements.define('tnt-tab-view', TnTTabView);
    }

    if (!customElements.get('tnt-tab-child')) {
        customElements.define('tnt-tab-child', class extends HTMLElement { });
    }
}

export function onUpdate(element, dotNetRef) {
    if (dotNetRef) {
        element.update();
    }
}

export function onDispose(element, dotNetRef) {
}
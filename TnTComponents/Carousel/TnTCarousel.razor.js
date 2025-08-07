const carouselsByIdentifier = new Map();

function clamp(number, min, max) {
    return Math.min(Math.max(number, min), max);
}

export class TnTCarousel extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
        this._scrollListener = null;
        this.nextIndex = null;
        this.prevIndex = null;
        this.carouselViewPort = null;
        this.prevButton = null;
        this.nextButton = null;
        this.currentIndex = 0;
        this.carouselItemCount = 0;
    }

    disconnectedCallback() {
        let identifier = this.getAttribute(TnTComponents.customAttribute);
        if (carouselsByIdentifier.get(identifier)) {
            carouselsByIdentifier.delete(identifier);
        }
        // Remove scroll listener
        if (this._scrollListener) {
            this.carouselViewPort.removeEventListener('scroll', this._scrollListener);
            this._scrollListener = null;
        }

        if (this.prevButton) {
            this.prevButton.removeEventListener('click', this.prevIndex);
            this.prevButton = null;
        }

        if (this.nextButton) {
            this.nextButton.removeEventListener('click', this.nextIndex);
            this.nextButton = null;
        }
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            if (carouselsByIdentifier.get(oldValue)) {
                carouselsByIdentifier.delete(oldValue);
            }
            carouselsByIdentifier.set(newValue, this);

            this.carouselViewPort = this.querySelector(':scope > .tnt-carousel-viewport');
            this.prevButton = this.querySelector(':scope > .tnt-carousel-prev-button');
            this.nextButton = this.querySelector(':scope > .tnt-carousel-next-button');
            this._recalculateChildWidths();

            // Add scroll listener when attached to DOM
            this._scrollListener = () => {
                this._recalculateChildWidths();
            };

            this.nextIndex = () => {
                this._nextIndex();
            };

            this.prevIndex = () => {
                this._prevIndex();
            };

            this.carouselViewPort.addEventListener('scroll', this._scrollListener);
            this.prevButton.addEventListener('click', this.prevIndex);
            this.nextButton.addEventListener('click', this.nextIndex);
        }
    }

    _recalculateChildWidths() {
        // Find all tnt-carousel-item children and call _updateSize
        const items = this.querySelectorAll(':scope > .tnt-carousel-viewport > tnt-carousel-item');
        items.forEach(item => {
            if (typeof item.containWithinScrollParent === 'function') {
                item.containWithinScrollParent();
            }
        });
        this.carouselItems = items;
    }

    _nextIndex() {
        this.updateIndex(this.currentIndex + 1);
    }

    _prevIndex() {
        this.updateIndex(this.currentIndex - 1);
    }

    updateIndex(newIndex) {
        if (this.carouselItems?.length > 0) {
            this.currentIndex = clamp(newIndex, 0, this.carouselItems.length - 1);
            const itemLeft = this.carouselItems[this.currentIndex].offsetLeft - this.carouselViewPort.offsetLeft;

            this.carouselViewPort.scrollTo({
                left: itemLeft,
                behavior: 'smooth'
            });
        }
    }
}

export function onLoad(element, dotNetRef) {
    if (!customElements.get('tnt-carousel')) {
        customElements.define('tnt-carousel', TnTCarousel);
    }
}

export function onUpdate(element, dotNetRef) { }
export function onDispose(element, dotNetRef) {
    if (element instanceof TnTCarousel) {
        element.disconnectedCallback();
    }
}
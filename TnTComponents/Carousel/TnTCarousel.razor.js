const carouselsByIdentifier = new Map();

function clamp(number, min, max) {
    return Math.min(Math.max(number, min), max);
}

const gapSize = 8;
export class TnTCarousel extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
        this.carouselScrollContainer = null;
        this.carouselViewPort = null;
        this.carouselItems = null;
        this.fullSize = null;
        this._scrollListener = null;


        this.nextIndex = null;
        this.prevIndex = null;
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

        //if (this.prevButton) {
        //    this.prevButton.removeEventListener('click', this.prevIndex);
        //    this.prevButton = null;
        //}

        //if (this.nextButton) {
        //    this.nextButton.removeEventListener('click', this.nextIndex);
        //    this.nextButton = null;
        //}
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            if (carouselsByIdentifier.get(oldValue)) {
                carouselsByIdentifier.delete(oldValue);
            }
            carouselsByIdentifier.set(newValue, this);
            this.carouselScrollContainer = this.querySelector(':scope > .tnt-carousel-scroll-container');
            this.carouselViewPort = this.carouselScrollContainer.querySelector(':scope > .tnt-carousel-viewport');
            this.carouselItems = this.carouselViewPort.querySelectorAll(':scope > tnt-carousel-item');
            this.carouselViewPort.style.setProperty('--tnt-carousel-item-gap', `${gapSize}px`);

            this._calculateFullSize();
            this._recalculateChildWidths();
            //    this.prevButton = this.querySelector(':scope > .tnt-carousel-prev-button');
            //    this.nextButton = this.querySelector(':scope > .tnt-carousel-next-button');

            // Add scroll listener when attached to DOM
            this._scrollListener = () => {
                this._recalculateChildWidths();
            };

            //    this.nextIndex = () => {
            //        this._nextIndex();
            //    };

            //    this.prevIndex = () => {
            //        this._prevIndex();
            //    };

            this.carouselScrollContainer.addEventListener('scroll', this._scrollListener);
            //    this.prevButton.addEventListener('click', this.prevIndex);
            //    this.nextButton.addEventListener('click', this.nextIndex);
        }
    }

    _calculateFullSize() {
        if (this.carouselViewPort && this.carouselItems?.length > 0) {
            const parentRect = this.carouselViewPort.getBoundingClientRect();
            this.fullSize = (this.carouselItems.length * (parentRect.width * 0.8)) + (this.carouselItems.length * gapSize);
            this.carouselViewPort.style.width = `${this.fullSize}px`;
        }
    }

    _recalculateChildWidths() {
        if (this.carouselItems) {
            // Find all tnt-carousel-item children and call _updateSize
            this.carouselItems.forEach(item => {
                if (typeof item.updateItemWidth === 'function') {
                    item.updateItemWidth();
                }
            });
        }
    }

    _nextIndex() {
        //this.updateIndex(this.currentIndex + 1);
    }

    _prevIndex() {
        //this.updateIndex(this.currentIndex - 1);
    }

    updateIndex(newIndex) {
        //if (this.carouselItems?.length > 0) {
        //    this.currentIndex = clamp(newIndex, 0, this.carouselItems.length - 1);
        //    const itemLeft = this.carouselItems[this.currentIndex].offsetLeft - this.carouselViewPort.offsetLeft;

        //    this.carouselViewPort.scrollTo({
        //        left: itemLeft,
        //        behavior: 'smooth'
        //    });
        //}
    }
}

export function onLoad(element, dotNetRef) {
    if (!customElements.get('tnt-carousel')) {
        customElements.define('tnt-carousel', TnTCarousel);
    }
}

export function onUpdate(element, dotNetRef) { }
export function onDispose(element, dotNetRef) {
}
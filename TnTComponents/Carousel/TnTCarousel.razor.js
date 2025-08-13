const carouselsByIdentifier = new Map();

const gapSize = 8;
export class TnTCarousel extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
        this.carouselViewPort = null;
        this.carouselItems = null;
        this.fullSize = null;
        this._scrollListener = null;
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
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            if (carouselsByIdentifier.get(oldValue)) {
                carouselsByIdentifier.delete(oldValue);
            }
            carouselsByIdentifier.set(newValue, this);
            this.onUpdate();
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

    onUpdate() {
        this.carouselViewPort = this.querySelector(':scope > .tnt-carousel-viewport');
        this.carouselItems = this.carouselViewPort.querySelectorAll(':scope > tnt-carousel-item');
        this.carouselViewPort.style.setProperty('--tnt-carousel-item-gap', `${gapSize}px`);

        this._recalculateChildWidths();

        if (this._scrollListener) {
            this.carouselViewPort.removeEventListener('scroll', this._scrollListener);
            this._scrollListener = null;
        }
        // Add scroll listener when attached to DOM
        this._scrollListener = () => {
            this._recalculateChildWidths();
        };

        this.carouselViewPort.addEventListener('scroll', this._scrollListener);
    }
}

export function onLoad(element, dotNetRef) {
    if (!customElements.get('tnt-carousel')) {
        customElements.define('tnt-carousel', TnTCarousel);
    }
}

export function onUpdate(element, dotNetRef) {
    if (element && element instanceof TnTCarousel) {
        element.onUpdate();
    } 
}
export function onDispose(element, dotNetRef) {
}
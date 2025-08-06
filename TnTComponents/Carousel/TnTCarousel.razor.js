const carouselsByIdentifier = new Map();

export class TnTCarousel extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
        this._scrollListener = null;
        this.carouselViewPort = null;
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

            this.carouselViewPort = this.querySelector(':scope > .tnt-carousel-viewport');
            this._recalculateChildWidths();

            // Add scroll listener when attached to DOM
            this._scrollListener = () => {
                this._recalculateChildWidths();
            };
            this.carouselViewPort.addEventListener('scroll', this._scrollListener);
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
    }
}

export function onLoad(element, dotNetRef) {
    if (!customElements.get('tnt-carousel')) {
        customElements.define('tnt-carousel', TnTCarousel);
    }
}

export function onUpdate(element, dotNetRef) {}
export function onDispose(element, dotNetRef) {
    if (element instanceof TnTCarousel) {
        element.disconnectedCallback();
    }
}
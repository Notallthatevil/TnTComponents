const carouselsByIdentifier = new Map();

export class TnTCarouselItem extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
    }

    disconnectedCallback() {
        let identifier = this.getAttribute(TnTComponents.customAttribute);
        if (carouselsByIdentifier.get(identifier)) {
            carouselsByIdentifier.delete(identifier);
        }
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            if (carouselsByIdentifier.get(oldValue)) {
                carouselsByIdentifier.delete(oldValue);
            }
            carouselsByIdentifier.set(newValue, this);
            this._updateSize();
        }
    }

    _updateSize() {
        const bgImage = this.style.backgroundImage;
        if (bgImage && bgImage !== 'none') {
            // Extract URL from background-image: url("...")
            const match = bgImage.match(/url\(["']?(.*?)["']?\)/);
            if (match && match[1]) {
                const img = new window.Image();
                img.onload = () => {
                    this.style.setProperty('--tnt-carousel-item-bg-width', `${img.naturalWidth}px`);
                };
                img.src = match[1];
            }
        }
    }
}
export function onLoad(element, dotNetRef) {
    if (!customElements.get('tnt-carousel-item')) {
        customElements.define('tnt-carousel-item', TnTCarouselItem);
    }

}

export function onUpdate(element, dotNetRef) {
    if (element?._updateSize) {
        element._updateSize();
    }

}
export function onDispose(element, dotNetRef) {
    if (element instanceof TnTCarousel) {
        element.disconnectedCallback();
    }
}
const carouselsByIdentifier = new Map();

export class TnTCarouselItem extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
        this.backgroundImageWidth = '';
        this.naturalWidth = '80%';
        this.style.setProperty('--tnt-carousel-item-bg-width', this.naturalWidth);
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

            this.carouselContainer = this.closest('.tnt-carousel-viewport');
            this.updateSize();
        }
    }

    updateSize() {
        const bgImage = this.style.backgroundImage;
        if (bgImage && bgImage !== 'none') {
            // Extract URL from background-image: url("...")
            const match = bgImage.match(/url\(["']?(.*?)["']?\)/);
            if (match && match[1]) {
                const img = new window.Image();
                img.onload = () => {
                    this.naturalWidth = img.naturalWidth;
                    this.backgroundImageWidth = `${this.naturalWidth}px`;
                    this.style.setProperty('--tnt-carousel-item-bg-width', this.backgroundImageWidth);
                    this.containWithinScrollParent();
                };
                img.src = match[1];
            }
        } else {
            this.containWithinScrollParent();
        }
    }

    containWithinScrollParent() {
        // Get bounding rects
        const parentRect = this.carouselContainer.getBoundingClientRect();
        const itemRect = this.getBoundingClientRect();

        let newWidth = this.naturalWidth;
        if (itemRect.width > parentRect.width) {
            newWidth = parentRect.width;
        }
        else {
            if (itemRect.left < parentRect.left) { // Scrolled off to the left 
                newWidth = itemRect.right - parentRect.left;
            }
            else if (parentRect.right > itemRect.right) {
                newWidth = parentRect.right - itemRect.left;
            }
        }
        this.style.width = `${newWidth}px`;
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
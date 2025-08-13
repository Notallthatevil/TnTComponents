const carouselsByIdentifier = new Map();

const gapSize = 8;
export class TnTCarousel extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute, 'tnt-auto-play-interval'];
    constructor() {
        super();
        this.carouselViewPort = null;
        this.carouselItems = null;
        this.fullSize = null;
        this._scrollListener = null;
        // Drag state
        this._isDragging = false;
        this._dragStartX = 0;
        this._dragScrollLeft = 0;
        this._dragMoved = false;
        this._onPointerDown = null;
        this._onPointerMove = null;
        this._onPointerUp = null;
        this._onPointerLeave = null;
        this._onClickCapture = null;
        this._rafId = null; // for throttling recalculations during drag
        // Resize observer
        this._resizeObserver = null;
        // Auto play
        this._autoPlayIntervalMs = null;
        this._autoPlayTimerId = null;
        this._currentAutoIndex = -1; // start before first
    }

    disconnectedCallback() {
        let identifier = this.getAttribute(TnTComponents.customAttribute);
        if (carouselsByIdentifier.get(identifier)) {
            carouselsByIdentifier.delete(identifier);
        }
        // Remove scroll listener
        if (this._scrollListener && this.carouselViewPort) {
            this.carouselViewPort.removeEventListener('scroll', this._scrollListener);
            this._scrollListener = null;
        }
        if (this._rafId) {
            cancelAnimationFrame(this._rafId);
            this._rafId = null;
        }
        if (this._resizeObserver) {
            this._resizeObserver.disconnect();
            this._resizeObserver = null;
        }
        this._stopAutoPlay();
        this._detachDragEvents();
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            if (carouselsByIdentifier.get(oldValue)) {
                carouselsByIdentifier.delete(oldValue);
            }
            carouselsByIdentifier.set(newValue, this);
            this.onUpdate();
        } else if (name === 'tnt-auto-play-interval' && oldValue !== newValue) {
            this._configureAutoPlayFromAttribute();
        }
    }

    _configureAutoPlayFromAttribute() {
        const attr = this.getAttribute('tnt-auto-play-interval');
        if (attr == null || attr === '') {
            this._autoPlayIntervalMs = null;
            this._stopAutoPlay();
            return;
        }
        const seconds = parseFloat(attr);
        if (!isNaN(seconds) && seconds > 0) {
            this._autoPlayIntervalMs = seconds * 1000;
            this._startAutoPlay();
        } else {
            this._autoPlayIntervalMs = null;
            this._stopAutoPlay();
        }
    }

    _startAutoPlay() {
        this._stopAutoPlay();
        if (!this._autoPlayIntervalMs) return;
        if (!this.carouselItems || this.carouselItems.length === 0) return;
        // Initialize index so first tick goes to item 0
        if (this._currentAutoIndex < 0 || this._currentAutoIndex >= this.carouselItems.length) {
            this._currentAutoIndex = -1;
        }
        this._autoPlayTimerId = setInterval(() => {
            if (this._isDragging) return; // pause while dragging
            this._advanceAutoPlay();
        }, this._autoPlayIntervalMs);
    }

    _stopAutoPlay() {
        if (this._autoPlayTimerId) {
            clearInterval(this._autoPlayTimerId);
            this._autoPlayTimerId = null;
        }
    }

    _advanceAutoPlay() {
        if (!this.carouselItems || this.carouselItems.length === 0) return;
        this._currentAutoIndex = (this._currentAutoIndex + 1) % this.carouselItems.length;
        const target = this.carouselItems[this._currentAutoIndex];
        if (!target) return;
        // Scroll so the target's left aligns. Use smooth if snapping enabled; otherwise instant.
        this.carouselViewPort.scrollTo({ left: target.offsetLeft, behavior: 'smooth' });
    }

    _recalculateChildWidths() {
        if (this.carouselItems) {
            this.carouselItems.forEach(item => {
                if (typeof item.updateItemWidth === 'function') {
                    item.updateItemWidth();
                }
            });
        }
    }

    _detachDragEvents() {
        if (!this.carouselViewPort) return;
        if (this._onPointerDown) this.carouselViewPort.removeEventListener('pointerdown', this._onPointerDown);
        if (this._onPointerMove) this.carouselViewPort.removeEventListener('pointermove', this._onPointerMove);
        if (this._onPointerUp) this.carouselViewPort.removeEventListener('pointerup', this._onPointerUp);
        if (this._onPointerLeave) this.carouselViewPort.removeEventListener('pointerleave', this._onPointerLeave);
        if (this._onClickCapture) this.carouselViewPort.removeEventListener('click', this._onClickCapture, true);
        this._onPointerDown = this._onPointerMove = this._onPointerUp = this._onPointerLeave = this._onClickCapture = null;
    }

    _scheduleRecalc() {
        if (this._rafId) return; // already scheduled
        this._rafId = requestAnimationFrame(() => {
            this._rafId = null;
            this._recalculateChildWidths();
        });
    }

    _attachDragEvents() {
        if (!this.carouselViewPort) return;
        // Clean existing first
        this._detachDragEvents();

        const moveThreshold = 8; // px before we consider it a drag
        this._onPointerDown = (e) => {
            if (e.button !== 0 && e.pointerType === 'mouse') return;
            this._isDragging = true;
            this._dragMoved = false;
            this._dragStartX = e.clientX;
            this._dragScrollLeft = this.carouselViewPort.scrollLeft;
            this.carouselViewPort.classList.add('tnt-carousel-dragging');
            this._stopAutoPlay(); // pause autoplay immediately on interaction
            try { this.carouselViewPort.setPointerCapture(e.pointerId); } catch { }
        };
        this._onPointerMove = (e) => {
            if (!this._isDragging) return;
            const dx = e.clientX - this._dragStartX;
            if (Math.abs(dx) > moveThreshold) this._dragMoved = true;
            e.preventDefault();
            const newLeft = this._dragScrollLeft - dx;
            if (this.carouselViewPort.scrollLeft !== newLeft) {
                this.carouselViewPort.scrollLeft = newLeft; // direct assignment triggers scroll event
                // Fallback immediate measurement (helps some browsers while dragging)
                this._scheduleRecalc();
                this.carouselViewPort.dispatchEvent(new Event('scroll'));
            }
        };
        const finishDrag = () => {
            if (!this._isDragging) return;
            this._isDragging = false;
            this.carouselViewPort.classList.remove('tnt-carousel-dragging');
            // Ensure final recalculation after last movement
            this._scheduleRecalc();
            // Re-align autoplay index to closest item so rotation appears natural
            this._syncAutoIndexToViewport();
            this._startAutoPlay();
        };
        this._onPointerUp = () => { finishDrag(); };
        this._onPointerLeave = () => { finishDrag(); };
        this._onClickCapture = (e) => {
            if (this._dragMoved) {
                e.stopPropagation();
                e.preventDefault();
            }
        };

        this.carouselViewPort.addEventListener('pointerdown', this._onPointerDown, { passive: true });
        this.carouselViewPort.addEventListener('pointermove', this._onPointerMove, { passive: false });
        this.carouselViewPort.addEventListener('pointerup', this._onPointerUp, { passive: true });
        this.carouselViewPort.addEventListener('pointerleave', this._onPointerLeave, { passive: true });
        this.carouselViewPort.addEventListener('click', this._onClickCapture, true);
    }

    _syncAutoIndexToViewport() {
        if (!this.carouselItems || this.carouselItems.length === 0) return;
        const viewportLeft = this.carouselViewPort.scrollLeft;
        let closestIndex = 0;
        let closestDistance = Number.POSITIVE_INFINITY;
        this.carouselItems.forEach((item, idx) => {
            const dist = Math.abs(item.offsetLeft - viewportLeft);
            if (dist < closestDistance) {
                closestDistance = dist;
                closestIndex = idx;
            }
        });
        this._currentAutoIndex = closestIndex - 1; // so next advance goes to the visible one if we just started
    }

    _attachResizeObserver() {
        if (!this.carouselViewPort) return;
        if (this._resizeObserver) {
            this._resizeObserver.disconnect();
            this._resizeObserver = null;
        }
        // Observe viewport size changes
        this._resizeObserver = new ResizeObserver(() => {
            // Use rAF to avoid layout thrash if rapid
            this._scheduleRecalc();
        });
        this._resizeObserver.observe(this.carouselViewPort);
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
        this._scrollListener = () => {
            // Recalc on natural scroll events
            this._recalculateChildWidths();
        };

        this.carouselViewPort.addEventListener('scroll', this._scrollListener, { passive: true });
        this._attachDragEvents();
        this._attachResizeObserver();
        this._configureAutoPlayFromAttribute();
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
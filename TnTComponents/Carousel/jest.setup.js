// Global setup for carousel JS tests (executed via Jest setupFiles)

global.TnTComponents = { customAttribute: 'tnt-id' };

if (typeof global.requestAnimationFrame === 'undefined') {
  global.requestAnimationFrame = (cb) => setTimeout(cb, 16);
  global.cancelAnimationFrame = (id) => clearTimeout(id);
}

if (typeof global.PointerEvent === 'undefined') {
  class PointerEventPolyfill extends Event {
    constructor(type, params = {}) {
      super(type, params);
      this.button = params.button || 0;
      this.buttons = params.buttons || (this.button === 0 ? 1 : 0);
      this.clientX = params.clientX || 0;
      this.clientY = params.clientY || 0;
      this.pointerId = params.pointerId || 1;
      this.pointerType = params.pointerType || 'mouse';
    }
  }
  global.PointerEvent = PointerEventPolyfill;
}

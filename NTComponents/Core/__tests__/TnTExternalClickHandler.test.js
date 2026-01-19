import { externalClickCallbackRegister, externalClickCallbackDeregister } from '../TnTExternalClickHandler.razor.js';

// Unified DotNetObjectRef mock supporting invocation tracking
function createDotNetRef(id, onInvoke) {
  return {
    _id: id,
    invoked: [],
    invokeMethodAsync(method) {
      this.invoked.push(method);
      if (onInvoke) onInvoke(method);
      return Promise.resolve();
    }
  };
}

function createHostElement(tag = 'div') {
  const host = document.createElement(tag);
  host.id = 'host-' + Math.random();
  // Standard bounding box: (10, 10, 100, 100)
  host.getBoundingClientRect = () => ({
    left: 10, top: 10, right: 100, bottom: 100, width: 90, height: 90
  });
  const inside = document.createElement('button');
  inside.textContent = 'inside';
  host.appendChild(inside);
  document.body.appendChild(host);
  return { host, inside };
}

describe('TnTExternalClickHandler JS module', () => {
  let idCounter = 0;

  afterEach(() => {
    document.body.innerHTML = '';
  });

  // Core behavior tests
  test('does not invoke OnClick when mousedown+click occur inside element', () => {
    const { host } = createHostElement();
    let called = 0;
    const ref = createDotNetRef(++idCounter, (m) => { if (m === 'OnClick') called++; });
    externalClickCallbackRegister(host, ref);
    // Inside coordinates (50, 50)
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, clientX: 50, clientY: 50 }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true, clientX: 50, clientY: 50 }));
    expect(called).toBe(0);
    externalClickCallbackDeregister(ref);
  });

  test('invokes OnClick when mousedown+click occur outside element', () => {
    const { host } = createHostElement();
    let called = 0;
    const ref = createDotNetRef(++idCounter, (m) => { if (m === 'OnClick') called++; });
    externalClickCallbackRegister(host, ref);
    // Outside coordinates (200, 200)
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, clientX: 200, clientY: 200 }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true, clientX: 200, clientY: 200 }));
    expect(called).toBe(1);
    externalClickCallbackDeregister(ref);
  });

  test('does not invoke OnClick if mousedown was inside then click outside (drag out)', () => {
    const { host } = createHostElement();
    let called = 0;
    const ref = createDotNetRef(++idCounter, (m) => { if (m === 'OnClick') called++; });
    externalClickCallbackRegister(host, ref);
    // Mousedown inside (50, 50), Click outside (200, 200)
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, clientX: 50, clientY: 50 }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true, clientX: 200, clientY: 200 }));
    expect(called).toBe(0);
    externalClickCallbackDeregister(ref);
  });

  test('does not invoke OnClick if element removed but click occurs within element boundaries', () => {
    const { host, inside } = createHostElement();
    let called = 0;
    const ref = createDotNetRef(++idCounter, (m) => { if (m === 'OnClick') called++; });
    externalClickCallbackRegister(host, ref);
    
    // Mousedown inside child
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, clientX: 50, clientY: 50 }));
    
    // Remove the host or child (doesn't matter for position-based check, but let's remove child)
    inside.remove();
    
    // Click at position (50, 50) still inside boundaries
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true, clientX: 50, clientY: 50 }));
    
    expect(called).toBe(0);
    externalClickCallbackDeregister(ref);
  });

  test('deregister removes handlers (no further invocations)', () => {
    const { host } = createHostElement();
    let called = 0;
    const ref = createDotNetRef(++idCounter, (m) => { if (m === 'OnClick') called++; });
    externalClickCallbackRegister(host, ref);
    // Outside
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, clientX: 200, clientY: 200 }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true, clientX: 200, clientY: 200 }));
    expect(called).toBe(1);
    
    externalClickCallbackDeregister(ref);
    
    // Outside again
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, clientX: 200, clientY: 200 }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true, clientX: 200, clientY: 200 }));
    expect(called).toBe(1); // Still 1
  });
});


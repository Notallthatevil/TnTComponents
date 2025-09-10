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
    const { host, inside } = createHostElement();
    let called = 0;
    const ref = createDotNetRef(++idCounter, (m) => { if (m === 'OnClick') called++; });
    externalClickCallbackRegister(host, ref);
    inside.dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
    inside.dispatchEvent(new MouseEvent('click', { bubbles: true }));
    expect(called).toBe(0);
    externalClickCallbackDeregister(ref);
  });

  test('invokes OnClick when mousedown target is outside element', () => {
    const { host } = createHostElement();
    let called = 0;
    const ref = createDotNetRef(++idCounter, (m) => { if (m === 'OnClick') called++; });
    externalClickCallbackRegister(host, ref);
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true }));
    expect(called).toBe(1);
    externalClickCallbackDeregister(ref);
  });

  test('does not invoke OnClick if element removed after internal mousedown before click', () => {
    const { host, inside } = createHostElement();
    let called = 0;
    const ref = createDotNetRef(++idCounter, (m) => { if (m === 'OnClick') called++; });
    externalClickCallbackRegister(host, ref);
    inside.dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
    host.remove();
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true }));
    expect(called).toBe(0);
    externalClickCallbackDeregister(ref);
  });

  test('deregister removes handlers (no further invocations)', () => {
    const { host } = createHostElement();
    let called = 0;
    const ref = createDotNetRef(++idCounter, (m) => { if (m === 'OnClick') called++; });
    externalClickCallbackRegister(host, ref);
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true }));
    expect(called).toBe(1);
    externalClickCallbackDeregister(ref);
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true }));
    expect(called).toBe(1);
  });

  // Edge / additional coverage tests
  test('re-register same ref replaces prior handlers (no double fire)', () => {
    const { host } = createHostElement();
    const ref = createDotNetRef(++idCounter);
    externalClickCallbackRegister(host, ref);
    externalClickCallbackRegister(host, ref); // replace existing
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true }));
    expect(ref.invoked.filter(m => m === 'OnClick').length).toBe(1);
    externalClickCallbackDeregister(ref);
  });

  test('deregister without prior register is a no-op', () => {
    const ref = createDotNetRef(++idCounter);
    expect(() => externalClickCallbackDeregister(ref)).not.toThrow();
  });

  test('mousedown inside then click outside does not fire (tracked as internal)', () => {
    const { host, inside } = createHostElement();
    const ref = createDotNetRef(++idCounter);
    externalClickCallbackRegister(host, ref);
    inside.dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true }));
    expect(ref.invoked.includes('OnClick')).toBe(false);
    externalClickCallbackDeregister(ref);
  });

  test('no mousedown before click => treated as external (fires)', () => {
    const { host } = createHostElement();
    const ref = createDotNetRef(++idCounter);
    externalClickCallbackRegister(host, ref);
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true }));
    expect(ref.invoked.filter(m => m === 'OnClick').length).toBe(1);
    externalClickCallbackDeregister(ref);
  });

  test('element null at register still invokes on outside click (treated external)', () => {
    const ref = createDotNetRef(++idCounter);
    externalClickCallbackRegister(null, ref);
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true }));
    expect(ref.invoked.filter(m => m === 'OnClick').length).toBe(1);
    externalClickCallbackDeregister(ref);
  });

  test('element removed after outside mousedown before click still fires', () => {
    const { host } = createHostElement();
    const ref = createDotNetRef(++idCounter);
    externalClickCallbackRegister(host, ref);
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
    host.remove();
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true }));
    expect(ref.invoked.filter(m => m === 'OnClick').length).toBe(1);
    externalClickCallbackDeregister(ref);
  });

  test('multiple refs independent', () => {
    const { host: host1 } = createHostElement();
    const { host: host2 } = createHostElement();
    const ref1 = createDotNetRef(++idCounter);
    const ref2 = createDotNetRef(++idCounter);
    externalClickCallbackRegister(host1, ref1);
    externalClickCallbackRegister(host2, ref2);
    document.body.dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
    document.body.dispatchEvent(new MouseEvent('click', { bubbles: true }));
    expect(ref1.invoked.filter(m => m === 'OnClick').length).toBe(1);
    expect(ref2.invoked.filter(m => m === 'OnClick').length).toBe(1);
    externalClickCallbackDeregister(ref1);
    externalClickCallbackDeregister(ref2);
  });

  test('deregister twice is safe', () => {
    const { host } = createHostElement();
    const ref = createDotNetRef(++idCounter);
    externalClickCallbackRegister(host, ref);
    externalClickCallbackDeregister(ref);
    expect(() => externalClickCallbackDeregister(ref)).not.toThrow();
  });
});

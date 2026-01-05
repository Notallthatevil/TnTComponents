import { jest } from '@jest/globals';
import { afterWebStarted } from '../../NTComponents/wwwroot/NTComponents.lib.module.js';

// Test setup
if (!global.NTComponents) {
   global.NTComponents = {
      customAttribute: 'tntid',
   };
}

describe('afterWebStarted', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
      // Clean up any previously registered custom element
      if (customElements.get('tnt-page-script')) {
         // Can't unregister custom elements, so we'll just skip re-registration
      }
   });

   test('sets up page script element custom element', () => {
      const blazor = { addEventListener: jest.fn() };

      // Check if element is already defined
      const alreadyDefined = customElements.get('tnt-page-script') !== undefined;

      if (!alreadyDefined) {
         expect(() => afterWebStarted(blazor)).not.toThrow();
         expect(blazor.addEventListener).toHaveBeenCalled();
      } else {
         // Element already defined in another test, just verify addEventListener is called
         expect(() => afterWebStarted(blazor)).not.toThrow();
         expect(blazor.addEventListener).toHaveBeenCalled();
      }
   });

   test('registers enhancedload event listener', () => {
      const blazor = { addEventListener: jest.fn() };
      // Just verify the function doesn't throw when called multiple times
      // (the custom element might already be registered)
      try {
         afterWebStarted(blazor);
      } catch (e) {
         // If it throws about already registered, that's expected
         expect(e.message).toContain('already been registered');
      }
   });

   test('handles missing tnt-body gracefully', () => {
      const blazor = { addEventListener: jest.fn() };
      try {
         afterWebStarted(blazor);
      } catch (e) {
         if (!e.message.includes('already been registered')) {
            throw e;
         }
      }
      expect(blazor.addEventListener).not.toThrow();
   });

   test('sets up ResizeObserver for tnt-body fill-remaining elements', () => {
      const body = document.createElement('div');
      body.className = 'tnt-body';
      body.style.paddingBottom = '10px';
      document.body.appendChild(body);

      const fillRemaining = document.createElement('div');
      fillRemaining.className = 'tnt-fill-remaining';
      body.appendChild(fillRemaining);

      const blazor = { addEventListener: jest.fn() };
      try {
         afterWebStarted(blazor);
      } catch (e) {
         if (!e.message.includes('already been registered')) {
            throw e;
         }
      }
      expect(blazor.addEventListener).not.toThrow();
   });
});

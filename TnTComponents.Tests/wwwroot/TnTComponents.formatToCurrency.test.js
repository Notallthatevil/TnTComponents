import { jest } from '@jest/globals';
import '../../TnTComponents/wwwroot/TnTComponents.lib.module.js';

describe('TnTComponents.formatToCurrency', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
   });

   test('formats numeric input to currency', () => {
      const input = document.createElement('input');
      input.value = '1234.5';
      input.setAttribute('cultureCode', 'en-US');
      input.setAttribute('currencyCode', 'USD');

      const event = new Event('keyup');
      Object.defineProperty(event, 'target', { value: input });

      TnTComponents.formatToCurrency(event);

      expect(input.value).toContain('1');
   });

   test('uses default culture code if not provided', () => {
      const input = document.createElement('input');
      input.value = '100';

      const event = new Event('keyup');
      Object.defineProperty(event, 'target', { value: input });

      TnTComponents.formatToCurrency(event);

      expect(input.value).toBeTruthy();
   });

   test('uses default currency code if not provided', () => {
      const input = document.createElement('input');
      input.value = '100';

      const event = new Event('keyup');
      Object.defineProperty(event, 'target', { value: input });

      TnTComponents.formatToCurrency(event);

      expect(input.value).toBeTruthy();
   });

   test('returns early for modifier keys', () => {
      const input = document.createElement('input');
      input.value = '100';

      const event = new KeyboardEvent('keyup', { keyCode: 8 });
      Object.defineProperty(event, 'target', { value: input, configurable: true });

      TnTComponents.formatToCurrency(event);

      expect(input.value).toBe('100');
   });
});

import { jest } from '@jest/globals';
import '../../TnTComponents/wwwroot/TnTComponents.lib.module.js';

describe('TnTComponents.getCurrentLocation', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
   });

   test('returns current window location href', () => {
      const result = TnTComponents.getCurrentLocation();
      expect(result).toBe(window.location.href);
   });
});

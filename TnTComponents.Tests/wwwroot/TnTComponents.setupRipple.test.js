import { jest } from '@jest/globals';
import '../../TnTComponents/wwwroot/TnTComponents.lib.module.js';

describe('TnTComponents.setupRipple', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
   });

   test('appends tnt-ripple-effect element to all elements with tnt-ripple class', () => {
      const button1 = document.createElement('button');
      button1.className = 'tnt-ripple';
      const button2 = document.createElement('button');
      button2.className = 'tnt-ripple';
      document.body.appendChild(button1);
      document.body.appendChild(button2);

      TnTComponents.setupRipple();

      expect(button1.querySelector('tnt-ripple-effect')).not.toBeNull();
      expect(button2.querySelector('tnt-ripple-effect')).not.toBeNull();
   });

   test('does not append ripple effect if no elements with tnt-ripple class', () => {
      TnTComponents.setupRipple();
      expect(document.querySelectorAll('tnt-ripple-effect').length).toBe(0);
   });

   test('only appends to elements with tnt-ripple class', () => {
      const div = document.createElement('div');
      div.className = 'other-class';
      document.body.appendChild(div);

      TnTComponents.setupRipple();

      expect(div.querySelector('tnt-ripple-effect')).toBeNull();
   });
});

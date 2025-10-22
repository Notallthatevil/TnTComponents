import { jest } from '@jest/globals';
import '../../TnTComponents/wwwroot/TnTComponents.lib.module.js';

describe('TnTComponents.toggleSideNav', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
   });

   test('toggles tnt-toggle class on side nav toggle indicators', () => {
      const navContainer = document.createElement('div');
      navContainer.className = 'tnt-side-nav-toggle-indicator';

      const toggler = document.createElement('div');
      toggler.className = 'tnt-toggle-indicator';
      toggler.classList.add('tnt-toggle');

      navContainer.appendChild(toggler);
      document.body.appendChild(navContainer);

      const event = new Event('click');

      TnTComponents.toggleSideNav(event);

      expect(toggler.classList.contains('tnt-toggle')).toBe(false);
   });

   test('adds tnt-toggle class if not present', () => {
      const navContainer = document.createElement('div');
      navContainer.className = 'tnt-side-nav-toggle-indicator';

      const toggler = document.createElement('div');
      toggler.className = 'tnt-toggle-indicator';

      navContainer.appendChild(toggler);
      document.body.appendChild(navContainer);

      const event = new Event('click');

      TnTComponents.toggleSideNav(event);

      expect(toggler.classList.contains('tnt-toggle')).toBe(true);
   });

   test('handles multiple side nav indicators', () => {
      const navContainer1 = document.createElement('div');
      navContainer1.className = 'tnt-side-nav-toggle-indicator';
      const toggler1 = document.createElement('div');
      toggler1.className = 'tnt-toggle-indicator tnt-toggle';
      navContainer1.appendChild(toggler1);

      const navContainer2 = document.createElement('div');
      navContainer2.className = 'tnt-side-nav-toggle-indicator';
      const toggler2 = document.createElement('div');
      toggler2.className = 'tnt-toggle-indicator tnt-toggle';
      navContainer2.appendChild(toggler2);

      document.body.appendChild(navContainer1);
      document.body.appendChild(navContainer2);

      const event = new Event('click');
      TnTComponents.toggleSideNav(event);

      expect(toggler1.classList.contains('tnt-toggle')).toBe(false);
      expect(toggler2.classList.contains('tnt-toggle')).toBe(false);
   });
});

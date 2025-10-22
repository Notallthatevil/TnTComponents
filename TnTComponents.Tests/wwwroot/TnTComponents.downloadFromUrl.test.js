import { jest } from '@jest/globals';
import '../../TnTComponents/wwwroot/TnTComponents.lib.module.js';

describe('TnTComponents.downloadFromUrl', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
   });

   test('fetches blob from URL and calls downloadFileFromBlob', async () => {
      global.fetch = jest.fn().mockResolvedValue({
         blob: jest.fn().mockResolvedValue(new Blob(['test content'])),
      });

      TnTComponents.downloadFileFromBlob = jest.fn();

      await TnTComponents.downloadFromUrl('test.txt', 'https://example.com/file.txt');

      expect(fetch).toHaveBeenCalledWith('https://example.com/file.txt');
      expect(TnTComponents.downloadFileFromBlob).toHaveBeenCalled();
   });
});

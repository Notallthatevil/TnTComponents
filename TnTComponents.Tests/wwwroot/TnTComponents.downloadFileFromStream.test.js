import { jest } from '@jest/globals';
import '../../TnTComponents/wwwroot/TnTComponents.lib.module.js';

describe('TnTComponents.downloadFileFromStream', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
   });

   test('calls downloadFileFromBlob with correct parameters', async () => {
      const mockStreamRef = {
         arrayBuffer: jest.fn().mockResolvedValue(new ArrayBuffer(10)),
      };

      TnTComponents.downloadFileFromBlob = jest.fn();

      await TnTComponents.downloadFileFromStream('test.txt', mockStreamRef);

      expect(TnTComponents.downloadFileFromBlob).toHaveBeenCalled();
      const [fileName, blob] = TnTComponents.downloadFileFromBlob.mock.calls[0];
      expect(fileName).toBe('test.txt');
      expect(blob instanceof Blob).toBe(true);
   });
});

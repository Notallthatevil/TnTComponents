window.TnTComponents = {

   MediaCallbacks: { },
   MediaCallback: function(query, instance) {
      if (instance) {
         function callback(event) {
            instance.invokeMethodAsync('OnChanged', event.matches)
         };
         var query = matchMedia(query);
         this.MediaCallbacks[instance._id] = function () {
            query.removeListener(callback);
         }
         query.addListener(callback);
         return query.matches;
      } else {
         instance = query;
         if (this.MediaCallbacks[instance._id]) {
            this.MediaCallbacks[instance._id]();
            delete this.MediaCallbacks[instance._id];
         }
      }
   }
}
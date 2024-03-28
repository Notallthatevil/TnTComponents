using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Ext;
public static class NavigationManagerExt {


    public static string UpdateOrReplaceParameters(this NavigationManager navManager, IReadOnlyDictionary<string, object?> parameters) {
       return navManager.UpdateOrReplaceParameters(navManager.Uri, parameters);
    }

    public static string UpdateOrReplaceParameters(this NavigationManager navManager, string uri, IReadOnlyDictionary<string, object?> parameters) {
        var uriBuilder = new UriBuilder(uri);
        var currentParams = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

        var dictionary = new Dictionary<string, object?>(parameters);

        foreach (var key in currentParams.AllKeys) {
            if (!dictionary.ContainsKey(key)) {
                dictionary[key] = currentParams[key];
            }
        }

        return navManager.GetUriWithQueryParameters(uri, parameters);
    }

}


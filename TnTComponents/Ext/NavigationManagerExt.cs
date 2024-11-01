using Microsoft.AspNetCore.Components;
using System;

namespace TnTComponents.Ext;

public static class NavigationManagerExt {
    public static string UpdateOrReplaceParameters(this NavigationManager navManager, params KeyValuePair<string, object?>[] keyValuePairs) {
        return navManager.UpdateOrReplaceParameters(navManager.Uri, new Dictionary<string, object?>(keyValuePairs));
    }

    public static string UpdateOrReplaceParameters(this NavigationManager navManager, string uri, IReadOnlyDictionary<string, object?> parameters) {
        var uriBuilder = new UriBuilder(uri);
        var currentParams = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

        var dictionary = new Dictionary<string, object?>(parameters);

        foreach (var key in currentParams.AllKeys) {
            if (key is not null) {
                if (!dictionary.ContainsKey(key)) {
                    dictionary[key] = currentParams[key];
                }

                if (dictionary[key] is string str && string.IsNullOrWhiteSpace(str)) {
                    dictionary[key] = null;
                }
            }
        }

        return navManager.GetUriWithQueryParameters(uri, dictionary);
    }


    public static void UpdateUriWithQueryParameters(this NavigationManager navManager, string uri, IReadOnlyDictionary<string, object?> parameters) {
        var newUri = navManager.UpdateOrReplaceParameters(uri, parameters);
        if (!newUri.Equals(uri, StringComparison.OrdinalIgnoreCase)) {
            navManager.NavigateTo(newUri, replace: true);
        }
    }

    public static void UpdateUriWithQueryParameters(this NavigationManager navManager, params KeyValuePair<string, object?>[] keyValuePairs) {
        var newUri = navManager.UpdateOrReplaceParameters(keyValuePairs);
        if (!newUri.Equals(navManager.Uri, StringComparison.OrdinalIgnoreCase)) {
            navManager.NavigateTo(newUri, replace: true);
        }
    }
}
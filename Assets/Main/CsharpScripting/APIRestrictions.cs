using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SpatialSys.Client.CSharpScripting
{
    /// APIRestrictions is a static class designed to manage API method allowlisting in a Unity project.
    /// It loads a configuration file named 'csharp_api_allowlist.txt' from Unity's Resources folder.
    public class APIRestrictions
    {
        public static bool IsMethodAllowlisted(string methodFullName)
        {
            return APIRestrictions.Instance._allowlist.IsMethodAllowlisted(methodFullName);
        }

        private static APIRestrictions _instance;

        private static APIRestrictions Instance => _instance ?? (_instance = new APIRestrictions());

        private APIAllowlist _allowlist;

        private APIRestrictions()
        {
            string allowlistText = Resources.Load<TextAsset>("csharp_api_allowlist").text;
            if (allowlistText == null)
            {
                Debug.LogError("APIRestrictions: Could not load csharp_api_allowlist.txt from Resources folder.");
                return;
            }
            _allowlist = new APIAllowlist(allowlistText);
        }
    }
}

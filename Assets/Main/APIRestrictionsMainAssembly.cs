using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SpatialSys.Client.CSharpScripting
{
    /// APIRestrictionsMainAssembly is a static class designed to manage API method allowlisting in a Unity project.
    /// It loads a configuration file named 'csharp_api_allowlist.txt' from Unity's Resources folder.
    public class APIRestrictionsMainAssembly
    {

        public static bool HelloWorld()
        {
            Debug.Log("Hello, world!");
            return true;
        }
        public static bool IsMethodAllowlisted(string methodFullName)
        {
            return APIRestrictionsMainAssembly.Instance._allowlist.IsMethodAllowlisted(methodFullName);
        }

        private static APIRestrictionsMainAssembly _instance;

        private static APIRestrictionsMainAssembly Instance => _instance ?? (_instance = new APIRestrictionsMainAssembly());

        private APIAllowlistMainAssembly _allowlist;

        private APIRestrictionsMainAssembly()
        {
            string allowlistText = Resources.Load<TextAsset>("csharp_api_allowlist").text;
            if (allowlistText == null)
            {
                Debug.LogError("APIRestrictionsMainAssembly: Could not load csharp_api_allowlist.txt from Resources folder.");
                return;
            }
            _allowlist = new APIAllowlistMainAssembly(allowlistText);
        }
    }
}

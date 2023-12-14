using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace SpatialSys.Client.CSharpScripting.Tests
{
    [TestFixture]
    public class APIAllowlistManagerTests
    {
        [Test]
        public void TestRestrictions()
        {
            // method name (string), is allowlisted (bool), verified method name (string)
            var methods = new List<(string, bool, string)> {
                // We allow most APIs
                ("System.Console.WriteLine", true, $"{typeof(Console).FullName}.{nameof(Console.WriteLine)}"),

                // Disallowed:
                // UnityEngine.Application.Quit
                ("UnityEngine.Application.Quit", false, $"{typeof(Application).FullName}.{nameof(Application.Quit)}"),
                // System.Reflection*
                ("System.Reflection.TypeInfo", false, $"{typeof(System.Reflection.TypeInfo).FullName}"),
                ("System.Reflection.MethodBase.GetCurrentMethod", false, $"{typeof(MethodBase).FullName}.{nameof(MethodBase.GetCurrentMethod)}"),

            };

            foreach ((string, bool, string) method in methods)
            {
                Assert.AreEqual(method.Item1, method.Item3, $"Method {method.Item3} should be {method.Item1}");
                Assert.AreEqual(method.Item2, APIRestrictions.IsMethodAllowlisted(method.Item3), $"Method {method.Item3} should be {(method.Item2 ? "allowlisted" : "blocked")}");

            }
        }
    }
}

using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Corelib.Utils
{
    public static class AsmdefSorter
    {
        [MenuItem("Game/Project/Sort Asmdef References Alphabetically")]
        public static void SortAsmdefReferences()
        {
            string[] asmdefPaths = Directory.GetFiles("Assets", "*.asmdef", SearchOption.AllDirectories);

            foreach (string path in asmdefPaths)
            {
                string json = File.ReadAllText(path);
                var root = JObject.Parse(json);

                var refsToken = root["references"] as JArray;
                if (refsToken == null || refsToken.Count <= 1)
                    continue;

                var refs = refsToken.Select(t => t.ToString()).ToList();

                var sorted = refs
                    .OrderBy(r => GetDisplayName(r), System.StringComparer.OrdinalIgnoreCase)
                    .ToList();

                root["references"] = new JArray(sorted);
                File.WriteAllText(path, root.ToString(Newtonsoft.Json.Formatting.Indented));
                Debug.Log($"Sorted references by name in {path}");
            }

            AssetDatabase.Refresh();
        }

        static string GetDisplayName(string reference)
        {
            const string guidPrefix = "GUID:";
            if (reference.StartsWith(guidPrefix))
            {
                var guid = reference.Substring(guidPrefix.Length);
                var targetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(targetPath) || !File.Exists(targetPath))
                    return reference;

                try
                {
                    var targetJson = File.ReadAllText(targetPath);
                    var targetRoot = JObject.Parse(targetJson);
                    var name = targetRoot["name"]?.ToString();
                    return string.IsNullOrEmpty(name) ? reference : name;
                }
                catch
                {
                    return reference;
                }
            }
            return reference;
        }
    }
}

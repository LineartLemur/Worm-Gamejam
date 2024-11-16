using System;
using System.IO;
using System.Threading;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildPreprocessorVersionNumber : IPreprocessBuildWithReport {
        public void OnPreprocessBuild(BuildReport report) {
            SetVersion();
        }

        public void SetVersion() {
            DateTime d = DateTime.Now;
            var buildNumberInsert = Environment.GetEnvironmentVariable("BUILD_NUMBER");
            if (!string.IsNullOrEmpty(buildNumberInsert)) {
                buildNumberInsert = $" CI{buildNumberInsert}";
            }
            string s = $"v{Application.version}{buildNumberInsert} {d.Year.ToString().Substring(2)}.{d.Month:00}.{d.Day:00}.{d.Hour:00}.{d.Minute:00}";
            Debug.Log(s);
            File.WriteAllText("Assets/version.txt", s);
            UnityEditor.AssetDatabase.Refresh();
        }

        public int callbackOrder {
            get {
                return 0;
            }
        }
    }
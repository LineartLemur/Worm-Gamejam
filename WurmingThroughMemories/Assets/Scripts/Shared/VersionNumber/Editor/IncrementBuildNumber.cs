
using System;
using System.Threading;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEngine.Networking;

public class IncrementBuildNumber : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } } // Part of the IPreprocessBuildWithReport interface

    public void OnPreprocessBuild(BuildReport report) {
        return;
        if (report.summary.platform == BuildTarget.iOS) // Check if the build is for iOS
        {
            PlayerSettings.iOS.buildNumber = IncreasedNumber("IOS-"+Application.identifier).ToString();
            Debug.Log("increased iOS Build number to " + PlayerSettings.iOS.buildNumber);
        }

        else if (report.summary.platform == BuildTarget.Android  ) // Check if the build is for iOS
        {
            PlayerSettings.Android.bundleVersionCode = IncreasedNumber("Android-"+Application.identifier);
            Debug.Log("<color=green>increased Android Build number to " + PlayerSettings.Android.bundleVersionCode + "</color>");
        }

    }

    public int IncreasedNumber(string name) {

        Debug.Log("https://buildnumber.pepi.dev/"+name);
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://buildnumber.pepi.dev/"+name))
        {
            // Request and wait for the desired page.
            webRequest.SendWebRequest();

            int safety = 500;
            while (!webRequest.isDone) {
                Thread.Sleep(10);
                safety--;
                if (safety < 0) {
                    throw new Exception("Webrequest... timeout :c");
                }
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                throw new Exception("Webrequest... failed with error :c \n" + webRequest.error);
            }
            return Int32.Parse(webRequest.downloadHandler.text);
        }
    }
}
//
//  AppLovinPreProcessBuild.cs
//  AppLovin Unity Plugin
//
//  Created by Max Buck on 7/21/20.
//  Copyright © 2019 AppLovin. All rights reserved.
//

using System;
using System.IO;
using UnityEditor.Build;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

public class AppLovinPreProcessBuild : 
#if UNITY_2018_1_OR_NEWER
    IPreprocessBuildWithReport
#else
    IPreprocessBuild
#endif
{
    public int callbackOrder 
    { 
        get { return int.MinValue; } 
    }

#if UNITY_2018_1_OR_NEWER
    public void OnPreprocessBuild(BuildReport report)
#else
    public void OnPreprocessBuild(BuildTarget target, string path)
#endif
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var applovin = assembly.GetType("MaxSdk");
            if(applovin == null)
                continue;

            Debug.Log("MAX SDK detected. Removing standalone AppLovin SDK.");
            string applovinSDKPath = Path.Combine(Application.dataPath, "AppLovinSdk");

            AssetDatabase.StartAssetEditing();
            FileUtil.DeleteFileOrDirectory(applovinSDKPath + ".meta");
            FileUtil.DeleteFileOrDirectory(applovinSDKPath);
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();

            break;
        }
    }

}

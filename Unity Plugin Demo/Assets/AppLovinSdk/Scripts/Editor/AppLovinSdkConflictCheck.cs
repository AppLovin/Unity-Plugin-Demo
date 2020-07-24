//
//  AppLovinSdkConflictCheck.cs
//  AppLovin Unity Plugin
//
//  Created by Max Buck on 7/21/20.
//  Copyright © 2019 AppLovin. All rights reserved.
//

using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AppLovinSdkConflictCheck
{
    static AppLovinSdkConflictCheck()
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var applovin = assembly.GetType("MaxSdk");
            if(applovin == null) continue;

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

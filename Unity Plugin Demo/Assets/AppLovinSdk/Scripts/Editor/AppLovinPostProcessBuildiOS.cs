//
//  AppLovinPostProcessBuildiOS.cs
//  AppLovin Unity Plugin
//
//  Created by Thomas So on 3/17/19.
//  Copyright © 2019 AppLovin. All rights reserved.
//

#if UNITY_IOS

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public static class AppLovinPostProcessBuildiOS
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            Debug.Log("[AppLovin] Starting iOS post-process build script...");

            var projectPath = Path.Combine(buildPath, "Unity-iPhone.xcodeproj/project.pbxproj");
            var project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));

            //
            // Add the -ObjC linker flag
            //
#if UNITY_2019_3_OR_NEWER
            var target = project.GetUnityFrameworkTargetGuid();
#else
            var target = project.TargetGuidByName("Unity-iPhone");
#endif

            project.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");

            //
            // Write modified Xcode project back to original location
            //
            File.WriteAllText(projectPath, project.WriteToString());

            Debug.Log("[AppLovin] Finished iOS post-process build script...");
        }
    }
}

#endif

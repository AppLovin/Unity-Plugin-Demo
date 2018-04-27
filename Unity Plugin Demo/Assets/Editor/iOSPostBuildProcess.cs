using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class BuildPostProcessor
{
	[PostProcessBuildAttribute(1)]
	public static void OnPostProcessBuild(BuildTarget target, string path)
	{
		if (target == BuildTarget.iOS)
		{
			string projectPath = PBXProject.GetPBXProjectPath(path);
			PBXProject project = new PBXProject();
			project.ReadFromString(File.ReadAllText(projectPath));

			string targetName = PBXProject.GetUnityTargetName();
			string targetGUID = project.TargetGuidByName(targetName);

			// Add `-ObjC` to "Other Linker Flags".
			project.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ObjC");

			// Add frameworks
			project.AddFrameworkToProject(targetGUID, "AdSupport.framework", false);
			project.AddFrameworkToProject(targetGUID, "CoreTelephony.framework", false);
			project.AddFrameworkToProject(targetGUID, "StoreKit.framework", false);
			project.AddFrameworkToProject(targetGUID, "WebKit.framework", false);

			// Write
			File.WriteAllText(projectPath, project.WriteToString());
		}
	}
}

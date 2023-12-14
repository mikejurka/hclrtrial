using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallHybridCLR
{
    [UnityEditor.MenuItem("HybridCLR/Install HybridCLR from Local and Generate All")]
    public static void Install()
    {
        var installerController = new HybridCLR.Editor.Installer.InstallerController();
        Debug.Log(installerController.HasInstalledHybridCLR() ? "Re-installing HybridCLR" : "Installing HybridCLR");
        installerController.InstallFromLocal("LocalHybridCLR/il2cpp_plus_community_2021-main/libil2cpp");
        HybridCLR.Editor.Commands.PrebuildCommand.GenerateAll();
    }
}

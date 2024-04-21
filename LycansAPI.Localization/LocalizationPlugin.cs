using LycansAPI.Core;
using BepInEx;

namespace LycansAPI.Localization;

[BepInPlugin(LocalizationAPI.PLUGIN_GUID, LocalizationAPI.PLUGIN_NAME, LocalizationAPI.PLUGIN_VERSION)]
[BepInDependency(LMAPI.PLUGIN_GUID)]
[BepInProcess("Lycans.exe")]
public class LocalizationPlugin : BaseUnityPlugin
{
    private void OnEnable()
    {
        LocalizationAPI.Hook();
    }

    private void OnDisable()
    {
        LocalizationAPI.Unhook();
    }
}
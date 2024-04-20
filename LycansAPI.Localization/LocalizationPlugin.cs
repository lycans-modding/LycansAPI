using LycansAPI.Core;
using BepInEx;

namespace LycansAPI.Localization;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
[BepInDependency(LMAPI.PLUGIN_GUID)]
[BepInProcess("Lycans.exe")]
public class LocalizationPlugin : BaseUnityPlugin
{
    public const string PLUGIN_GUID = LMAPI.PLUGIN_GUID + ".localization";
    public const string PLUGIN_NAME = LMAPI.PLUGIN_NAME + ".Localization";
    public const string PLUGIN_VERSION = LMAPI.PLUGIN_VERSION;

    private void OnEnable()
    {
        LocalizationAPI.Hook();
    }

    private void OnDisable()
    {
        LocalizationAPI.Unhook();
    }
}
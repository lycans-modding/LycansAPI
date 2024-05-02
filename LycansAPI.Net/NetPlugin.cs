using BepInEx;
using LycansAPI.Core;

namespace LycansAPI.Net;

[BepInPlugin(NetAPI.PLUGIN_GUID, NetAPI.PLUGIN_NAME, NetAPI.PLUGIN_VERSION)]
[BepInDependency(LMAPI.PLUGIN_GUID)]
[BepInProcess("Lycans.exe")]
public class NetPlugin : BaseUnityPlugin
{
    private void OnDisable()
    {
        NetAPI.Instance.ClearNetworkObjects();
    }
}
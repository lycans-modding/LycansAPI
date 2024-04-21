using BepInEx;
using LycansAPI.Core;

namespace LycansAPI.State;

[BepInPlugin(StateAPI.PLUGIN_GUID, StateAPI.PLUGIN_NAME, StateAPI.PLUGIN_VERSION)]
[BepInDependency(LMAPI.PLUGIN_GUID)]
[BepInProcess("Lycans.exe")]
public class StatePlugin : BaseUnityPlugin
{
    private void OnEnabled()
    {
        StateAPI.Hook();
    }

    private void OnDisabled()
    {
        StateAPI.Unhook();
    }

}
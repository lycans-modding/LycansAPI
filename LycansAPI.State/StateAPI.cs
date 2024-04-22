using LycansAPI.Core;

namespace LycansAPI.State;

public class StateAPI
{
    public const string PLUGIN_GUID = LMAPI.PLUGIN_GUID + ".state";
    public const string PLUGIN_NAME = LMAPI.PLUGIN_NAME + ".State";
    public const string PLUGIN_VERSION = LMAPI.PLUGIN_VERSION;

    internal static StateAPI Instance
    {
        get 
        {
            _instance ??= new StateAPI();
            return _instance;
        }
    }

    private static StateAPI? _instance;

    internal void Hook()
    {
        On.GameState.Spawned += GameState_Spawned;
    }

    internal void Unhook()
    {
        On.GameState.Spawned -= GameState_Spawned;
    }

    private void GameState_Spawned(On.GameState.orig_Spawned orig, GameState self)
    {
        orig(self);
    }
}
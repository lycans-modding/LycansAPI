using LycansAPI.Core;

namespace LycansAPI.State;

public static class StateAPI
{
    public const string PLUGIN_GUID = LMAPI.PLUGIN_GUID + ".state";
    public const string PLUGIN_NAME = LMAPI.PLUGIN_NAME + ".State";
    public const string PLUGIN_VERSION = LMAPI.PLUGIN_VERSION;

    internal static void Hook()
    {
        On.GameState.Spawned += GameState_Spawned;
    }

    internal static void Unhook()
    {
        On.GameState.Spawned -= GameState_Spawned;
    }

    private static void GameState_Spawned(On.GameState.orig_Spawned orig, GameState self)
    {
        orig(self);
    }
}
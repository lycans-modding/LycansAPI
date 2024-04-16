using BepInEx;
using UnityEngine;

namespace LycansAPI.Core;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
[BepInProcess("Lycans.exe")]
public class LMAPI : BaseUnityPlugin
{
    public const string PLUGIN_GUID = "fr.lycans.modding.api";
    public const string PLUGIN_NAME = "LycansAPI";
    public const string PLUGIN_VERSION = "1.0.0";

    private const string GAME_BUILD = "0.11.1";

    internal static LMAPI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Log.Init(Logger);
    }

    private void Start()
    {
        CheckGameVersion();
    }

    private void CheckGameVersion()
    {
        var buildId = Application.version;

        if (GAME_BUILD.Equals(buildId)) return;

        Log.Warning($"The API was built for version '{GAME_BUILD}', you are running version '{buildId}'");
        Log.Warning("Should there be any bug with mods, check if there is any update to the API.");
    }
}
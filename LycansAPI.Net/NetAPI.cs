using Fusion;
using LycansAPI.Core;
using LycansAPI.Net.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace LycansAPI.Net;

public class NetAPI
{
    public const string PLUGIN_GUID = LMAPI.PLUGIN_GUID + ".net";
    public const string PLUGIN_NAME = LMAPI.PLUGIN_NAME + ".Net";
    public const string PLUGIN_VERSION = LMAPI.PLUGIN_VERSION;

    /// <summary>
    /// Enregistre un GameObject comme NetworkObject afin qu'il soit synchronisé par
    /// Fusion entre le serveur et les clients.
    /// 
    /// Le prefab passé en paramètre ne doit pas être supprimé car il sert de "référence" afin
    /// de faire apparaître une version networké de l'objet.
    /// </summary>
    /// <param name="prefab">Un GameObject de Unity avec un composant NetworkObject et des NetworkBehaviour</param>
    /// <param name="uniqueKey">Une clée d'identification unique à votre objet qui sera utilisée pour synchroniser</param>
    /// <returns>
    /// L'identifiant network du GameObject créé, utilisé par Fusion avec la fonction Runner.Spawn()
    /// ou null si l'objet n'a pas pu être enregistré.
    /// </returns>
    public static NetworkPrefabId? RegisterNetworkObject(GameObject prefab, string uniqueKey)
    {
        if (Instance._networkedPrefabs.ContainsKey(uniqueKey))
        {
            Core.Log.Error($"NetworkObject with key '{uniqueKey}' was already registered! Please use another one.");
            return null;
        }

        if (!prefab.TryGetComponent<NetworkObject>(out var netObj))
        {
            Core.Log.Warning($"Prefab with key '{uniqueKey}' is missing 'NetworkObject' component, adding it for you.");
            netObj = prefab.AddComponent<NetworkObject>();
        }

        netObj.NetworkedBehaviours = prefab.GetComponents<NetworkBehaviour>();
        netObj.NetworkGuid = new NetworkObjectGuid(uniqueKey.ToGuid().ToString());

        var source = new NetworkPrefabSourceStatic()
        {
            PrefabReference = netObj,
        };

        if (!NetworkProjectConfig.Global.PrefabTable.TryAdd(netObj.NetworkGuid, source, out var id))
        {
            Core.Log.Error($"Prefab with key '{uniqueKey}' couldn't be added to prefab table! Your object won't be networked!");
            return null;
        }

        return id;
    }

    /// <summary>
    /// Récupère l'identifiant network d'un GameObject à partir d'une clée unique.
    /// </summary>
    /// <param name="uniqueKey">La clée d'identification unique à votre objet</param>
    /// <returns>
    /// L'identifiant network du GameObject créé, utilisé par Fusion avec la fonction Runner.Spawn()
    /// ou null si l'objet n'a pas pu être trouvé.
    /// </returns>
    public static NetworkPrefabId? TryGetNetworkObject(string uniqueKey)
    {
        if (!Instance._networkedPrefabs.TryGetValue(uniqueKey, out var netObj))
        {
            Core.Log.Error($"No prefab registered with '{uniqueKey}' as identifier!");
            return null;
        }
        
        return netObj;
    }

    internal static NetAPI Instance
    {
        get
        {
            _instance ??= new NetAPI();
            return _instance;
        }
    }

    internal void ClearNetworkObjects()
        => _networkedPrefabs.Clear();

    private static NetAPI? _instance;
    private Dictionary<string, NetworkPrefabId> _networkedPrefabs = new();
}
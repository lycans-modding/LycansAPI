# LycansAPI.Net

## À propos

Module qui permet d'assurer la synchronisation des objets networkés entre le
serveur et les clients.

## Utilisation

Afin d'utiliser ce module, il faut dans un premier temps créer un nouvel objet
Unity qui possède un ou plusieurs `NetworkBehaviour` et un composant de
type `NetworkObject`. Il suffit ensuite d'enregistrer cet objet avec l'API afin
que ce dernier puisse être spawn en jeu.

**Attention : Le `GameObject` créé ne doit pas être supprimé !**

Exemple :

```cs
// Fichier : MyNetworkedComponent.cs
// Plus de doc sur Fusion : https://doc.photonengine.com/fusion/v1/fusion-intro

[NetworkBehaviourWeaved(1)]
internal class MyNetworkedComponent : NetworkBehaviour
{
    [DefaultForProperty(nameof(MyNetworkedBool), 0, 1)]
    [SerializeField]
    private bool _myNetworkedBool;

    [Networked]
    [NetworkedWeaved(0, 1)]
    public unsafe bool MyNetworkedBool
    {
        get
        {
            if (Ptr == null)
            {
                throw new InvalidOperationException("Error when accessing MyNetworkedComponent.MyNetworkedBool. Networked properties can only be accessed when Spawned() has been called.");
            }
            return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
        }
        private set
        {
            if (Ptr == null)
            {
                throw new InvalidOperationException("Error when accessing MyNetworkedComponent.MyNetworkedBool. Networked properties can only be accessed when Spawned() has been called.");
            }
            ReadWriteUtilsForWeaver.WriteBoolean((int*)((byte*)Ptr + 0), value);
        }
    }

    public override void Spawned(NetworkRunner runner)
    {
        Log.Debug("NetworkedObject Spawned");
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        Log.Debug("NetworkedObject Despawned");
        Destroy(gameObject);
    }
}
```

```cs
// Fichier : MyPlugin.cs

// <headers requis>
public class MyPlugin : BaseUnityPlugin
{
    // <Guid, auteur, nom...>

    public void Start()
    {
        var prefab = new GameObject("MyNetworkedObject");
        // Optionnel car sera ajouté automatiquement si manquant mais
        // affichera alors un avertissement dans la console
        prefab.AddComponent<NetworkObject>();
        prefab.AddComponent<MyNetworkedComponent>();
        NetAPI.RegisterNetworkObject(prefab, "MyPluginGUID.MyNetworkedObject");
        
        // Important pour que la copie de référence ne soit pas supprimée !
        DontDestroyOnLoad(prefab);
    }
}
```

```cs
// Ailleurs dans le code...
// Par exemple dans un patch de la méthode GameManager.Spawned

// <code>

private static void GameManager_Spawned(On.GameManager.orig_Spawned orig, GameManager self)
{
    orig(self);

    if (self.Session.IsOpen && self.Runner.IsServer)
    {
        var netObjId = NetAPI.TryGetNetworkObject("MyPluginGUID.MyNetworkedObject");
        
        // Demande à Fusion de créer une nouvelle instance à partir du prefab qu'on
        // a enregistré précédemment afin qu'il soit synchronisé avec les autres
        // joueurs.
        var netObj = self.Runner.Spawn(netObjId);
    }
}

// <code>

```
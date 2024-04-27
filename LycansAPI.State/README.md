# LycansAPI.State

## À propos

Module qui permet de hook les différents états du jeu (les différentes étapes d'un round par exemple)

## Utilisation

Le module remplace les fonctions de la state machine du jeu afin d'y injecter du code avant et après.
Il est techniquement possible de hook tous les états de la state machine. Par contre, Lycans n'utilise que les états suivants par défaut
dans la fonction `GameState.Spawned()`

```
- Pregame -> onEnter
- Play -> onEnter
- Transition -> onEnter / onExit
- Meeting -> onEnter / onExit
- EndGame -> onEnter
```

### Exemple de pre hook

Un pre hook permet d'exécuter du code avant celui de base du jeu. Il permet également de ne pas exécuter le code d'origine du jeu.

Attention cependant, il est souvent préférable de ne pas passer l'exécution du code du jeu, car celà passe également les post hooks et réduit
également la compatibilité avec les autres mods. Idéalement, vous devriez passer le code du jeu uniquement si vous remplacez intégralement
son fonctionnement.

```cs
StateAPI.AddPreEnterHook(GameState.EGameState.Play, (GameState instance, GameState.EGameState previousState) => 
{
    Log.Info("Je m'exécute AVANT le code du jeu quand on rentre dans un round de jeu");

    if (instance.Runner.IsServer)
    {
        Log.Info("Je suis exécuté uniquement par le serveur");
    }

    if (instance.Runner.IsPlayer)
    {
        Log.Info("Je suis exécuté par un joueur");
    }

    // Ne passe pas l'exécution du code du jeu
    return false;
});
```

### Exemple de post hook

Un post hook permet d'exécuter du code après celui de base du jeu. Il permet de modifier une partie du comportement du jeu, notamment en remplaçant
des valeurs ou alors en ajoutant de nouveaux comportements.

```cs
StateAPI.AddPostExitHook(GameState.EGameState.Meeting, (GameState instance, GameState.EGameState previousState) => 
{
    Log.Info("Je m'exécute APRÈS le code du jeu quand on quitte un vote");

    if (instance.Runner.IsServer)
    {
        Log.Info("Je suis exécuté uniquement par le serveur");
    }

    if (instance.Runner.IsPlayer)
    {
        Log.Info("Je suis exécuté par un joueur");
    }
});
```
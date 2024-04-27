# LycansAPI.State

## � propos

Module qui permet de hook les diff�rents �tats du jeu (les diff�rentes �tapes d'un round par exemple)

## Utilisation

Le module remplace les fonctions de la state machine du jeu afin d'y injecter du code avant et apr�s.
Il est techniquement possible de hook tous les �tats de la state machine. Par contre, Lycans n'utilise que les �tats suivants par d�faut
dans la fonction `GameState.Spawned()`

```
- Pregame -> onEnter
- Play -> onEnter
- Transition -> onEnter / onExit
- Meeting -> onEnter / onExit
- EndGame -> onEnter
```

### Exemple de pre hook

Un pre hook permet d'ex�cuter du code avant celui de base du jeu. Il permet �galement de ne pas ex�cuter le code d'origine du jeu.

Attention cependant, il est souvent pr�f�rable de ne pas passer l'ex�cution du code du jeu, car cel� passe �galement les post hooks et r�duit
�galement la compatibilit� avec les autres mods. Id�alement, vous devriez passer le code du jeu uniquement si vous remplacez int�gralement
son fonctionnement.

```cs
StateAPI.AddPreEnterHook(GameState.EGameState.Play, (GameState instance, GameState.EGameState previousState) => 
{
    Log.Info("Je m'ex�cute AVANT le code du jeu quand on rentre dans un round de jeu");

    if (instance.Runner.IsServer)
    {
        Log.Info("Je suis ex�cut� uniquement par le serveur");
    }

    if (instance.Runner.IsPlayer)
    {
        Log.Info("Je suis ex�cut� par un joueur");
    }

    // Ne passe pas l'ex�cution du code du jeu
    return false;
});
```

### Exemple de post hook

Un post hook permet d'ex�cuter du code apr�s celui de base du jeu. Il permet de modifier une partie du comportement du jeu, notamment en rempla�ant
des valeurs ou alors en ajoutant de nouveaux comportements.

```cs
StateAPI.AddPostExitHook(GameState.EGameState.Meeting, (GameState instance, GameState.EGameState previousState) => 
{
    Log.Info("Je m'ex�cute APR�S le code du jeu quand on quitte un vote");

    if (instance.Runner.IsServer)
    {
        Log.Info("Je suis ex�cut� uniquement par le serveur");
    }

    if (instance.Runner.IsPlayer)
    {
        Log.Info("Je suis ex�cut� par un joueur");
    }
});
```
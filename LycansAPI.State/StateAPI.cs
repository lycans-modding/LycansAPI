using LycansAPI.Core;
using LycansAPI.Core.Extensions;
using LycansAPI.State.Structures;
using System;

namespace LycansAPI.State;

public class StateAPI
{
    public const string PLUGIN_GUID = LMAPI.PLUGIN_GUID + ".state";
    public const string PLUGIN_NAME = LMAPI.PLUGIN_NAME + ".State";
    public const string PLUGIN_VERSION = LMAPI.PLUGIN_VERSION;

    public enum HookedEvent
    {
        Enter,
        Exit
    }

    /// <summary>
    /// Fonction qui permet d'exécuter du code avant le code de l'état qui est hook.
    /// Si la fonction renvoie "true" la fonction d'origine et des hooks "post" ne
    /// sont pas exécutés.
    /// </summary>
    /// <param name="state">L'état que l'on souhaite hook</param>
    /// <param name="hook">
    /// Fonction qui sera exécutée avant le code d'origine, 
    /// elle prend en entrée l'instance du GameState, l'état précédent et renvoie un booléen
    /// </param>
    /// <param name="evt">Détermine quel état est hook entre Enter et Exit</param>
    public static void AddPreHook(GameState.EGameState state, Func<GameState, GameState.EGameState, bool> hook, HookedEvent evt)
    {
        if (evt == HookedEvent.Enter)
            Instance._hooks[state].preOnEnter.Add(hook);
        else
            Instance._hooks[state].preOnExit.Add(hook);
    }

    /// <summary>
    /// Fonction qui permet d'exécuter du code après le code de l'état qui est hook.
    /// </summary>
    /// <param name="state">L'état que l'on souhaite hook</param>
    /// <param name="hook">
    /// Fonction qui sera exécutée après le code d'origine, 
    /// elle prend en entrée l'instance du GameState, l'état précédent
    /// </param>
    public static void AddPostHook(GameState.EGameState state, Action<GameState, GameState.EGameState> hook, HookedEvent evt)
    {
        if (evt == HookedEvent.Enter)
            Instance._hooks[state].postOnEnter.Add(hook);
        else
            Instance._hooks[state].postOnExit.Add(hook);
    }

    /// <summary>
    /// Fonction qui permet d'exécuter du code avant le code de l'état qui est hook.
    /// Si la fonction renvoie "true" la fonction d'origine et des hooks "post" ne
    /// sont pas exécutés.
    /// </summary>
    /// <param name="state">L'état que l'on souhaite hook</param>
    /// <param name="hook">
    /// Fonction qui sera exécutée avant le code d'origine, 
    /// elle prend en entrée l'instance du GameState, l'état précédent et renvoie un booléen
    /// </param>
    public static void AddPreEnterHook(GameState.EGameState state, Func<GameState, GameState.EGameState, bool> hook)
        => AddPreHook(state, hook, HookedEvent.Enter);

    /// <summary>
    /// Fonction qui permet d'exécuter du code avant le code de l'état qui est hook.
    /// Si la fonction renvoie "true" la fonction d'origine et des hooks "post" ne
    /// sont pas exécutés.
    /// </summary>
    /// <param name="state">L'état que l'on souhaite hook</param>
    /// <param name="hook">
    /// Fonction qui sera exécutée avant le code d'origine, 
    /// elle prend en entrée l'instance du GameState, l'état précédent et renvoie un booléen
    /// </param>
    public static void AddPreExitHook(GameState.EGameState state, Func<GameState, GameState.EGameState, bool> hook)
        => AddPreHook(state, hook, HookedEvent.Exit);

    /// <summary>
    /// Fonction qui permet d'exécuter du code après le code de l'état qui est hook.
    /// </summary>
    /// <param name="state">L'état que l'on souhaite hook</param>
    /// <param name="hook">
    /// Fonction qui sera exécutée après le code d'origine, 
    /// elle prend en entrée l'instance du GameState, l'état précédent
    /// </param>
    public static void AddPostEnterHook(GameState.EGameState state, Action<GameState, GameState.EGameState> hook)
        => AddPostHook(state, hook, HookedEvent.Enter);

    /// <summary>
    /// Fonction qui permet d'exécuter du code après le code de l'état qui est hook.
    /// </summary>
    /// <param name="state">L'état que l'on souhaite hook</param>
    /// <param name="hook">
    /// Fonction qui sera exécutée après le code d'origine, 
    /// elle prend en entrée l'instance du GameState, l'état précédent
    /// </param>
    public static void AddPostExitHook(GameState.EGameState state, Action<GameState, GameState.EGameState> hook)
        => AddPostHook(state, hook, HookedEvent.Exit);

    internal static StateAPI Instance
    {
        get 
        {
            _instance ??= new StateAPI();
            return _instance;
        }
    }

    private static StateAPI? _instance;
    private ExtendedStateMachine<GameState.EGameState, GameState> _hooks = new();

    internal void Hook()
    {
        On.GameState.Spawned += GameState_Spawned;
    }

    internal void Unhook()
    {
        On.GameState.Spawned -= GameState_Spawned;
    }

    private Action<GameState.EGameState> HookState(Action<GameState.EGameState> orig, GameState instance, GameState.EGameState currentState, HookedEvent evt)
    {
        var preHooks = evt == HookedEvent.Enter ? Instance._hooks[currentState].preOnEnter : Instance._hooks[currentState].preOnExit;
        var postHooks = evt == HookedEvent.Enter ? Instance._hooks[currentState].postOnEnter : Instance._hooks[currentState].postOnExit;

        return (GameState.EGameState previousState) =>
        {
            bool shouldSkip = false;
            preHooks.ForEach(hook =>
            {
                shouldSkip &= hook.Invoke(instance, previousState);
            });

            if (shouldSkip) return;

            orig(previousState);

            postHooks.ForEach(hook => hook.Invoke(instance, previousState));
        };
    }

    private void GameState_Spawned(On.GameState.orig_Spawned orig, GameState self)
    {
        orig(self);

        self.StateMachine._states.Keys.ForEach(state =>
        {
            self.StateMachine[state].onEnter = HookState(self.StateMachine[state].onEnter, self, state, HookedEvent.Enter);
            self.StateMachine[state].onExit = HookState(self.StateMachine[state].onExit, self, state, HookedEvent.Exit);
        });
    }
}
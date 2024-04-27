using System;
using System.Collections.Generic;

namespace LycansAPI.State.Structures
{
    internal class ExtendedStateMachine<T, U>
    {
        public class StateHooks
        {
            public List<Func<U, T, bool>> preOnEnter = new();
            public List<Action<U, T>> postOnEnter = new();

            public List<Func<U, T, bool>> preOnExit = new();
            public List<Action<U, T>> postOnExit = new();
        }

        public StateHooks this[T state]
        {
            get
            {
                if (!_hooks.TryGetValue(state, out var hook))
                {
                    hook = new StateHooks();
                    _hooks.Add(state, hook);
                }

                return hook;
            }
        }

        private readonly Dictionary<T, StateHooks> _hooks = new();
    }
}
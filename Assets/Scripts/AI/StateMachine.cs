using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState curState;

    private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> curTransitions = new List<Transition>();
    private List<Transition> anyTransitions = new List<Transition>();

    private List<IState> allPosibleStates;

    private static readonly List<Transition> EmptyTransitions = new List<Transition>(0);

    public override string ToString()
    {
        string _msg = $"cur state: {curState?.GetType()} | anyTrans: ";
        foreach (var trans in anyTransitions)
        {
            _msg += trans?.ToString() + "|";
        }
        _msg += " curTrans: ";
        foreach (var trans in curTransitions)
        {
            _msg += trans?.ToString() + "|";
        }
        return _msg;
    }
    public void Tick(float _delta)
    {
        var transition = GetTransition();
        if (transition != null)
        {
            SetState(transition.To);
        }

        curState?.Tick(_delta);
    }

    public void SetState(IState _state)
    {
        if (_state == curState)
        {
            return;
        }
        curState?.OnExit();
        curState = _state;

        transitions.TryGetValue(curState.GetType(), out curTransitions);
        if (curTransitions == null)
        {
            curTransitions = EmptyTransitions;
        }

        curState.OnEnter();
        Debug.Log(ToString());
    }

    public void AddTransition(IState _from, IState _to, Func<bool> _condition)
    {
        TryAddState(_from);
        TryAddState(_to);
        if (transitions.TryGetValue(_from.GetType(), out var newTransitions) == false)
        {
            newTransitions = new List<Transition>();
            transitions[_from.GetType()] = newTransitions;
        }

        newTransitions.Add(new Transition(_to, _condition));
    }

    public void AddAnyTransition(IState _state, Func<bool> _condition)
    {
        TryAddState(_state);
        anyTransitions.Add(new Transition(_state, _condition));
    }

    private void TryAddState(IState _state)
    {
        if (allPosibleStates == null)
        {
            allPosibleStates = new List<IState>();
        }

        if (!allPosibleStates.Contains(_state))
        {
            allPosibleStates.Add(_state);
        }
    }

    private class Transition
    {
        public Func<bool> Condition
        {
            get;
        }
        public IState To
        {
            get;
        }

        public Transition(IState _to, Func<bool> _condition)
        {
            To = _to;
            Condition = _condition;
        }

        public override string ToString()
        {
            return $"to: {To.GetType()}";
        }
    }

    private Transition GetTransition()
    {
        foreach (var transition in anyTransitions)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        foreach (var transition in curTransitions)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        return null;
    }
}
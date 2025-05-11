using Godot;
using System;
using System.Collections.Generic;

public partial class CardStateMachine : Node
{
    public enum CardStates { NONE, DEFAULT, CLICKED, DRAGGING, AIMING, RELEASED}
    [Export] CardInteractState initialState;

    private CardInteractState currentState;
    private List<CardInteractState> cardStates = new List<CardInteractState>();

    public CardStateMachine(CardUI cardUI)
    {
        InitializeStateMachine(cardUI);
    }

    public void InitializeStateMachine(CardUI cardUI)
    {
        this.cardStates.Add(new CardDefaultState(CardStates.DEFAULT));
        this.cardStates.Add(new CardClickedState(CardStates.CLICKED));
        this.cardStates.Add(new CardDraggingState(CardStates.DRAGGING));
        this.cardStates.Add(new CardAimingState(CardStates.AIMING));
        this.cardStates.Add(new CardReleasedState(CardStates.RELEASED));

        foreach (CardInteractState cardStates in this.cardStates)
        {
            cardStates.cardUI = cardUI;
            cardStates.ChangeState += OnStateChanged;
        }

        initialState = this.cardStates[0];
        currentState = initialState;
        currentState.Enter();
    }

    public void OnInput(InputEvent inputEvent)
    {
        if(currentState != null)
        {
            currentState.OnInput(inputEvent);
        }
    }

    public void OnGuiInput(InputEvent inputEvent)
    {
        if(currentState != null)
        {
            currentState.OnGuiInput(inputEvent);
        }
    }

    public void OnMouseEntered()
    {
        if(currentState != null)
        {
            currentState.OnMouseEntered();
        }
    }

    public void OnMouseExited()
    {
        if(currentState != null)
        {
            currentState.OnMouseExited();
        }
    }

    public void OnStateChanged(CardInteractState from, int to)
    {
        //GD.Print(from.state.ToString() + " -> " + ((CardStates)to).ToString());
        if(from != currentState)
            return;

        CardInteractState newstate = cardStates.Find(x => x.state == (CardStates)to);
        if(newstate == null)
            return;

        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newstate;
        newstate.Enter();
    }
}
using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class _CLASS_ : EventData
{
    public override void InitializeEvent()
    {
        this.choices.Add(new Choice("This is button text", "this adds to the body"));
    }
}

public partial class Choice : EventChoice
{
    public Choice(string body, string outcome) : base(body, outcome)
    {
        this.body = body;
        this.outcomeText = outcome;
    }

    public override void Outcome()
    {
        //Effect
    }
}

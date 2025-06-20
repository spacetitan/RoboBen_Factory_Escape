using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class EventData : Resource
{
    public enum EventID { NONE, MOVE_COMBAT, MOVE_TREASURE, MOVE_REST, MOVE_SHOP, WATERFALL, SHRINE, HEALTH_SHRINE};
    
    [Export] public EventID id { get; private set; } = EventID.NONE;
    [Export] public int tier { get; private set; } = 0;
    [Export] public float weight { get; private set; } = 0.0f;
    [Export] public string title { get; private set; } = "";
    [Export(PropertyHint.MultilineText)] public string body { get; private set; } = "";
    [Export] public Texture2D texture { get; private set; } = null;
    [Export] public bool canRepeat { get; private set; } = false;
    [Export] public bool isTrapped { get; private set; } = false;
    
    public List<EventChoice> choices { get; private set; } = new List<EventChoice>();
    
    public float accumilatedWeight = 0.0f;
    
    public void SetData(EventData eventData)
    {
        this.id = eventData.id;
        this.tier = eventData.tier;
        this.weight = eventData.weight;
        this.title = eventData.title;
        this.body = eventData.body;
        this.texture = eventData.texture;
        this.canRepeat = eventData.canRepeat;
        this.isTrapped = eventData.isTrapped;
        
        this.accumilatedWeight = eventData.accumilatedWeight;

        foreach (EventChoice choice in eventData.choices)
        {
            this.choices.Add(choice);
        }
    }
    
    public virtual void InitializeEvent() { }

    public virtual void Enter()
    {
        foreach (EventChoice choice in this.choices)
        {
            choice.Enter();
        }
    }

    public virtual void Exit() { }
}

public partial class EventChoice
{
    public string body = "";
    public string outcomeText = "";
    public bool isDisabled = false;

    public EventChoice(string body, string outcome, bool isDisabled = false)
    {
        this.body = body;
        this.outcomeText = outcome;
        this.isDisabled = isDisabled;
    }
    
    public virtual void Enter() { }

    public virtual void Outcome() { }
}

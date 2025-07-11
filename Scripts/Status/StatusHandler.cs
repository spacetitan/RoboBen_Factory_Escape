using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StatusHandler : RefCounted
{
    [Signal] public delegate void StatusesAppliedEventHandler(Status.TriggerType type);
    private PackedScene statusUIScene = ResourceLoader.Load<PackedScene>("res://Scenes/UI/status_ui.tscn");
    public List<Status> statuses { get; private set; } = new List<Status>();
    public List<StatusUI> statusUIs { get; private set; } = new List<StatusUI>();
    const float STATUS_APPLY_INTERVAL = .25f;
    public Character owner { get; private set; }
    public GridContainer container { get; private set; }

    public StatusHandler(Character owner)
    {
        this.owner = owner;
    }

    public void SetContainer(GridContainer container)
    {
	    this.container = container;
    }

    public bool HasStatus(Status.StatusID id)
	{
		foreach(Status status in this.statuses)
		{
			if(status.id == id)
			{
				return true;
			}
		}

		return false;
	}
    
	public bool HasStatusUI(Status.StatusID id)
	{
		foreach(StatusUI status in this.statusUIs)
		{
			if(status.status.id == id)
			{
				return true;
			}
		}

		return false;
	}

	private Status GetStatus(Status.StatusID id)
	{
		foreach(Status status in this.statuses)
		{
			if(status.id == id)
			{
				return status;
			}
		}

		GD.Print("No Status Present");
		return null;
	}
	
	private StatusUI GetStatusUI(Status.StatusID id)
	{
		foreach(StatusUI UI in this.statusUIs)
		{
			if(UI.status.id == id)
			{
				return UI;
			}
		}

		GD.Print("No Status Present");
		return null;
	}

	public List<Status> GetAllStatuses()
	{
		List<Status> statuses = new List<Status>();

		foreach(Status status in this.statuses)
		{
			statuses.Add(status);
		}

		return statuses;
	}
	
	public void SpawnUI(Status status)
	{
		StatusUI ui = this.statusUIScene.Instantiate() as StatusUI;
		this.container.AddChild(ui);
		ui.GetSceneNodes();
		ui.SetData(status);
		ui.SetCustomMinimumSize(new Vector2(this.container.GetRect().Size.X / 4, this.container.GetRect().Size.X / 4));
		this.statusUIs.Add(ui);
	}
	
	public void ClearUI()
	{
		foreach (StatusUI status in this.statusUIs)
		{
			status.QueueFree();
		}
		this.statusUIs.Clear();
	}

    public void AddStatus(Status status)
	{
		if(status.stackType == Status.StackType.NONE)
		{
 			GD.Print("No stack type set for status");
			return;
		}

		if(!HasStatus(status.id))
		{
			status.InitializeStatus(this.owner);
            status.StatusApplied += OnStatusApplied;
            this.statuses.Add(status);
            SpawnUI(status);
			return;
		}

		if(!status.canExpire && status.stackType == Status.StackType.NONE) { return; }

		if(status.canExpire && status.stackType == Status.StackType.DURATION)
		{
			GetStatus(status.id).AddDuration(status.duration);
		}

		if(status.canExpire && status.stackType == Status.StackType.INTENSITY)
		{
			GetStatus(status.id).AddStacks(status.stacks);
		}
	}

    public void ApplyStatusByType(Status.TriggerType triggerType, Action callback = null)
	{
		if(triggerType == Status.TriggerType.EVENT) { return; }

		List<Status> statuses = GetAllStatuses().FindAll(x => x.triggerType == triggerType);

		if(statuses == null || !statuses.Any())
		{
			EmitSignal(SignalName.StatusesApplied, (int)triggerType);
			callback?.Invoke();
			return;
		}

		Tween tween = owner.CreateTween();
		foreach (Status status in statuses)
		{
			tween.TweenCallback(Callable.From(() => { status.ApplyStatus(owner); }));
			tween.TweenInterval(STATUS_APPLY_INTERVAL);
		}

		tween.Finished += () =>
		{
			callback?.Invoke();
			EmitSignal(SignalName.StatusesApplied, (int)triggerType);
		};
	}

    private void OnStatusApplied(Status status)
	{
		if(status.canExpire)
		{
			status.SetDuration(status.duration-1);
            status.SetStacks(status.stacks-1);

            if((status.stackType == Status.StackType.INTENSITY && status.stacks <= 0) 
            || (status.stackType == Status.StackType.DURATION && status.duration <= 0))
            {
                status.StatusApplied -= OnStatusApplied;
                RemoveStatus(status.id);
            }
		}
	}

	public void RemoveStatus(Status.StatusID id)
	{
		if (HasStatus(id))
		{
			Status status = GetStatus(id);
			StatusUI ui = GetStatusUI(id);
			
			this.statuses.Remove(status);
			ui.DestroyUI();
		}
	}
}

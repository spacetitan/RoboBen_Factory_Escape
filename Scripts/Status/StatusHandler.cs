using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StatusHandler : RefCounted
{
    [Signal] public delegate void StatusesAppliedEventHandler(Status.TriggerType type);
    public List<Status> statuses { get; private set; } = new List<Status>();
    const float STATUS_APPLY_INTERVAL = .25f;
    public Character owner { get; private set; }

    public StatusHandler(Character owner)
    {
        this.owner = owner;
    }

    private bool HasStatus(StringName id)
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

	private Status GetStatus(StringName id)
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

	public List<Status> GetAllStatuses()
	{
		List<Status> statuses = new List<Status>();

		foreach(Status status in this.statuses)
		{
			statuses.Add(status);
		}

		return statuses;
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
            status.StatusApplied += OnStatusApplied;
            this.statuses.Add(status);
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

    public void ApplyStatusByType(Status.TriggerType triggerType)
	{
		if(triggerType == Status.TriggerType.EVENT) { return; }

		List<Status> statuses = GetAllStatuses().FindAll(x => x.triggerType == triggerType);

		if(statuses == null || !statuses.Any())
		{
			EmitSignal(SignalName.StatusesApplied, (int)triggerType);
			return;
		}

		Tween tween = owner.CreateTween();
		foreach (Status status in statuses)
		{
			tween.TweenCallback(Callable.From(() => { status.ApplyStatus(owner); }));
			tween.TweenInterval(STATUS_APPLY_INTERVAL);
		}

		tween.Finished += () => {EmitSignal(SignalName.StatusesApplied, (int)triggerType); };
	}

    private void OnStatusApplied(Status status)
	{
		if(status.canExpire)
		{
			status.SetDuration(status.duration-1);
            status.SetStacks(status.stacks-1);

            if((status.stackType == Status.StackType.INTENSITY && status.stacks < 0) 
            || (status.stackType == Status.StackType.DURATION && status.duration < 0))
            {
                status.StatusApplied -= OnStatusApplied;
                this.statuses.Remove(status);
            }
		}
	}
}

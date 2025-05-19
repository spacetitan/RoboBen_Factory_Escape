using Godot;
using System;
using System.Collections.Generic;

public partial class RoomData : Node
{
    public enum Type{ NONE, COMBAT, SHOP, REST, EVENT, TREASURE };
    [Export] public Type type;
    [Export] public int row;
    [Export] public int column;
    [Export] public bool selected = false;
    public List<int> nextRoomNumber = new List<int>();
    public BattleData battleData = null;
    public EventData eventData = null;
    
    public String toString()
    {
        return "Room " + this.row.ToString() + " - " + this.column.ToString();
    }
    
    public Godot.Collections.Dictionary<StringName, Variant> saveRoomData()
    {
        Godot.Collections.Dictionary<StringName, Variant> data = new Godot.Collections.Dictionary<StringName, Variant>()
        {
            {"Room Type", (int)this.type},
            {"Row", this.row},
            {"column", this.column},
        };

        Godot.Collections.Dictionary<StringName, Variant> nextRoomData = new Godot.Collections.Dictionary<StringName, Variant>();
        if (nextRoomNumber.Count > 0)
        {
            for(int i = 0; i < this.nextRoomNumber.Count; i++)
            {
                nextRoomData.Add("Next Room " + i, this.nextRoomNumber[i]);
            }
        }
        data.Add("Next Rooms", nextRoomData);

        if (this.battleData != null)
        {
            data.Add("Battle Data", (int) this.battleData.battleID);
        }
        else
        {
            data.Add("Battle Data", -1);
        }

        if (this.eventData != null)
        {
            data.Add("Event Data", (int) this.eventData.id);
        }
        else
        {
            data.Add("Event Data", -1);
        }

        return data;
    }
}

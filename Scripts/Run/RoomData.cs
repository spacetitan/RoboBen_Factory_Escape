using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

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
            {"Column", this.column},
            {"Selected", this.selected},
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

    public void LoadRoomData(Godot.Collections.Dictionary data)
    {
        int roomType = (int) data["Room Type"];
        this.type = (Type) roomType;
        this.row = (int) data["Row"];
        this.column = (int) data["Column"];
        this.selected = (bool) data["Selected"];

        Dictionary nextRoomData = (Dictionary) data["Next Rooms"];
        foreach (KeyValuePair<Variant, Variant> roomData in nextRoomData)
        {
            this.nextRoomNumber.Add((int) roomData.Value);
        }

        int battleID = (int) data["Battle Data"];
        if (battleID != -1)
        {
            this.battleData = ResourceManager.instance.battles[(BattleData.BattleID) battleID];   
        }
        
        int eventID = (int) data["Event Data"];
        if (eventID != -1)
        {
            this.eventData = ResourceManager.instance.events[(EventData.EventID) eventID];   
        }
    }
}

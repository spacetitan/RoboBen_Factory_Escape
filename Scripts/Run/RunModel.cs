using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class RunModel : UIModel
{
    private PackedScene MAP_ROOM = ResourceLoader.Load<PackedScene>("res://Scenes/Run/room_panel.tscn");

    private List<RoomPanel> roomList = new List<RoomPanel>();
    private List<RoomPanel> selectedRooms = new List<RoomPanel>();
    private int floorsClimbed = 0;
    private RoomPanel currentRoom = null;
    private RoomPanel lastRoom = null;
    
    private ScrollContainer mapScrollContainer = null;
    private VBoxContainer mapContainer = null;

    public override void _Ready()
    {
        GetSceneNodes();
    }

    private void GetSceneNodes()
    {
        this.mapScrollContainer = GetNode<ScrollContainer>("%MapScrollContainer");
        this.mapScrollContainer.ClipContents = false;
        // this.mapScrollContainer.GetVScrollBar().Changed += () =>
        // {
        //     this.mapScrollContainer.ScrollVertical = (int)this.mapScrollContainer.GetVScrollBar().MaxValue; 
        // };

        this.mapContainer = this.mapScrollContainer.GetNode<VBoxContainer>("%MapContainer");
    }
    
    public override void Enter()
    {
        if (this.roomList == null || this.roomList.Count == 0)
        {
            CreateMap();
        }
        else
        {
            
        }
        // if(this.mapData == null || !this.mapData.Any())
        // {
        //     GenerateNewMap();
        //     UnlockFloor(this.floorsClimbed);
        // }
        // else
        // {
        //     this.floorsClimbed = RunManager.instance.currentRun.floorsClimbed;
        //     this.lastRoom = RunManager.instance.currentRun.lastRoom;
        //     UnlockNextRooms();
			     //
        //     double max = this.scrollContainer.GetVScrollBar().GetMax() - (this.scrollContainer.Size.Y);
        //     int num = (int)(max * FloorToScroll());
        //     this.scrollContainer.SetVScroll(num);
				    //
        //     Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Linear);
        //     tween.TweenProperty(this.scrollContainer, "scroll_vertical", num, 1f);
        // }
        // GameManager.instance.SaveGame();
    }
    
    void CreateMap()
    {
        foreach(List<RoomData> floors in RunManager.instance.currentRun.mapData)
        {
            HBoxContainer boxContainer = new HBoxContainer();
            boxContainer.Alignment = BoxContainer.AlignmentMode.Center;
            boxContainer.SetAnchorsAndOffsetsPreset(LayoutPreset.Center);
            boxContainer.AddThemeConstantOverride("separation", RunManager.instance.currentRun.rng.RandiRange(50,125)); 
            this.mapContainer.AddChild(boxContainer);
            this.mapContainer.MoveChild(boxContainer, 0);

            foreach(RoomData room in floors)
            {
                if(room.nextRoomNumber.Any())
                {
                    SpawnRoom(room, boxContainer);
                }
            }
        }

        int middle = Mathf.FloorToInt(MapGenerator.MAP_WIDTH/2);
        SpawnRoom(RunManager.instance.currentRun.mapData[RunManager.instance.mapGenerator.floors-1][middle], this.mapContainer.GetChild(0) as HBoxContainer);
        GetTree().CreateTimer(.01f).Timeout += ConnectLines;
    }
    
    void SpawnRoom(RoomData room, HBoxContainer floor)
    {
        RoomPanel roomPanel = this.MAP_ROOM.Instantiate() as RoomPanel;
        roomPanel.GetSceneNodes();
        roomPanel.SetRoom(room);
        roomPanel.PivotOffset = new Vector2(roomPanel.GetRect().Size.X/2, roomPanel.GetRect().Size.Y/2);
        this.roomList.Add(roomPanel);
        floor.AddChild(roomPanel);
    }
    
    void ConnectLines()
    {
        foreach(RoomPanel roomPanel in this.roomList)
        {
            foreach(int nextRoomNum in roomPanel.data.nextRoomNumber)
            {
                Line2D mapLine = new Line2D();
                mapLine.SetZIndex(-1);
                roomPanel.AddChild(mapLine);
                mapLine.AddPoint(roomPanel.PivotOffset);

                RoomPanel nextMapRoom = GetRoomPanel(roomPanel.data.row+1, nextRoomNum);

                HBoxContainer thisFloor = roomPanel.GetParent() as HBoxContainer;
                HBoxContainer nextFloor = nextMapRoom.GetParent() as HBoxContainer;
				
                Vector2 position = new Vector2(nextMapRoom.Position.X - roomPanel.Position.X + roomPanel.PivotOffset.X, nextFloor.Position.Y - thisFloor.Position.Y + roomPanel.PivotOffset.Y);
                mapLine.AddPoint(position);
            }
        }
    }
    
    public RoomPanel GetRoomPanel(RoomData data)
    {
        foreach(RoomPanel roomPanel in this.roomList)
        {
            if(roomPanel.data == data)
            {
                return roomPanel;
            }
        }

        GD.Print("returning null");
        return null;
    }
    
    public RoomPanel GetRoomPanel(int row, int column)
    {
        foreach(RoomPanel roomPanel in this.roomList)
        {
            if(roomPanel.data.row == row && roomPanel.data.column == column)
            {
                return roomPanel;
            }
        }

        GD.Print("returning null");
        return null;
    }
    
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MapView : UIView
{
    private PackedScene MAP_ROOM = ResourceLoader.Load<PackedScene>("res://Scenes/Run/room_panel.tscn");
    
    private ScrollContainer mapScrollContainer = null;
    private VBoxContainer mapContainer = null;
    private VBoxContainer legendContainer = null;
    private UIButton closeButton = null;
    
    private float offset = 0f;
    private int floorsClimbed = 0;
    private RoomPanel lastRoom = null;
    
    List<List<RoomData>> mapData = new List<List<RoomData>>();
    private List<RoomPanel> roomList = new List<RoomPanel>();
    
    private Vector2 panelSize = Vector2.Zero;
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.mapScrollContainer = GetNode<ScrollContainer>("%MapScrollContainer");
        this.mapScrollContainer.ClipContents = true;
        this.mapScrollContainer.GetVScrollBar().Changed += () =>
        {
            this.mapScrollContainer.ScrollVertical = (int)this.mapScrollContainer.GetVScrollBar().MaxValue; 
        };

        this.mapContainer = this.mapScrollContainer.GetNode<VBoxContainer>("%MapContainer");
        
        this.legendContainer = this.mapScrollContainer.GetNode<VBoxContainer>("%LegendContainer");
        List<UIDisplay> displayList = new List<UIDisplay>();
        foreach (UIDisplay display in this.legendContainer.GetChildren())
        {
            displayList.Add(display);
        }
        displayList[0].SetData("Battle", ResourceManager.instance.runIcons[RoomData.Type.COMBAT]);
        displayList[1].SetData("Treasure", ResourceManager.instance.runIcons[RoomData.Type.TREASURE]);
        displayList[2].SetData("Rest", ResourceManager.instance.runIcons[RoomData.Type.REST]);
        displayList[3].SetData("Shop", ResourceManager.instance.runIcons[RoomData.Type.SHOP]);
        displayList[4].SetData("Event", ResourceManager.instance.runIcons[RoomData.Type.EVENT]);
        
        this.closeButton = this.GetNode<UIButton>("%CloseButton");
        this.closeButton.SetData("Close");
        this.closeButton.button.Pressed += () =>
        {
            UIManager.instance.popUpModel.ClosePopup(UIModel.ViewID.MAP);
        };
        
        this.panelSize = new Vector2(this.mapScrollContainer.Size.X/ 6f, this.mapScrollContainer.Size.X / 6f);
    }
    
    public float FloorToScroll()
    {
        return ((float)(RunManager.instance.mapGenerator.floors - RunManager.instance.currentRun.floorsClimbed) / (float)RunManager.instance.mapGenerator.floors);
    }

    public void OpenPopUp()
    {
        InitializeMap();
        this.ShowView();
    }
    
    public override void Exit()
    {
        ClearMap();
    }
    
    public void InitializeMap()
    {
        this.mapData = RunManager.instance.currentRun.mapData;
        RunModel model = UIManager.instance.models[UIManager.UIState.RUN] as RunModel;
        if(this.mapData != null || this.mapData.Any())
        {
            this.floorsClimbed = RunManager.instance.currentRun.floorsClimbed;
            CreateMap();
            UnlockNextRooms();
        }
    }
    
    void CreateMap()
    {
        foreach(List<RoomData> floors in RunManager.instance.currentRun.mapData)
        {
            HBoxContainer boxContainer = new HBoxContainer();
            boxContainer.Alignment = BoxContainer.AlignmentMode.Center;
            boxContainer.SetAnchorsAndOffsetsPreset(LayoutPreset.Center);
            boxContainer.AddThemeConstantOverride("separation", 10); 
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
        GetTree().CreateTimer(.1f).Timeout += ConnectLines;
    }
    
    void SpawnRoom(RoomData room, HBoxContainer floor)
    {
        RoomPanel roomPanel = this.MAP_ROOM.Instantiate() as RoomPanel;
        roomPanel.GetSceneNodes();
        roomPanel.SetRoom(room, true);
        roomPanel.SetCustomMinimumSize(this.panelSize);
        roomPanel.PivotOffset = new Vector2(this.panelSize.X/2, this.panelSize.Y/2);
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
                mapLine.Width = 5;
                roomPanel.AddChild(mapLine);
                mapLine.AddPoint(roomPanel.PivotOffset);
                mapLine.TextureMode = Line2D.LineTextureMode.Tile;
                mapLine.Width = 5;
                mapLine.Texture = ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.LINE];

                RoomPanel nextMapRoom = GetRoomPanel(roomPanel.data.row+1, nextRoomNum);

                HBoxContainer thisFloor = roomPanel.GetParent() as HBoxContainer;
                HBoxContainer nextFloor = nextMapRoom.GetParent() as HBoxContainer;
				
                Vector2 position = new Vector2((nextMapRoom.Position.X - roomPanel.Position.X) + roomPanel.PivotOffset.X, nextFloor.Position.Y - thisFloor.Position.Y + roomPanel.PivotOffset.Y);
                mapLine.AddPoint(position);
            }
        }
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
    
    void UnlockFloor(int whichFloor = -1)
    {
        if(whichFloor == -1)
        {
            whichFloor = RunManager.instance.currentRun.floorsClimbed;
        }

        foreach(RoomPanel room in this.roomList)
        {
            if(room.data.row == whichFloor)
            {
                room.SetAvailability(true);
            }
            else
            {
                room.SetAvailability(false);
            }
        }
    }
    
    void UnlockNextRooms()
    {
        foreach(RoomPanel room in this.roomList)
        {
            if(room.data.selected)
            {
                room.ShowSelected();
            }
        }
    }
    
    public void ClearMap()
    {
        foreach (Node node in this.mapContainer.GetChildren())
        {
            node.QueueFree();
        }
        this.roomList.Clear();
    }
}

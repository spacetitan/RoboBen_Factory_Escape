using Godot;
using System;

public partial class RoomPanel : Panel
{
    [Signal] public delegate void RoomSelectedEventHandler(RoomData room);
    [Export] public TextureRect texture;
    [Export] public AnimationPlayer animationPlayer;

    bool available = false;
    private bool isDisplay = false;
    public RoomData data = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.texture = this.GetNode<TextureRect>("%RoomTexture");
        this.animationPlayer = GetNode<AnimationPlayer>("%RoomAP");
    }
    
    public void OnInputEvent(InputEvent inputEvent)
    {
        if(inputEvent.IsActionPressed("LeftMouse") && this.available && !isDisplay)
        {
            this.data.selected = true;
            this.animationPlayer.Play("select");
            return;
        }
    }
    
    public void OnMapRoomSelected()
    {
        RunManager.instance.SelectRoom(this.data);
    }
    
    public void SetRoom(RoomData room, bool display = false)
    {
        this.data = room;
        this.texture.Texture = ResourceManager.instance.runIcons[room.type];
        this.isDisplay = display;

        if (room.selected)
        {
            this.animationPlayer.Play("selected");
        }
    }
    
    // public void PlayAnimation(string anim)
    // {
    //     this.animationPlayer.Play(anim);
    // }
    //
    // public void SetAvailability(bool availability)
    // {
    //     this.available = availability;
    //
    //     if(this.available)
    //     {
    //         this.animationPlayer.Play("highlight");
    //     }
    //     else
    //     {
    //         this.animationPlayer.Play("RESET");
    //     }
    // }
    //
    // public void ShowSelected()
    // {
    //     this.line2D.Modulate = new Godot.Color(255,255,255);
    // }
}

using Godot;
using System;

public partial class RoomPanel : Panel
{
    [Signal] public delegate void RoomSelectedEventHandler(RoomData room);
    [Export] public TextureRect texture;
    [Export] public AnimationPlayer animationPlayer;

    public bool isAvailable { get; private set; } = false;
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
        if(inputEvent.IsActionPressed("LeftMouse") && this.isAvailable && !isDisplay)
        {
            EmitSignal(SignalName.RoomSelected, this.data);
            this.data.selected = true;
            this.animationPlayer.Play("select");
            AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.ROOM_SELECTED], true);
            this.isAvailable = false;
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
        SetAvailability(false);
    }
    
    public void SetAvailability(bool availability)
    {
        this.isAvailable = availability;
    
        if(this.isAvailable)
        {
            this.animationPlayer.Play("highlight");
        }
        else
        {
            this.animationPlayer.Play("RESET");
        }
    }
    
    public void ShowSelected()
    {
        this.texture.Texture = ResourceManager.instance.runGlowIcons[this.data.type];
        this.animationPlayer.Play("selected");
    }

    public void OnMouseEntered()
    {
        if (!this.isDisplay && this.isAvailable)
        {
            this.texture.Texture = ResourceManager.instance.runGlowIcons[this.data.type];
        }
    }

    public void OnMouseExited()
    {
        if (!this.isDisplay && !this.data.selected && this.isAvailable)
        {
            this.texture.Texture = ResourceManager.instance.runIcons[this.data.type];
        }
    }
}

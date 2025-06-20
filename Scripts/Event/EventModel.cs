using Godot;
using System;

public partial class EventModel : UIModel
{
    public RichTextLabel titleLabel = null;
    public RichTextLabel bodyLabel = null;
    public VBoxContainer choicesContainer = null;
    public TextureRect eventTexture = null;
    public Control playerSpawn = null;
    public EventData eventData { get; private set; } = null;
    
    public Vector2 playerSize { get; private set; } = Vector2.Zero;

    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        Panel panel = GetNode<Panel>("%TextPanel");
        this.titleLabel = panel.GetNode<RichTextLabel>("%TitleLabel");
        this.bodyLabel = panel.GetNode<RichTextLabel>("%BodyLabel");
        this.eventTexture = this.GetNode<TextureRect>("%EventTexture");
        this.playerSpawn = this.GetNode<Control>("%PlayerSpawn");
        this.playerSize = new Vector2(this.playerSpawn.Size.X, this.playerSpawn.Size.X);
        this.choicesContainer = this.GetNode<VBoxContainer>("%ChoicesContainer");
    }

    public void SetData(EventData eventData)
    {
        this.eventData = new EventData();
        this.eventData.SetData(eventData);
        this.eventData.Enter();

        this.titleLabel.Text = eventData.title;
        this.bodyLabel.Text = eventData.body;
        this.eventTexture.Texture = eventData.texture;
        
        RoomHUDView roomHUD = UIManager.instance.hudModel.views[ViewID.ROOM_HUD] as RoomHUDView;

        foreach (EventChoice choice in this.eventData.choices)
        {
            UIButton button = ResourceManager.instance.uiButtonScene.Instantiate() as UIButton;
            button.GetSceneNodes();
            button.SetData(choice.body);
            button.button.Pressed += () =>
            {
                choice.Outcome();
                this.bodyLabel.Text += "\n\n" + choice.outcomeText;
                OnButtonPressed();
            };
            this.choicesContainer.AddChild(button);
            button.SetCustomMinimumSize(roomHUD.leaveButton.Size);
            button.label.AddThemeFontSizeOverride("normal_font_size", 24);
        }

        if (this.eventData.isTrapped)
        {
            roomHUD.ToggleLeaveButton(false);
        }

        Player player = ResourceManager.instance.playerScene.Instantiate() as Player;
        player.GetSceneNodes();
        playerSpawn.AddChild(player);
        player.SetCustomMinimumSize(this.playerSize);
        player.SetPlayerData(RunManager.instance.currentRun);
    }
    
    public void OnButtonPressed()
    {
        int count = 0;
        foreach (UIButton button in this.choicesContainer.GetChildren())
        {
            if (!this.eventData.canRepeat)
            {
                button.button.Disabled = true;
            }
            else if (this.eventData.choices[count].isDisabled)
            {
                button.button.Disabled = true;
            }

            button.SetData(this.eventData.choices[count].body);

            count++;
        }

        RoomHUDView roomHUD = UIManager.instance.hudModel.views[ViewID.ROOM_HUD] as RoomHUDView;
        roomHUD.ToggleLeaveButton(true);
        roomHUD.UpdateData();
    }

    public override void Enter()
    {
        UIManager.instance.vfxModel.OpenCurtain();
    }

    public override void Exit()
    {
        if (this.playerSpawn.GetChildren().Count > 0)
        {
            Player player = this.playerSpawn.GetChild(0) as Player;
            player.DestroyPlayer();
        }

        foreach (Node node in this.choicesContainer.GetChildren())
        {
            node.QueueFree();
        }
    }
}

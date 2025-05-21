using Godot;
using System;

public partial class CharPickerPanel : Panel
{
    private RichTextLabel titleLabel = null;
    private TextureRect pickerTexture = null;

    public Button pickerButton = null;
    public PlayerData playerData { get; private set; } = null;
    public EnemyData enemyData { get; private set; } = null;

    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.titleLabel = this.GetNode<Panel>("%TitlePanel").GetNode<RichTextLabel>("%TitleLabel");
        this.pickerTexture = this.GetNode<TextureRect>("%PickerTexture");
        
        this.pickerButton = this.GetNode<Button>("%PickerButton");
    }

    public void SetData(PlayerData playerData)
    {
        this.playerData = playerData;
        
        this.titleLabel.Text = playerData.name;
        this.pickerTexture.Texture = playerData.texture;
    }
    
    public void SetData(EnemyData enemyData)
    {
        this.enemyData = enemyData;
        
        this.titleLabel.Text = enemyData.name;
        this.pickerTexture.Texture = enemyData.texture;
    }
}

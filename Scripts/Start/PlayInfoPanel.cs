using Godot;
using System;

public partial class PlayInfoPanel : Control
{
    public struct InfoData()
    {
        public Texture2D texture = null;
        public string title = "";
        public string body = "";
        public string type = "";
    }

    private TextureRect infoTexture = null;
    private RichTextLabel titleLabel = null;
    private RichTextLabel bodyLabel = null;
    private RichTextLabel typeLabel = null;
    
    public override void _Ready() { }

    public void GetSceneNodes()
    {
        this.infoTexture = this.GetNode<TextureRect>("%InfoTexture");
        this.titleLabel = this.GetNode<RichTextLabel>("%TitleLabel");
        this.bodyLabel = this.GetNode<RichTextLabel>("%BodyLabel");
        this.typeLabel = this.GetNode<Panel>("%TypePanel").GetNode<RichTextLabel>("%TypeLabel");
    }

    public void SetData(InfoData data)
    {
        this.infoTexture.Texture = data.texture;
        this.titleLabel.Text = data.title;
        this.bodyLabel.Text = data.body;
        this.typeLabel.Text = data.type;
    }

    public void ResetData()
    {
        this.infoTexture.Texture = ResourceManager.instance.debugIcon;
        this.titleLabel.Text = "";
        this.bodyLabel.Text = "";
        this.typeLabel.Text = "";
    }
}

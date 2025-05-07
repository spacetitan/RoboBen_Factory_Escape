using Godot;
using System;

public partial class UIButton : Control
{
    public Button button = null;
    public TextureRect texture = null;
    public RichTextLabel label = null;

    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.button = GetNode<Button>("%Button");
        
        HBoxContainer uiContainer = GetNode<HBoxContainer>("%UIContainer");
        this.texture = uiContainer.GetNode<TextureRect>("ButtonTexture");
        this.label = uiContainer.GetNode<RichTextLabel>("ButtonLabel");
    }

    public void SetData(string text, Texture2D texture = null)
    {
        if (text != null)
        {
            this.label.Text = text;
            this.label.Show();
        }

        if (texture != null)
        {
            this.texture.Texture = texture;
            this.texture.Show();
        }
    }
}

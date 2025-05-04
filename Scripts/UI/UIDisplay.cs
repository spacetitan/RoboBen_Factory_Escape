using Godot;
using System;

public partial class UIDisplay : Panel
{
    private TextureRect displayTexture = null;
    private RichTextLabel displayText = null;
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        HBoxContainer container = this.GetNode<HBoxContainer>("%DisplayContainer");
        this.displayTexture = container.GetNode<TextureRect>("%DisplayTexture");
        this.displayText = container.GetNode<RichTextLabel>("%DisplayLabel");
    }

    public void SetData(string text, Texture2D texture = null)
    {
        if (texture != null)
        {
            this.displayTexture.Texture = texture;
            this.displayTexture.Show();
        }

        if (text != null)
        {
            this.displayText.Text = text;
            this.displayText.Show();
        }
    }
}

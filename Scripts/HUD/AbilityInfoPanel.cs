using Godot;
using System;

public partial class AbilityInfoPanel : UIView
{
    private TextureRect abilityTexture = null;
    private RichTextLabel abilityTitle = null;
    private RichTextLabel abilityDesc = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.abilityTexture = this.GetNode<TextureRect>("%AbilityTexture");
        this.abilityTitle = this.GetNode<RichTextLabel>("%TitleLabel");
        this.abilityDesc = this.GetNode<RichTextLabel>("%DescLabel");
    }

    public void SetData(PlayerData data)
    {
        this.abilityTexture.Texture = data.ability.texture;
        this.abilityTitle.Text = data.ability.name;
        this.abilityDesc.Text = data.ability.desc;
    }
}

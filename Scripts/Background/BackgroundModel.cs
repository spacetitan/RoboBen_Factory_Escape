using Godot;
using System;

public partial class BackgroundModel : UIModel
{
    private TextureRect backgroundTexture = null;
    
    [Export] private Texture2D[] backgroundTextures = null;

    public override void _Ready()
    {
        GetSceneNodes();
    }
    
    public override void InitializeModel()
    {
        foreach (Control control in this.GetChildren())
        {
            if (control is UIView)
            {
                UIView view = control as UIView;
                view.InitializeView(this);
                this.views.Add(view.ID, view);
            }
        }
    }

    public void GetSceneNodes()
    {
        this.backgroundTexture = this.GetNode<TextureRect>("%BackgroundTexture");
    }

    public void ChangeBackgroundTexture(UIManager.UIState state)
    {
        switch (state)
        {
            case UIManager.UIState.START:
                this.backgroundTexture.Texture = backgroundTextures[0];
                break;
            
            case UIManager.UIState.BATTLE:
            case UIManager.UIState.TREASURE:
            case UIManager.UIState.REST:
            case UIManager.UIState.SHOP:
            case UIManager.UIState.EVENT:
                this.backgroundTexture.Texture = backgroundTextures[1];
                break;
            
            case UIManager.UIState.RUN:
                this.backgroundTexture.Texture = backgroundTextures[2];
                break;
            
            case UIManager.UIState.NONE:
            default:
                
                break;
        }
    }
}

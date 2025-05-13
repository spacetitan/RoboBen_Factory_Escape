using Godot;
using System;

public partial class StatusUI : Panel
{
    public TextureRect statusTexture = null;
    public RichTextLabel statusLabel = null;
    public PowerUpInfo infoPanel = null;
    public PowerUpInfo infoPanelInv = null;
    public Status status { get; private set; }
    private bool isInv = false;
    
    public override void _Ready()
    {
        
    }

    public void GetSceneNodes()
    {
        this.statusTexture = GetNode<TextureRect>("%StatusTexture");
        this.statusLabel = GetNode<RichTextLabel>("%StatusLabel");
        this.infoPanel = GetNode<PowerUpInfo>("%InfoPanel");
        this.infoPanelInv = GetNode<PowerUpInfo>("%InfoPanelInv");
    }

    public void SetData(Status status, bool inv = false)
    {
        this.status = status;
        this.status.StatusChanged += UpdateUI;
        
        this.statusTexture.Texture = this.status.statusIcon;
        
        if (status.stackType == Status.StackType.INTENSITY)
        {
            this.statusLabel.Text = this.status.stacks.ToString();
        }
        else if (status.stackType == Status.StackType.DURATION)
        {
            this.statusLabel.Text = this.status.duration.ToString();
        }
        
        this.infoPanel.SetData(status);
        this.infoPanelInv.SetData(status);
        this.isInv = inv;
    }
    
    public void UpdateUI()
    {
        this.statusTexture.Texture = this.status.statusIcon;
        
        if(!this.status.canExpire) { return; }

        if (status.stackType == Status.StackType.INTENSITY)
        {
            this.statusLabel.Text = this.status.stacks.ToString();

            if(this.status.stacks < 1)
            {
                DestroyUI();
            }
        }
        else if (status.stackType == Status.StackType.DURATION)
        {
            this.statusLabel.Text = this.status.duration.ToString();

            if(this.status.duration < 1)
            {
                DestroyUI();
            }
        }
        
        this.infoPanel.SetData(status);
        this.infoPanelInv.SetData(status);
    }
    
    public void OnMouseEntered()
    {
        if (this.isInv)
        {
            this.infoPanelInv.ShowView();
        }
        else
        {
            this.infoPanel.ShowView();
        }
    }

    public void OnMouseExited()
    {
        this.infoPanel.HideView();
        this.infoPanelInv.HideView();
    }

    public void DestroyUI()
    {
        this.status.StatusChanged -= UpdateUI;
        this.QueueFree();
    }
}

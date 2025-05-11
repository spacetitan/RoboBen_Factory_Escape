using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TargetSelector : Control
{
    const int ARC_POINTS = 9;

    private TextureRect targetSprite;
    private Area2D targetArea;
    private Line2D cardArc;
    
    private List<Character> targets = new List<Character>();

    private CardUI currentCard;
    private Player player;
    bool targeting = false;
    
    public override void _Ready()
    {
        GetSceneNodes();

        EventManager.instance.CardAimStarted += OnAimStarted;
        EventManager.instance.AbilityAimStarted += OnAimStarted;
        EventManager.instance.AimEnded += OnAimEnded;
    }

    private void GetSceneNodes()
    {
        this.targetSprite = GetNode<TextureRect>("%SelectorTexture");
        this.targetArea = GetNode<Area2D>("%SelectorArea2D");
        this.cardArc = GetNode<Line2D>("%SelectorLine2D");
        
        this.SetPivotOffset(new Vector2(this.Size.X / 2, this.Size.Y / 2));
        
        CircleShape2D circleShape = new CircleShape2D();
        circleShape.SetRadius(this.Size.X / 2);
        this.targetArea.GetNode<CollisionShape2D>("CollisionShape2D").SetShape(circleShape);
    }
    
    public override void _Process(double delta)
    {
        if(!targeting)
        {
            return;
        }

        //this.Position = GetGlobalMousePosition();

        this.targetSprite.Position = GetLocalMousePosition() - this.GetPivotOffset();
        this.targetArea.Position = GetLocalMousePosition();
        cardArc.Points = GetPoints();
    }
    
    Vector2[] GetPoints()
    {
        List<Vector2> points = new List<Vector2>();

        Vector2 start = new Vector2();
        
        if (this.currentCard != null)
        {
            start = this.currentCard.GlobalPosition - this.GlobalPosition;
            start += this.currentCard.GetPivotOffset();
        }
        else if(this.player != null)
        {
            start = this.player.GlobalPosition - this.GlobalPosition;
            start += this.player.GetPivotOffset();
        }
        
         
        Vector2 target = GetGlobalMousePosition() - this.GlobalPosition;
        Vector2 distance = target - start;

        for(int i = 0; i < ARC_POINTS; i++)
        {
            float t = 1.0f/ARC_POINTS * i;
            float x = start.X + distance.X / ARC_POINTS * i;
            float y = start.Y + EaseOutCubic(t) * distance.Y;
            points = points.Append(new Vector2(x,y)).ToList();
        }

        points.Append(target);

        return points.ToArray();
    }
    
    float EaseOutCubic(float number)
    {
        return 1.0f - MathF.Pow(1.0f - number, 3.0f);
    }
    
    void OnAimStarted(CardUI card)
    {
        if(!card.data.isSingleTargeted())
        {
            return;
        }

        this.targeting = true;
        this.targetArea.Monitoring = true;
        this.targetArea.Monitorable = true;
        this.targetSprite.Visible = true;
        currentCard = card;
    }
    
    void OnAimStarted(Player player)
    {
        this.targeting = true;
        this.targetArea.Monitoring = true;
        this.targetArea.Monitorable = true;
        this.targetSprite.Visible = true;
        this.player = player;
    }

    void OnAimEnded()
    {
        this.targeting = false;
        this.cardArc.ClearPoints();
        this.targetArea.Position = Vector2.Zero;
        this.targetArea.Monitoring = false;
        this.targetArea.Monitorable = false;
        this.targetSprite.Visible = false;
        this.currentCard = null;
        this.player = null;
    }

    void OnArea2DEntered(Area2D area)
    {
        if(!targeting)
        {
            return;
        }

        if (area.IsInGroup("Enemy"))
        {
            Character enemy = area.GetParent<Character>();
            
            if(!this.targets.Contains(enemy))
            {
                this.targets.Add(enemy);
                enemy.ShowTargeted(true);

                if (this.currentCard != null)
                {
                    this.currentCard.targets = this.targets;
                    this.currentCard.AddThemeStyleboxOverride("panel", this.currentCard.cardStylePlay);
                }
            }
        }

        if (area.IsInGroup("Burner"))
        {
            if (this.currentCard != null)
            {
                this.currentCard.SetBurner(true);
            }
        }
    }
    void OnArea2DExited(Area2D area)
    {
        if(!targeting)
        {
            return;
        }
        
        if (area.IsInGroup("Enemy"))
        {
            Character enemy = area.GetParent<Character>();
            if(this.targets.Contains(enemy))
            {
                this.targets.Remove(enemy);
                enemy.ShowTargeted(false);
                this.currentCard.AddThemeStyleboxOverride("panel", this.currentCard.cardstyleDrag);
                
                if (this.currentCard != null)
                {
                    this.currentCard.targets = this.targets;
                }
            }
        }
        
        if (area.IsInGroup("Burner"))
        {
            if (this.currentCard != null)
            {
                this.currentCard.SetBurner(false);
            }
        }
    }
    
    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionReleased("LeftMouse") && this.player != null)
        {
            this.player.playerData.ability.ApplyEffects(this.targets, this.player.playerData);
            OnAimEnded();
            this.targets.Clear();
        }
    }
}

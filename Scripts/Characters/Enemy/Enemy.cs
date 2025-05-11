using Godot;
using System;

public partial class Enemy : Character
{
    public EnemyData data { get; private set; } = null;
    
    private TextureRect texture = null;
    public TextureRect targetTexture = null;
    public TextureRect armorTexture = null;
    public RichTextLabel armorLabel = null;
    public TextureRect intentTexture = null;
    public RichTextLabel intentLabel = null;
    public Area2D enemyArea { get; private set; } = null;
    public HealthUI healthUI { get; private set; } = null;
    
    [Export] private EnemyActionHandler actionHandler = null;
    [Export] public EnemyAction currentAction { get; private set; } = null;
    
    public void GetSceneNodes()
    {
        this.texture = this.GetNode<TextureRect>("%EnemyTexture");
        this.targetTexture = this.GetNode<TextureRect>("%TargetTexture");
        this.targetTexture.Hide();
        this.armorTexture = this.GetNode<TextureRect>("%ArmorTexture");
        this.armorLabel = this.armorTexture.GetNode<RichTextLabel>("%ArmorLabel");
        this.intentTexture = this.GetNode<TextureRect>("%IntentTexture");
        this.intentLabel = this.GetNode<RichTextLabel>("%IntentLabel");
        this.enemyArea = this.GetNode<Area2D>("%TargetArea2D");
        this.healthUI = this.GetNode<HealthUI>("%HealthUI");
    }

    public void SetData(EnemyData enemyData)
    {
        this.data = enemyData;
        this.data.SetHealth(this.data.maxHealth);
        this.texture.Texture = this.data.texture;
        this.StatsChanged += UpdateEnemy;
        this.StatsChanged += UpdateUI;
        
        this.actionHandler = new EnemyActionHandler(this);
        UpdateEnemy();
        UpdateUI();
        
        SetTargetSize();
    }

    public void TakeTurn(Action callback = null)
    {
        if(this.currentAction == null)
        {
            GD.Print("Current Action is null");
            return;
        }
        
        this.currentAction.PerformAction(callback);
    }
    
    public void UpdateEnemy()
    {
        if(this.currentAction == null)
        {
            SetCurrentAction(this.actionHandler.GetAction());
            return;
        }
    }

    public void UpdateUI()
    {
        this.healthUI.SetData(this.data, true);

        if (this.currentAction != null)
        {
            this.intentTexture.Texture = ResourceManager.instance.intentIcons[this.currentAction.intentKey];
            this.intentLabel.Text = this.currentAction.value.ToString();
        }
    }

    public void SetCurrentAction(EnemyAction value)
    {
        this.currentAction = value;
        if(this.currentAction != null)
        {
            //UpdateIntent();
        }
        else
        {
            UpdateEnemy();
        }
    }

    public void SetTargetSize()
    {
        this.SetPivotOffset(new Vector2(this.Size.X / 2, this.Size.Y / 2));
        this.enemyArea.SetPosition(this.GetPivotOffset());
        
        CircleShape2D circleShape = new CircleShape2D();
        circleShape.SetRadius(this.Size.X / 3);
        this.enemyArea.GetNode<CollisionShape2D>("CollisionShape2D").SetShape(circleShape);
    }

    public override void TakeDamage(int amount, Modifier.Type type = Modifier.Type.DMG_TAKEN)
    {
        if(this.data.health <= 0 || amount <= 0) { return; }
        
        int modifiedAmount = this.modifierHandler.GetModifiedValue(amount, type);
        int initial_dmg = modifiedAmount;
        
        Tween tween = CreateTween();
        tween.TweenCallback(Callable.From(()=>{UIManager.instance.vfxModel.Shake(this, 16, .15f);})); // must be less than interval time
        tween.TweenCallback(Callable.From(() =>
        {
            this.texture.Material = ResourceManager.instance.shaders["damage"];
            modifiedAmount = Mathf.Clamp(modifiedAmount - armor, 0, modifiedAmount);
            this.armor = Mathf.Clamp(armor - initial_dmg, 0, armor);
            this.data.health = Math.Clamp(this.data.health - modifiedAmount, 0, this.data.maxHealth);
            EmitSignal(SignalName.StatsChanged);
        }));
        tween.TweenInterval(0.17f); // this is the interval time
        tween.Finished += ()=>
        {
            this.texture.Material = null;

            if(data.health <=0)
            {
                //DestroyPlayer();
            }
        };
    }

    public void TakeDirectDamage(int amount, Modifier.Type type = Modifier.Type.DMG_TAKEN)
    {
        if(this.data.health <= 0 || amount <= 0) { return; }
        
        Tween tween = CreateTween();
        tween.TweenCallback(Callable.From(()=>{UIManager.instance.vfxModel.Shake(this, 16, .15f);})); // must be less than interval time
        tween.TweenCallback(Callable.From(() =>
        {
            this.texture.Material = ResourceManager.instance.shaders["damage"];
            this.data.health = Math.Clamp(this.data.health - amount, 0, this.data.maxHealth);
            EmitSignal(SignalName.StatsChanged);
        }));
        tween.TweenInterval(0.17f); // this is the interval time
        tween.Finished += ()=>
        {
            this.texture.Material = null;

            if(data.health <=0)
            {
                //DestroyPlayer();
            }
        };
    }

    public override void Heal(int amount, Modifier.Type type = Modifier.Type.HEALED)
    {
        Tween tween = CreateTween();
        tween.TweenCallback(Callable.From(()=>{UIManager.instance.vfxModel.Shake(this, 16, .15f);})); // must be less than interval time
        tween.TweenCallback(Callable.From(() =>
        {
            this.texture.Material = ResourceManager.instance.shaders["heal"];
            int modifiedAmount = this.modifierHandler.GetModifiedValue(amount, type);
            this.data.health = Math.Clamp(this.data.health + modifiedAmount, 0, this.data.maxHealth);
            EmitSignal(SignalName.StatsChanged);
        }));
        tween.TweenInterval(0.17f); // this is the interval time
        tween.Finished += ()=>
        {
            this.texture.Material = null;

            if(data.health <=0)
            {
                //DestroyPlayer();
            }
        };
    }

    public int GetModifiedAttack(int value)
    {
        return this.modifierHandler.GetModifiedValue(value + this.data.attack, Modifier.Type.DMG_DEALT);
    }

    public int GetModifiedDefence(int value)
    {
        return this.modifierHandler.GetModifiedValue(value + this.data.defense, Modifier.Type.ARMOR_GEN);
    }
    
    public override void ShowTargeted(bool show)
    {
        if (show)
        {
            this.targetTexture.Show();
        }
        else
        {
            this.targetTexture.Hide();
        }
    }
}

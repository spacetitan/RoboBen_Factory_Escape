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
    public GridContainer statusContainer = null;
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
        this.statusContainer = this.GetNode<GridContainer>("%StatusContainer");
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
        
        this.statusHandler.SetContainer(this.statusContainer);
        
        this.actionHandler = new EnemyActionHandler(this);
        UpdateEnemy();
        UpdateUI();

        switch (this.data.id)
        {
            case CharacterData.CharacterID.GRUBBFLY:
            case CharacterData.CharacterID.TORTIGRUB:
            case CharacterData.CharacterID.GRUBBOID:
                this.texture.Scale = new Vector2(0.85f, 0.85f);
                    break;
            
            case CharacterData.CharacterID.GRUBBMANTIS:
                this.texture.Scale = new Vector2(1.25f, 1.25f);
                    break;
            
            case CharacterData.CharacterID.GRUBBIG:
            default:
                break;
        }
        
        SetTargetSize();
    }

    public void DestroyEnemy()
    {
        this.StatsChanged -= UpdateEnemy;
        this.StatsChanged -= UpdateUI;
        this.QueueFree();
    }

    public void TakeTurn(Action callback = null)
    {
        if(this.currentAction == null)
        {
            GD.Print("Current Action is null");
            return;
        }

        BattleModel model = UIManager.instance.models[UIManager.UIState.BATTLE] as BattleModel;
        if (model.turnOrderStateMachine.currentPhaseState == TurnOrderStateMachine.PhaseState.BATTLE_END)
        {
            return;
        }

        this.currentAction.PerformAction(callback);
        this.currentAction = null;
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
        if (this.armor > 0)
        {
            this.healthUI.SetData(this, ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.SHIELD]);
        }
        else
        {
            this.healthUI.SetData(this, null, true);
        }

        

        if (this.currentAction != null)
        {
            this.intentTexture.Texture = ResourceManager.instance.intentIcons[this.currentAction.intent];
            this.intentLabel.Text = this.currentAction.GetIntent();
        }
    }

    public void StartOfTurnReset()
    {
        this.armor = 0;
        UpdateUI();
    }

    public void SetCurrentAction(EnemyAction value)
    {
        this.currentAction = value;
        if(this.currentAction != null)
        {
            UpdateUI();
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
            this.data.SetHealth(Math.Clamp(this.data.health - modifiedAmount, 0, this.data.maxHealth));
            EmitSignal(SignalName.StatsChanged);
        }));
        tween.TweenInterval(0.17f); // this is the interval time
        tween.Finished += ()=>
        {
            this.texture.Material = null;

            if(data.health <=0)
            {
                AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.DEATH]);
                EventManager.instance.EmitSignal(EventManager.SignalName.EnemyDied, this);
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
            this.data.SetHealth(Math.Clamp(this.data.health - amount, 0, this.data.maxHealth));
            EmitSignal(SignalName.StatsChanged);
        }));
        tween.TweenInterval(0.17f); // this is the interval time
        tween.Finished += ()=>
        {
            this.texture.Material = null;

            if(data.health <=0)
            {
                EventManager.instance.EmitSignal(EventManager.SignalName.EnemyDied, this);
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
            this.data.SetHealth(Math.Clamp(this.data.health + modifiedAmount, 0, this.data.maxHealth));
            EmitSignal(SignalName.StatsChanged);
        }));
        tween.TweenInterval(0.17f); // this is the interval time
        tween.Finished += ()=>
        {
            this.texture.Material = null;
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

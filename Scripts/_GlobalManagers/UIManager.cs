using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class UIManager : Node
{
	public static UIManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}

	public enum UILayer { NONE, BACKGROUNDS, MENU, HUD, POPUP, VFX };
	public Dictionary<UILayer, CanvasLayer> layers { get; private set; } = new Dictionary<UILayer, CanvasLayer>();

	public enum UIState { NONE, START, RUN, BATTLE, TREASURE, SHOP, REST, EVENT};
	public Dictionary<UIState, UIModel> models { get; private set; } = new Dictionary<UIState, UIModel>();
	
	public UIModel currentModel { get; private set; } = null;
	public UIModel lastModel { get; private set; } = null;
	public PopUpModel popUpModel { get; private set; } = null;
	public VFXModel vfxModel { get; private set; } = null;
	public BackgroundModel backgroundModel { get; private set; } = null;
	public HUDModel hudModel { get; private set; } = null;
	
	public Tween tween { get; private set; } = null;

	public override void _Ready()
	{
		init();
		GetSceneNodes();
	}

	public void GetSceneNodes()
	{
		int count = 1;
		foreach(CanvasLayer layer in this.GetChildren())
		{
			this.layers.Add((UILayer)count, layer);
			count++;

			foreach(UIModel model in layer.GetChildren())
			{
				model.InitializeModel();

				if (model.state != UIState.NONE)
				{
					this.models.Add(model.state ,model);
				}
				else
				{
					switch (model)
					{
						case PopUpModel:
							this.popUpModel = model as PopUpModel;
							break;
						
						case VFXModel:
							this.vfxModel = model as VFXModel;
							break;
						
						case BackgroundModel:
							this.backgroundModel = model as BackgroundModel;
							break;
						
						case HUDModel:
							this.hudModel = model as HUDModel;
							break;
					}
				}
			}
		}
		
		this.backgroundModel.ChangeBackgroundTexture(UIState.START);
		this.currentModel = this.models[UIState.START];
		this.currentModel.Enter();
	}
	
	public void ChangeStateTo(UIState panelState)
	{
		//GD.Print(this.currentModel.state.ToString() + " -> " + panelState.ToString());
		
		this.vfxModel.CloseCurtain(() =>
		{
			if(this.currentModel != null)
			{
				if(this.currentModel.state == panelState)
				{
					return;
				}

				this.currentModel.HideModel();
				this.lastModel = this.currentModel;
			}

			this.currentModel = this.models[panelState];
			this.backgroundModel.ChangeBackgroundTexture(panelState);
			this.hudModel.ChangeHUDTo(panelState);

			if(tween != null && tween.IsRunning())
			{
				tween.Finished += () => { this.currentModel.ShowModel(); };
			}
			else
			{
				this.currentModel.ShowModel();
			}
		});
	}
}

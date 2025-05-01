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

	public enum UIState { NONE, START, RUN, BATTLE, };
	public Dictionary<UIState, UIModel> models { get; private set; } = new Dictionary<UIState, UIModel>();
	
	public UIModel currentModel { get; private set; } = null;
	public UIModel lastModel { get; private set; } = null;
	public PopUpModel popUpModel { get; private set; } = null;
	public VFXModel vfxModel { get; private set; } = null;
	public BackgroundModel backgroundModel { get; private set; } = null;
	
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

				if (model.state == UIState.NONE)
				{
					switch (model)
					{
						case PopUpModel:
							this.popUpModel = model as PopUpModel;
						break;
						
						case VFXModel:
							this.vfxModel = model as VFXModel;
							this.vfxModel.OpenCurtain();
						break;
						
						case BackgroundModel:
							this.backgroundModel = model as BackgroundModel;
						break;
					}
				}
				else
				{
					this.models.Add(model.state ,model);
				}
			}
		}
		
		this.currentModel = this.models[UIState.START];
		this.backgroundModel.ChangeBackgroundTexture(UIState.START);
	}
	
	public void ChangeStateTo(UIState panelState)
	{
		this.vfxModel.CloseCurtain(() =>
		{
			if(this.currentModel != null)
			{
				if(this.currentModel.state == panelState)
				{
					return;
				}

				this.currentModel.Exit();
				this.lastModel = this.currentModel;
			}

			this.currentModel = this.models[panelState];
			this.backgroundModel.ChangeBackgroundTexture(panelState);

			if(tween != null && tween.IsRunning())
			{
				tween.Finished += () => { this.currentModel.Enter(); };
			}
			else
			{
				this.currentModel.Enter();
			}
		});
	}
}

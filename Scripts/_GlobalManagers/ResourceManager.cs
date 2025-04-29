using Godot;

[GlobalClass]
public partial class ResourceManager : Node
{
	public static ResourceManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}
	
	public Texture2D debugIcon = null;

	public override void _Ready()
	{
		init();
		LoadResources();
	}

	public void LoadResources()
	{
		this.debugIcon = ResourceLoader.Load<Texture2D>("res://icon.svg");
	}
}

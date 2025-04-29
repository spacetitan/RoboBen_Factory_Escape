using Godot;

[GlobalClass]
public partial class RunManager : Node
{
	public static RunManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}

	public override void _Ready()
	{
		init();
	}
}

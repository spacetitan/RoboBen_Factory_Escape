using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Invis : Status
{
	public override void InitializeStatus(Character target)
	{
		
		base.InitializeStatus(target);
	}

	public override void ApplyStatus(Character target)
	{
		base.ApplyStatus(target);
	}
	
	public override Invis CreateInstance()
	{
		return base.CreateInstance() as Invis;
	}
}

using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Effect : RefCounted
{
    [Export] public AudioStream sfx;
    public virtual void Execute(Character target){}
    public virtual void Execute(List<Character> target){}
}
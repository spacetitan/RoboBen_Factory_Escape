// meta-name: Global Manager
// meta-description: For managers that would be called from anywhere.
using Godot;

[GlobalClass]
public partial class _CLASS_ : Node
{
    public static _CLASS_ instance;

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
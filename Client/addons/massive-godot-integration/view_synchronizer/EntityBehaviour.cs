using Godot;
using Massive;

namespace massive_godot_integration.view_synchronizer;

public abstract partial class EntityBehaviour : Node {
	public abstract void OnEntityAssigned(Entity entity);
	public abstract void OnEntityRemoved();
}
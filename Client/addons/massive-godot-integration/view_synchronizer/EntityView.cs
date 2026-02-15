using Godot;
using Godot.Collections;
using Massive;
using Massive.QoL;

namespace massive_godot_integration.view_synchronizer;

public partial class EntityView : Node2D, IEntityView {
	[Export] private Array<EntityBehaviour> _entityBehaviours = [];

	public Entity Entity { get; protected set; }

	public void AssignEntity(Entity entity) {
		Entity = entity;

		if (Engine.GetMainLoop() is not SceneTree tree) {
			GD.PrintErr("Failed to get SceneTree.");
			return;
		}
		
		tree.Root.AddChild(this);

		foreach (var viewBehaviour in _entityBehaviours) {
			viewBehaviour.OnEntityAssigned(Entity);
		}
	}

	public void RemoveEntity() {
		foreach (var viewBehaviour in _entityBehaviours) {
			viewBehaviour.OnEntityRemoved();
		}

		GetParent().RemoveChild(this);

		Entity = Entity.Dead;
	}

#if UNITY_EDITOR
		[ContextMenu("Find Behaviours and Components")]
		public void CollectViewBehaviours()
		{
			UnityEditor.Undo.RecordObject(this, "Find behaviours");
			var behaviours = GetComponentsInChildren<EntityBehaviour>(true);

			_entityBehaviours.Clear();
			foreach (var behaviour in behaviours)
			{
				_entityBehaviours.Add(behaviour);
			}

			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif
}
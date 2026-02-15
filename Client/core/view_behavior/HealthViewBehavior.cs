using Fixed64;
using Godot;
using massive_godot_integration.view_synchronizer;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Client.core.view_behavior;

public partial class HealthViewBehavior : EntityBehaviour {
	[Export] private ProgressBar _targetProgress;

	private DataSet<Health> _healths;
	private Entity _entity;
	
	public override void OnEntityAssigned(Entity entity) {
		_entity = entity;
		_healths = entity.World.DataSet<Health>();
		Update();
	}
	
	public override void OnEntityRemoved() {
		_healths = null;
		_entity = Entity.Dead;
	}

	public override void _Process(double delta) {
		Update();
	}

	private void Update() {
		if (!_healths.Has(_entity.Id)) {
			return;
		}
		
		var health = _healths.Get(_entity.Id);
		_targetProgress.Value = health.Current.ToFloat();
		_targetProgress.MaxValue = health.Max.ToFloat();
	}
}
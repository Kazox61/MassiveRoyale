using Fixed64;
using Godot;
using Massive;
using massivegodotintegration.addons.massive_godot_integration.synchronizer;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Client.core.view_behavior;

public partial class HealthViewBehavior : ViewBehavior {
	[Export] private ProgressBar _targetProgress;

	private DataSet<Health> _healths;
	private Entity _entity;
	
	public override void OnEntityAssigned(World world, Entity entity) {
		_entity = entity;
		_healths = world.DataSet<Health>();
	}
	public override void OnEntityRemoved() {
		_healths = null;
		_entity = Entity.Dead;
	}

	public override void _Process(double delta) {
		if (!_healths.Has(_entity.Id)) {
			return;
		}
		
		var health = _healths.Get(_entity.Id);
		_targetProgress.Value = health.Current.ToFloat();
		_targetProgress.MaxValue = health.Max.ToFloat();
	}
}
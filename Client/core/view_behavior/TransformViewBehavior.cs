using Fixed64;
using Godot;
using MassiveRoyale.Core.Components;
using Massive;
using massivegodotintegration.addons.massive_godot_integration.synchronizer;
using MassiveRoyale.Core;

namespace MassiveRoyale.Client.core.view_behavior;

[GlobalClass]
public partial class TransformViewBehavior : ViewBehavior {
	[Export] private Node2D _targetNode;
	[Export] private AnimatedSprite2D _targetAnimatedSprite;

	private DataSet<Transform> _transforms;
	private Entity _entity;
	
	public override void OnEntityAssigned(World world, Entity entity) {
		_entity = entity;
		_transforms = world.DataSet<Transform>();
	}
	public override void OnEntityRemoved() {
		_transforms = null;
		_entity = Entity.Dead;
	}

	public override void _Process(double delta) {
		if (!_transforms.Has(_entity.Id)) {
			return;
		}
		
		var transform = _transforms.Get(_entity.Id);
		_targetNode.Position = new Vector2(
			transform.Position.X.ToFloat() * GameConfig.PixelPerField,
			transform.Position.Y.ToFloat() * GameConfig.PixelPerField
		);
		
		if (_targetAnimatedSprite == null) {
			return;
		}
		
		var direction = DirectionFromRadians(transform.Rotation.ToFloat());
		if (_targetAnimatedSprite.Animation != direction) {
			_targetAnimatedSprite.Animation = direction;
		}
	}
	
	private static string DirectionFromRadians(float radians, bool allowDiagonals = true) {
		radians = Mathf.PosMod(radians, Mathf.Tau);

		if (allowDiagonals) {
			var index = Mathf.RoundToInt(radians / (Mathf.Tau / 8)) % 8;

			return index switch {
				0 => "Right",
				1 => "DownRight",
				2 => "Down",
				3 => "DownLeft",
				4 => "Left",
				5 => "UpLeft",
				6 => "Up",
				7 => "UpRight",
				_ => "Down"
			};
		}
		else {
			var index = Mathf.RoundToInt(radians / (Mathf.Tau / 4)) % 4;

			return index switch {
				0 => "Right",
				1 => "Down",
				2 => "Left",
				3 => "Up",
				_ => "Down"
			};
		}
	}

}
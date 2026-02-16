using Fixed64;
using Godot;
using massive_godot_integration.view_synchronizer;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Client.core.view_behavior;

public partial class TroopAnimationViewBehavior : EntityBehaviour {
	[Export] private AnimatedSprite2D _targetAnimatedSprite;
	[Export] private SpriteFrames _moveAnimationSprite;
	[Export] private SpriteFrames _attackAnimationFrames;

	private DataSet<AttackProgress> _attackProgresses;
	private Entity _entity;
	
	public override void OnEntityAssigned( Entity entity) {
		_entity = entity;
		_attackProgresses = entity.World.DataSet<AttackProgress>();
		Update();
	}
	public override void OnEntityRemoved() {
		_attackProgresses = null;
		_entity = Entity.Dead;
	}

	public override void _Process(double delta) {
		Update();
	}

	private void Update() {
		return;
		if (!_attackProgresses.Has(_entity.Id)) {
			if (!_entity.IsAlive) {
				return;
			}
			
			if (_targetAnimatedSprite.SpriteFrames != _moveAnimationSprite) {
				_targetAnimatedSprite.SpriteFrames = _moveAnimationSprite;
			}

			var movement = _entity.Get<Movement>();
			SetProgress(movement.ProgressRatio.ToFloat());

			return;
		}
		
		var attackProgress = _attackProgresses.Get(_entity.Id);
		if (_targetAnimatedSprite.SpriteFrames != _attackAnimationFrames) {
			_targetAnimatedSprite.SpriteFrames = _attackAnimationFrames;
		}

		SetProgress(attackProgress.ProgressRatio.ToFloat());
	}

	private void SetProgress(float progressRatio) {
		var ratio = Mathf.Clamp(progressRatio, 0f, 1f);

		var animationName = _targetAnimatedSprite.Animation;
		var frames = _targetAnimatedSprite.SpriteFrames;
		var frameCount = frames.GetFrameCount(animationName);

		var exactFrame = ratio * (frameCount - 1);
		var frameIndex = Mathf.FloorToInt(exactFrame);
		var frameProgress = exactFrame - frameIndex;

		_targetAnimatedSprite.SetFrameAndProgress(frameIndex, frameProgress);
	}
}
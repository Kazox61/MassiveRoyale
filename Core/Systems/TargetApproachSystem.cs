using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class TargetApproachSystem : CoreSystem, IUpdate {
	public void Update() {
		World.Exclude<AttackProgress>().ForEach((Entity entity, ref Transform transform, ref Target target, ref NextAttack nextAttack) => {
			var targetEntity = target.TargetEntifier.In(World);
			if (!targetEntity.IsAlive) {
				return;
			}
			
			ref var targetTransform = ref targetEntity.Get<Transform>();
			ref var targetHitbox = ref targetEntity.Get<Hitbox>();
			var dx = targetTransform.Position.X - transform.Position.X;
			var dy = targetTransform.Position.Y - transform.Position.Y;
			var distanceSqr = dx * dx + dy * dy;
			var effectiveRange = nextAttack.Range.ToFP() + targetHitbox.Radius;
			
			transform.Rotation = FP.Atan2(dy, dx);

			if (distanceSqr <= effectiveRange * effectiveRange) {
				entity.Set(new AttackProgress {
					ProgressRatio = FP.Zero,
					Duration = FP.One,
					AttackExecutionRatio = FP.Half
				});
				
				if (entity.Has<Movement>()) {
					ref var movement1 = ref entity.Get<Movement>();
					movement1.ProgressRatio = FP.Zero;
				}
				
				return;
			}
			
			if (!entity.Has<Movement>()) {
				return;
			}
			
			ref var movement = ref entity.Get<Movement>();
			movement.MoveTowards(ref transform, targetTransform.Position);
			movement.UpdateMovementProgress();
		});
	}
}
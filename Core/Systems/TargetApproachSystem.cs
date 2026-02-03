using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class TargetApproachSystem : CoreSystem, IUpdate {
	public void Update() {
		World.ForEach((Entity entity, ref Transform transform, ref Target target, ref Movement movement, ref AttackRange attackRange) => {
			if (entity.Has<AttackProgress>()) {
				return;
			}
			
			var targetEntity = target.TargetEntifier.In(World);
			if (!targetEntity.IsAlive) {
				return;
			}
			
			ref var targetTransform = ref targetEntity.Get<Transform>();
			ref var targetHitbox = ref targetEntity.Get<Hitbox>();
			var dx = targetTransform.Position.X - transform.Position.X;
			var dy = targetTransform.Position.Y - transform.Position.Y;
			var distanceSqr = dx * dx + dy * dy;
			var effectiveRange = attackRange.Value.ToFP() + targetHitbox.Radius;

			if (distanceSqr <= effectiveRange * effectiveRange) {
				entity.Set(new AttackProgress {
					ProgressRatio = FP.Zero,
					Duration = FP.One,
					AttackExecutionRatio = FP.Half
				});
				
				return;
			}
			
			movement.MoveTowards(ref transform, targetTransform.Position);
		});
	}
}
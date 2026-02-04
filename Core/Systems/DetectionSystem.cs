using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class DetectionSystem : CoreSystem, IUpdate {
	public void Update() {
		World.ForEach((Entity entity, ref Team team, ref Transform transform, ref DetectionRange detectionRange, ref NextAttack nextAttack) => {
			if (entity.Has<Target>()) {
				var target = entity.Get<Target>();
				var targetEntity = target.TargetEntifier.In(World);
				if (!targetEntity.IsAlive) {
					entity.Remove<Target>();
				}
				return;
			}

			Entity? closestTarget = null;
			var closestDistanceSqr = FP.MaxValue;
			var teamIndex = team.TeamIndex;
			var currentPosition = transform.Position;
			var detectionRangeValue = detectionRange.Value.ToFP();
			var targetElevationLayer = nextAttack.TargetElevationLayer;

			World.ForEach((Entity targetEntity, ref Team targetTeam, ref Transform targetTransform, ref Hitbox targetHitbox) => {
				if (teamIndex == targetTeam.TeamIndex) {
					return;
				}
				
				if (!TargetUtility.CanTarget(targetElevationLayer, targetHitbox.ElevationLayer)) {
					return;
				}

				var dx = targetTransform.Position.X - currentPosition.X;
				var dy = targetTransform.Position.Y - currentPosition.Y;
				var distanceSqr = dx * dx + dy * dy;
				var effectiveRange = detectionRangeValue + targetHitbox.Radius;
				if (distanceSqr <= effectiveRange * effectiveRange && distanceSqr < closestDistanceSqr) {
					closestDistanceSqr = distanceSqr;
					closestTarget = targetEntity;
				}
			});

			if (closestTarget != null) {
				entity.Set(new Target { TargetEntifier = closestTarget.Value.Entifier });
			}
		});
	}
}
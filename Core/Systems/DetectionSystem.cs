using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class DetectionSystem : CoreSystem, IUpdate {
	public void Update() {
		World.ForEach((Entity entity, ref Team team, ref Transform transform, ref DetectionRange detectionRange) => {
			if (entity.Has<Target>()) {
				return;
			}

			Entity? closestTarget = null;
			var closestDistanceSqr = FP.MaxValue;
			var teamIndex = team.TeamIndex;
			var currentPosition = transform.Position;
			var detectionRangeValue = detectionRange.Value.ToFP();

			World.ForEach((Entity targetEntity, ref Team targetTeam, ref Transform targetTransform, ref Hitbox targetHitbox) => {
				if (teamIndex == targetTeam.TeamIndex) {
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
using Fixed64;
using Massive;
using Massive.Netcode;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class TrackingProjectileSystem : CoreSystem, IUpdate {
	public void Update() {
		World.ForEach((Entity entity, ref TrackingProjectile trackingProjectile, ref Transform transform) => {
			var targetEntity = trackingProjectile.TargetEntifier.In(World);
			
			if (!targetEntity.IsAlive) {
				entity.Destroy();
				return;
			}
			
			if (!targetEntity.Has<Transform>()) {
				entity.Destroy();
				return;
			}
			
			var targetTransform = targetEntity.Get<Transform>();
			
			var direction = FVector2.NormalizeSafe(targetTransform.Position - transform.Position);
			var step = direction * trackingProjectile.Speed * GameConfig.DeltaTime;
			var distanceToTarget = FVector2.DistanceSqr(transform.Position, targetTransform.Position);
			var stepLengthSqr = FVector2.LengthSqr(step);
			if (stepLengthSqr < distanceToTarget) {
				transform.Position += step;
				return;
			}

			transform.Position = targetTransform.Position;
			World.CreateEntity(new Hit {
				Damage = trackingProjectile.Damage,
				IsSplash = trackingProjectile.IsSplash,
				SplashRadius = trackingProjectile.SplashRadius,
				HitElevationLayer = trackingProjectile.HitElevationLayer,
				TargetEntifier = trackingProjectile.TargetEntifier,
				SourceEntifier = trackingProjectile.SourceEntifier
			});
			
			entity.Destroy();
		});
	}
}
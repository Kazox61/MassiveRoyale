using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class MovementSystem : CoreSystem, IUpdate {
	public void Update() {
		World.ForEach((Entity entity, ref Transform transform, ref FollowPath path) => {
			
			if (entity.Has<Target>()) {
				ref var target = ref entity.Get<Target>();
				var targetEntity = target.TargetEntifier.In(World);
				if (targetEntity.IsAlive) {
					ref var targetTransform = ref targetEntity.Get<Transform>();
					var dx = targetTransform.Position.X - transform.Position.X;
					var dy = targetTransform.Position.Y - transform.Position.Y;
					var distance = FMath.Sqrt(dx * dx + dy * dy);

					var range = 5.ToFP();
					if (distance <= range) {
						// attack
						return;
					}
				}
			}

			var waypoint = path.Waypoints[path.CurrentIndex];
			var direction = new FVector2(waypoint.X, waypoint.Y) - transform.Position;
			var step = FVector2.Normalize(direction) * 4.ToFP() * GameConfig.DeltaTime;

			if (FVector2.LengthSqr(step) >= FVector2.LengthSqr(direction)) {
				if (path.CurrentIndex < path.Waypoints.Length - 1) {
					path.CurrentIndex++;
				}
				
				transform.Position = new FVector2(waypoint.X, waypoint.Y);
			}
			else {
				transform.Position += step;
			}
		});

	}
}
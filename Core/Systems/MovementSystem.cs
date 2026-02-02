using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class MovementSystem : CoreSystem, IUpdate {
	public void Update() {
		World.ForEach((Entity entity, ref Team team, ref Transform transform) => {
			
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

			var waypoints = LaneUtility.GetWaypoints(team, transform.Position);
			foreach (var waypoint in waypoints) {
				if (waypoint.Y * team.Direction <= transform.Position.Y * team.Direction) {
					continue;
				}
				
				var direction = waypoint - transform.Position;
				var step = FVector2.Normalize(direction) * 4.ToFP() * GameConfig.DeltaTime;

				if (FVector2.LengthSqr(step) >= FVector2.LengthSqr(direction)) {
					continue;
				}
					
				transform.Position += step;
					
				break;
			}
		});

	}
}
using Fixed64;
using Massive;
using Massive.Netcode;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class WaypointMovementSystem : CoreSystem, IUpdate {
	
	public void Update() {
		World
			.Include<Team, Transform, Movement>()
			.Exclude<Target>()
			.ForEach((ref Team team, ref Transform transform, ref Movement movement, ref Hitbox hitbox) => {
				if (LayerUtility.HasLayer(ElevationLayer.Ground, hitbox.ElevationLayer)) {
					var waypoints = LaneUtility.GetWaypoints(team, transform.Position);
					foreach (var waypoint in waypoints) {
						var delta = waypoint.Y * team.Direction - transform.Position.Y * team.Direction;
						if (delta <= 0.001f.ToFP()) {
							continue;
						}
					
						var halfGateWidth = LaneUtility.LineGateWidth / 2;
						var preferredX = FP.Clamp(
							transform.Position.X,
							waypoint.X - halfGateWidth,
							waypoint.X + halfGateWidth
						);
						var targetPosition = new FVector2(preferredX, waypoint.Y);
						
						var dx = targetPosition.X - transform.Position.X;
						var dy = targetPosition.Y - transform.Position.Y;
						transform.Rotation = FP.Atan2(dy, dx);
						
						movement.MoveTowards(ref transform, targetPosition);
						movement.UpdateMovementProgress();
						return;
					}
				}

				var closestTowerId = -1;
				var closestDistanceSqr = FP.MaxValue;
				var closestTowerPosition = new FVector2();
				
				foreach (var entityId in World.Include<Team, Tower, Transform, Hitbox>()) {
					var entity = World.GetEntity(entityId);
					ref var towerTeam = ref entity.Get<Team>();
					if (towerTeam.TeamIndex == team.TeamIndex) {
						continue;
					}
					
					ref var towerTransform = ref entity.Get<Transform>();
					ref var towerHitbox = ref entity.Get<Hitbox>(); //TODO: use hitbox radius?
					var dx = towerTransform.Position.X - transform.Position.X;
					var dy = towerTransform.Position.Y - transform.Position.Y;
					var distanceSqr = dx * dx + dy * dy;
					if (distanceSqr < closestDistanceSqr) {
						closestDistanceSqr = distanceSqr;
						closestTowerId = entityId;
						closestTowerPosition = towerTransform.Position;
					}
				}
				
				if (closestTowerId != -1) {
					var dx = closestTowerPosition.X - transform.Position.X;
					var dy = closestTowerPosition.Y - transform.Position.Y;
					transform.Rotation = FP.Atan2(dy, dx);
					movement.MoveTowards(ref transform, closestTowerPosition);
					movement.UpdateMovementProgress();
				}
			});
	}
}
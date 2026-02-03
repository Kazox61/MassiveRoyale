using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class WaypointMovementSystem : CoreSystem, IUpdate {
	
	public void Update() {
		var query = World.Include<Team, Transform, Movement>().Exclude<Target>();
		query.ForEach((ref Team team, ref Transform transform, ref Movement movement) => {
			var waypoints = LaneUtility.GetWaypoints(team, transform.Position);
			foreach (var waypoint in waypoints) {
				var delta = waypoint.Y * team.Direction - transform.Position.Y * team.Direction;
				if (delta <= 0.001f.ToFP()) {
					continue;
				}
				
				movement.MoveTowards(ref transform, waypoint);
				return;
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
				movement.MoveTowards(ref transform, closestTowerPosition);
			}
		});
	}
}
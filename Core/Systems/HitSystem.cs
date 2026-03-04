using Massive;
using Massive.Netcode;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class HitSystem : CoreSystem, IUpdate {
	public void Update() {
		var teams = World.DataSet<Team>();
		var transforms = World.DataSet<Transform>();
		var hitboxes = World.DataSet<Hitbox>();
		
		World.ForEach((Entity entity, ref Hit hit) => {
			if (hit.IsSplash) {
				var targetTransform = transforms.Get(hit.TargetEntifier.Id);
				var sourceTeam = teams.Get(hit.SourceEntifier.Id);
				foreach (var targetableEntityId in World.Include<Team, Transform>()) {
					var targetableTeam = teams.Get(targetableEntityId);
					var targetableTransform = transforms.Get(targetableEntityId);
					var targetableHitbox = hitboxes.Get(targetableEntityId);

					if (targetableTeam.TeamIndex == sourceTeam.TeamIndex) {
						continue;
					}
					
					if (!LayerUtility.HasLayer(hit.HitElevationLayer, targetableHitbox.ElevationLayer)) {
						continue;
					}

					var overlaps = Area.Overlaps(
						targetTransform.Position,
						hit.SplashRadius,
						targetableTransform.Position,
						targetableHitbox.Radius
					);
					if (!overlaps) {
						continue;
					}
					
					World.CreateEntity(new Damage {
						Value = hit.Damage,
						TargetEntifier = World.GetEntifier(targetableEntityId),
						SourceEntifier = hit.SourceEntifier
					});
				}
			}
			else {
				World.CreateEntity(new Damage {
					Value = hit.Damage,
					TargetEntifier = hit.TargetEntifier,
					SourceEntifier = hit.SourceEntifier
				});
			}
			
			entity.Remove<Hit>();
		});
	}
}
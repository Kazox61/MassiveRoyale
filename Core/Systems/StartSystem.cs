using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class StartSystem : CoreSystem, IFirstTick {
	public void FirstTick() {
		var entity = World.CreateEntity();
		entity.Set(new Transform { Position = new FVector2(8.5.ToFP(), 5.5.ToFP())});
		entity.Set(new ViewAsset { PackedScenePath = "uid://tnjodsxnrsty" });
		entity.Set(new FollowPath { Waypoints = [
			new Waypoint { X = 3.5.ToFP(), Y = 14.ToFP() },
			new Waypoint { X = 3.5.ToFP(), Y = 16.ToFP() },
			new Waypoint { X = 3.5.ToFP(), Y = 23.ToFP() }
		]});
	}
}
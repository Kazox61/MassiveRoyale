using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class StartSystem : CoreSystem, IFirstTick {
	public void FirstTick() {
		var entity = World.CreateEntity();
		entity.Set(new Transform { Position = new FVector2(128.ToFP(), 128.ToFP())});
		entity.Set(new ViewAsset { PackedScenePath = "uid://tnjodsxnrsty" });
	}
}
using Massive;
using MassiveCommon;

namespace MassiveRoyale.Core;

public class GameSetup : IGameSetup {
	public void SetupGame(MassiveSystems systems, MassiveWorld world, uint seed) {
		systems
			.New<StartSystem>()
			.New<ElixirSystem>()
			.New<SpawnTroopSystem>()
			.New<WaypointMovementSystem>()
			.New<TargetApproachSystem>()
			.New<DetectionSystem>()
			.New<AttackProgressSystem>()
			.New<DamageSystem>()
			.New<SeparationSystem>();
	}
}
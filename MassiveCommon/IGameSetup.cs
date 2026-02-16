using Massive;

namespace MassiveCommon;

public interface IGameSetup {
	void SetupGame(MassiveSystems systems, MassiveWorld world, uint seed);
}
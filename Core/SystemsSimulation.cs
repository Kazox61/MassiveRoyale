using Massive;
using Massive.Netcode;

namespace MassiveRoyale.Core;

public class SystemsSimulation : ISimulation {
	public Systems Systems { get; }

	public SystemsSimulation(Systems systems) {
		Systems = systems;
	}
	
	public void Initialize() {
		Systems.Run<IInitialize>();
	}

	public void Update(int tick) {
		if (tick == 0) {
			Systems.Run<IFirstTick>();
		}
		
		Systems.Run<IUpdate>();
	}
}
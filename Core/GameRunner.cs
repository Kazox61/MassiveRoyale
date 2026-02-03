using Massive.Netcode;

namespace MassiveRoyale.Core;

public class GameRunner {
	public Session Session { get; }
	public int TargetTick { get; private set; }
	
	public GameRunner(Session session) {
		Session = session;
		TargetTick = 0;

		Session.Systems
			.New<StartSystem>()
			.New<WaypointMovementSystem>()
			.New<TargetApproachSystem>()
			.New<DetectionSystem>()
			.New<SeparationSystem>()
			.Build(Session);
		
		var systemSimulation = new SystemsSimulation(Session.Systems);
		Session.Simulations.Add(systemSimulation);
		
		systemSimulation.Initialize();
		
		Session.World.SaveFrame();
	}
	
	public void ProcessTick() {
		Session.Loop.FastForwardToTick(TargetTick);
		TargetTick += 1;
	}
}
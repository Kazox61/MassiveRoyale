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
			.New<AttackProgressSystem>()
			.New<DamageSystem>()
			.New<SeparationSystem>()
			.Build(Session);
		
		var basicSimulation = new BasicSimulation(Session.Systems);
		Session.Simulations.Add(basicSimulation);
		
		// basicSimulation.Initialize();
		
		Session.World.SaveFrame();
	}
	
	public void ProcessTick() {
		Session.Loop.FastForwardToTick(TargetTick);
		TargetTick += 1;
	}
}
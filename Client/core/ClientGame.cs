using Godot;
using massivegodotintegration.addons.massive_godot_integration.synchronizer;
using MassiveRoyale.Core;

namespace MassiveRoyale.Client.core;

public partial class ClientGame : Node {
	private GameRunner _gameRunner;
	private GodotEntitySynchronization _entitySynchronization;
	
	public override void _Ready() {
		var session = new Massive.Netcode.Session();
		_gameRunner = new GameRunner(session);
		
		_entitySynchronization = new GodotEntitySynchronization(session.World);
		_entitySynchronization.SubscribeViews();
	}
	
	public override void _Process(double delta) {
		_gameRunner.ProcessTick();
		_entitySynchronization.SynchronizeViews();
	}
}
using Fixed64;
using Godot;
using Massive.Netcode;
using massivegodotintegration.addons.massive_godot_integration.synchronizer;
using MassiveRoyale.Core;
using MassiveRoyale.Core.Input;

namespace MassiveRoyale.Client.core;

public partial class ClientGame : Node {
	private Session _session;
	private GameRunner _gameRunner;
	private GodotEntitySynchronization _entitySynchronization;
	
	public override void _Ready() {
		_session = new Session();
		_gameRunner = new GameRunner(_session);
		
		_entitySynchronization = new GodotEntitySynchronization(_session.World);
		_entitySynchronization.SubscribeViews();
	}
	
	public override void _PhysicsProcess(double delta) {
		_gameRunner.ProcessTick();
		_entitySynchronization.SynchronizeViews();
		
		var camera = GetViewport().GetCamera2D();
		var mousePosition = camera.GetGlobalMousePosition();
		
		var fieldX = Mathf.RoundToInt(mousePosition.X / GameConfig.PixelPerField);
		var fieldY = Mathf.RoundToInt(mousePosition.Y / GameConfig.PixelPerField);
		if (Input.IsActionJustPressed("left_click")) {
			_session.Inputs.SetActualInput(0, new PlayerInput { Position = new FVector2(fieldX.ToFP(), fieldY.ToFP())});
		}
		
		if (Input.IsActionJustPressed("right_click")) {
			_session.Inputs.SetActualInput(1, new PlayerInput { Position = new FVector2(fieldX.ToFP(), fieldY.ToFP())});
		}
	}
}
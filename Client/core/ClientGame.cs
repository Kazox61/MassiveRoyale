using Fixed64;
using Godot;
using massive_godot_integration.view_synchronizer;
using Massive.Netcode;
using MassiveRoyale.Core;
using MassiveRoyale.Core.Input;

namespace MassiveRoyale.Client.core;

public partial class ClientGame : Node2D {
	private Session _session;
	private GameRunner _gameRunner;
	private GodotViewSynchronizer _godotViewSynchronizer;
	
	public override void _EnterTree() {
		_session = new Session();
		_gameRunner = new GameRunner(_session);
		
		_godotViewSynchronizer = new GodotViewSynchronizer();
	}

	public override void _PhysicsProcess(double delta) {
		ProcessInput();
		_gameRunner.ProcessTick();
		_godotViewSynchronizer.SynchronizeAll(_session.World);
	}

	private void ProcessInput() {
		var numberPressed = 1;
		if (Input.IsKeyPressed(Key.Key1)) {
			numberPressed = 1;
		}
		if (Input.IsKeyPressed(Key.Key2)) {
			numberPressed = 2;
		}
		if (Input.IsKeyPressed(Key.Key3)) {
			numberPressed = 3;
		}
		if (Input.IsKeyPressed(Key.Key4)) {
			numberPressed = 4;
		}
		
		var camera = GetViewport().GetCamera2D();
		var mousePosition = camera.GetGlobalMousePosition();
		
		var fieldX = Mathf.RoundToInt(mousePosition.X / GameConfig.PixelPerField);
		var fieldY = Mathf.RoundToInt(mousePosition.Y / GameConfig.PixelPerField);
		if (Input.IsActionJustPressed("left_click")) {
			_session.Inputs.SetPredictionInput(0, new PlayerInput {
				Position = new FVector2(fieldX.ToFP(), fieldY.ToFP()), 
				Number = numberPressed
			});
		}
		
		if (Input.IsActionJustPressed("right_click")) {
			_session.Inputs.SetPredictionInput(1, new PlayerInput {
				Position = new FVector2(fieldX.ToFP(), fieldY.ToFP()), 
				Number = numberPressed
			});
		}
	}
}
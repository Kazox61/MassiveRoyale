using System.Net;
using Fixed64;
using Godot;
using massive_godot_integration.view_synchronizer;
using Massive.Netcode;
using MassiveCommon;
using MassiveRoyale.Core;
using MassiveRoyale.Core.Input;

namespace MassiveRoyale.Client.core;

public partial class ClientGame : Node2D {
	public static ClientGame Instance { get; private set; }
	
	[Export] public Button JoinButton;
	
	public GodotViewSynchronizer GodotViewSynchronizer { get; private set; }
	public Massive.Netcode.Client Client { get; private set; }
	public float ClientTime { get; private set; }
	public IGameSetup GameSetup { get; private set; }
	public Session Session => Client?.Session;

	public int LocalPlayerChannel => Client?.Connection.Channel ?? 0;

	public override void _EnterTree() {
		Instance = this;
		GodotViewSynchronizer = new GodotViewSynchronizer();
		
		JoinButton.Pressed += JoinGame;
	}

	public override void _ExitTree() {
		Instance = null;
		JoinButton.Pressed -= JoinGame;
	}

	private void JoinGame() {
		Client = new Massive.Netcode.Client(new SessionConfig(), new TcpConnection(), 0.1);
		Client.InputIdentifiers.RegisterAutomaticallyFromAllAssemblies();

		GameSetup = new GameSetup();

		GameSetup.SetupGame(Session.Systems, Session.World, 0);

		Session.Systems.Build(Session);

		var basicSimulation = new BasicSimulation(Session.Systems);

		Session.Simulations.Add(basicSimulation);

		// basicSimulation.Initialize();

		Client.Connection.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6348));
	}

	public override void _PhysicsProcess(double delta) {
		if (Client == null || !Client.Connection.IsConnected) {
			return;
		}
		
		JoinButton.Hide();
		
		ClientTime += (float)delta;

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
			Client.Session.Inputs.AppendPredictionEvent(
				Client.Connection.Channel,
				new PlayerInput {
					Position = new FVector2(fieldX.ToFP(), fieldY.ToFP()), 
					Number = numberPressed
				}
			);
		}

		Client.Update(ClientTime);
		
		GodotViewSynchronizer.SynchronizeAll(Session.World);
	}
}
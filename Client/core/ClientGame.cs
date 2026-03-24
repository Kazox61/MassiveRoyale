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
		JoinButton.Hide();

		var connection = new TcpConnection();
		Client = new Massive.Netcode.Client(new SessionConfig(), connection);
		Client.InputIdentifiers.RegisterAutomaticallyFromAllAssemblies();

		GameSetup = new GameSetup();

		GameSetup.SetupGame(Session.Systems, Session.World, 0);

		Session.Systems.Build(Session);

		var basicSimulation = new BasicSimulation(Session.Systems);

		Session.Simulations.Add(basicSimulation);

		// basicSimulation.Initialize();

		connection.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6348));
	}

	public override void _PhysicsProcess(double delta) {
		if (Client == null || !Client.Connection.IsConnected) {
			return;
		}
		
		ClientTime += (float)delta;

		Client.Update(ClientTime);
		
		GodotViewSynchronizer.SynchronizeAll(Session.World);
	}
}
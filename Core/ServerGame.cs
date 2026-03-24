using Massive.Netcode;

namespace MassiveRoyale.Core;

public class ServerGame {
	public Server Server { get; private set; }
	public double Time { get; private set; }

	private TcpConnectionsListener _listener;

	public ServerGame() {
		_listener = new TcpConnectionsListener(6348);
		Server = new Server(new SessionConfig(), _listener);
	}

	public void Start() {
		Server.InputIdentifiers.RegisterAutomaticallyFromAllAssemblies();

		new GameSetup().SetupGame(Server.Session.Systems, Server.Session.World, 33);

		Server.Session.Systems.Build(Server.Session);

		var basicSimulation = new BasicSimulation(Server.Session.Systems);

		Server.Session.Simulations.Add(basicSimulation);

		// basicSimulation.Initialize();

		_listener.Start();
	}

	public void Update(double delta) {
		Time += delta;
		Server.Update(Time);
	}
}
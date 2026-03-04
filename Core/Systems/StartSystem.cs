using Fixed64;
using Massive;
using Massive.Netcode;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class StartSystem : CoreSystem, IFirstTick {
	public static Team RedTeam = new Team { TeamIndex = 0, Direction = 1 };
	public static Team BlueTeam = new Team { TeamIndex = 1, Direction = -1 };
	public static Team[] Teams = [RedTeam, BlueTeam];
	
	public void FirstTick() {
		CreatePlayer(RedTeam);
		CreatePlayer(BlueTeam);
	}

	private Entity CreatePlayer(Team team) {
		var entity = World.CreateEntity(new Player {
			InputChannel = team.TeamIndex,
			Elixir = FP.Zero,
			CardQueue = [0, 1, 2, 3, 4, 5]
		});
		entity.Set(team);
		return entity;
	}
}
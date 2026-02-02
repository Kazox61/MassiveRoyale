using Fixed64;
using Massive;
using Massive.Netcode;
using MassiveRoyale.Core.Components;
using MassiveRoyale.Core.Input;

namespace MassiveRoyale.Core;

public class StartSystem : CoreSystem, IFirstTick, IUpdate {
	public static Team RedTeam = new Team { TeamIndex = 0, Direction = 1 };
	public static Team BlueTeam = new Team { TeamIndex = 1, Direction = -1 };
	public static Team[] Teams = [RedTeam, BlueTeam];
	
	public void FirstTick() {
		CreateTroop(RedTeam, new FVector2(8.5.ToFP(), 5.5.ToFP()));
		CreateTroop(RedTeam, new FVector2(9.5.ToFP(), 13.5.ToFP()));
		
		
		CreateTroop(BlueTeam, new FVector2(9.5.ToFP(), 22.5.ToFP()));
		CreateTroop(BlueTeam, new FVector2(2.5.ToFP(), 28.5.ToFP()));
	}
	
	private void CreateTroop(Team team, FVector2 field) {
		var entity = World.CreateEntity();
		entity.Set(team);
		entity.Set(new Transform { Position = field });
		entity.Set(new ViewAsset { PackedScenePath = "uid://tnjodsxnrsty" });
	}

	public void Update() {
		var playerInputs = Inputs.GetFreshInputs<PlayerInput>();
		foreach (var (channel, playerInputSource) in playerInputs) {
			if (playerInputSource.IsFresh()) {
				var team = Teams[channel];
				var field = playerInputSource.LastFresh().Position;
				CreateTroop(team, field);
			}
		}
	}
}
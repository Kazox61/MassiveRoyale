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
	
	public static FVector2 RedLeftTowerPosition = new FVector2(3.5.ToFP(), 5.5.ToFP());
	public static FVector2 RedRightTowerPosition = new FVector2(14.5.ToFP(), 5.5.ToFP());
	public static FVector2 BlueLeftTowerPosition = new FVector2(3.5.ToFP(), 24.5.ToFP());
	public static FVector2 BlueRightTowerPosition = new FVector2(14.5.ToFP(), 24.5.ToFP());
	
	public void FirstTick() {
		CreateBuilding(RedTeam, RedLeftTowerPosition).Add<Tower>();
		CreateBuilding(RedTeam, RedRightTowerPosition).Add<Tower>();
		
		CreateBuilding(BlueTeam, BlueLeftTowerPosition).Add<Tower>();
		CreateBuilding(BlueTeam, BlueRightTowerPosition).Add<Tower>();
		//CreateTroop(RedTeam, new FVector2(8.5.ToFP(), 5.5.ToFP()));
		// CreateTroop(RedTeam, new FVector2(9.5.ToFP(), 12.5.ToFP()));
		
		// CreateTroop(BlueTeam, new FVector2(9.5.ToFP(), 22.5.ToFP()));
		// CreateTroop(BlueTeam, new FVector2(2.5.ToFP(), 28.5.ToFP()));
	}
	
	private Entity CreateBuilding(Team team, FVector2 field) {
		var entity = World.CreateEntity();
		entity.Set(team);
		entity.Set(new Transform { Position = field });
		entity.Set(new Hitbox { Radius = FP.One });
		entity.Set(new PushWeight());
		entity.Set(new ViewAsset { PackedScenePath = "uid://cq5qowhu6bcnt" });
		return entity;
	}
	
	private void CreateTroop(Team team, FVector2 field, FP speed) {
		var entity = World.CreateEntity();
		entity.Set(team);
		entity.Set(new Transform { Position = field });
		entity.Set(new Hitbox { Radius = FP.Half });
		entity.Set(new DetectionRange { Value = 5 });
		entity.Set(new AttackRange { Value = 1 });
		entity.Set(new Movement { Speed = speed });
		entity.Set(new PushWeight { Value = 1 });
		entity.Set(new ViewAsset { PackedScenePath = "uid://tnjodsxnrsty" });
	}

	public void Update() {
		var playerInputs = Inputs.GetFreshInputs<PlayerInput>();
		foreach (var (channel, playerInputSource) in playerInputs) {
			if (playerInputSource.IsFresh()) {
				var team = Teams[channel];
				var field = playerInputSource.LastFresh().Position;
				CreateTroop(team, field, 2.ToFP() + (playerInputSource.LastFresh().ShiftPressed ? 2.ToFP() : FP.Zero));
			}
		}
	}
}
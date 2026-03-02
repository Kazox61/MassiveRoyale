using Fixed64;
using Massive;
using Massive.Netcode;
using Massive.QoL;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class StartSystem : CoreSystem, IFirstTick {
	public static Team RedTeam = new Team { TeamIndex = 0, Direction = 1 };
	public static Team BlueTeam = new Team { TeamIndex = 1, Direction = -1 };
	public static Team[] Teams = [RedTeam, BlueTeam];
	
	public static FVector2 RedLeftTowerPosition = new FVector2(3.5.ToFP(), 5.5.ToFP());
	public static FVector2 RedRightTowerPosition = new FVector2(14.5.ToFP(), 5.5.ToFP());
	public static FVector2 RedKingTowerPosition = new FVector2(9.ToFP(), 2.ToFP());
	public static FVector2 BlueLeftTowerPosition = new FVector2(3.5.ToFP(), 24.5.ToFP());
	public static FVector2 BlueRightTowerPosition = new FVector2(14.5.ToFP(), 24.5.ToFP());
	public static FVector2 BlueKingTowerPosition = new FVector2(9.ToFP(), 28.ToFP());
	
	public void FirstTick() {
		CreatePlayer(RedTeam);
		CreatePlayer(BlueTeam);
		
		CreateBuilding(RedTeam, RedLeftTowerPosition, BuildingConfigTable.Table[0]).Add<Tower>();
		CreateBuilding(RedTeam, RedRightTowerPosition, BuildingConfigTable.Table[0]).Add<Tower>();
		
		CreateBuilding(BlueTeam, BlueLeftTowerPosition, BuildingConfigTable.Table[0]).Add<Tower>();
		CreateBuilding(BlueTeam, BlueRightTowerPosition, BuildingConfigTable.Table[0]).Add<Tower>();
		
		CreateBuilding(RedTeam, RedKingTowerPosition, BuildingConfigTable.Table[1]).Add<Tower>();
		CreateBuilding(BlueTeam, BlueKingTowerPosition, BuildingConfigTable.Table[1]).Add<Tower>();
	}
	
	private Entity CreateBuilding(Team team, FVector2 field, BuildingConfig config) {
		var entity = World.CreateEntity(new Building());
		entity.Set(team);
		entity.Set(new Transform { Position = field });
		entity.Set(new Hitbox { Radius = config.HitboxRadius, ElevationLayer = config.HitboxLayer });
		entity.Set(new DetectionRange { Value = config.DetectionRange });
		entity.Set(new NextAttack { Range = config.AttackRange, Interval = config.AttackInterval, Damage = config.AttackDamage, TargetElevationLayer = config.AttackTargetLayer });
		entity.Set(new PushWeight { Value = 0 });
		entity.Set(new Health { Current = config.Health.ToFP(), Max = config.Health.ToFP() });
		entity.Set(new ViewAsset(config.AssetId));
		return entity;
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
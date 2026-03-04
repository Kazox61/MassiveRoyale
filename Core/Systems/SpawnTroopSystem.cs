using Fixed64;
using Massive;
using Massive.Netcode;
using Massive.QoL;
using MassiveRoyale.Core.Components;
using MassiveRoyale.Core.Input;

namespace MassiveRoyale.Core;

public class SpawnTroopSystem : CoreSystem, IFirstTick, IUpdate {
	public static FVector2 RedLeftTowerPosition = new FVector2(3.5.ToFP(), 5.5.ToFP());
	public static FVector2 RedRightTowerPosition = new FVector2(14.5.ToFP(), 5.5.ToFP());
	public static FVector2 RedKingTowerPosition = new FVector2(9.ToFP(), 2.ToFP());
	public static FVector2 BlueLeftTowerPosition = new FVector2(3.5.ToFP(), 24.5.ToFP());
	public static FVector2 BlueRightTowerPosition = new FVector2(14.5.ToFP(), 24.5.ToFP());
	public static FVector2 BlueKingTowerPosition = new FVector2(9.ToFP(), 28.ToFP());

	public void FirstTick() {
		var tower = new BuildingConfig {
			Health = 1400,
			DetectionRange = 7.5.ToFP(),
			AttackRange = 7.5.ToFP(),
			AttackDamage = 50,
			AttackInterval = 0.8.ToFP(),
			AttackTargetLayer = ElevationLayer.GroundAir,
			AssetId = 2
		};
		var mainTower = new BuildingConfig {
			Health = 2400,
			DetectionRange = 7.ToFP(),
			AttackRange = 7.ToFP(),
			AttackDamage = 50,
			AttackInterval = FP.One,
			AttackTargetLayer = ElevationLayer.GroundAir,
			AssetId = 3
		};
		
		CreateBuilding(StartSystem.RedTeam, RedLeftTowerPosition, tower).Add<Tower>();
		CreateBuilding(StartSystem.RedTeam, RedRightTowerPosition, tower).Add<Tower>();
		
		CreateBuilding(StartSystem.BlueTeam, BlueLeftTowerPosition, tower).Add<Tower>();
		CreateBuilding(StartSystem.BlueTeam, BlueRightTowerPosition, tower).Add<Tower>();
		
		CreateBuilding(StartSystem.RedTeam, RedKingTowerPosition, mainTower).Add<Tower>();
		CreateBuilding(StartSystem.BlueTeam, BlueKingTowerPosition, mainTower).Add<Tower>();
	}
	
	public void Update() {
		var players = World.DataSet<Player>();
		
		foreach (var (channel, input) in Inputs.GetAllEvents<PlayerInput>()) {
			var team = StartSystem.Teams[channel % StartSystem.Teams.Length];
			var isMirrored = team.TeamIndex % 2 == 0;
			var position = new FVector2(input.FieldX.ToFP() + FP.Half, input.FieldY.ToFP() + FP.Half);
			
			if (position.X < 0 || position.X > GameConfig.BoardFieldWidth) {
				continue;
			}
			
			if (position.Y < 0 || position.Y > GameConfig.BoardFieldHeight) {
				continue;
			}

			foreach (var playerId in players) {
				ref var player = ref players.Get(playerId);
				if (player.InputChannel != channel) {
					continue;
				}

				var cardId = player.CardQueue[input.CardIndex];
				var cardConfig = CardConfigTable.Table[cardId];
				
				
			
				if (!cardConfig.AllowOtherSideSpawn) {
					if (isMirrored && position.Y > GameConfig.BoardFieldHeightHalf.ToFP() - FP.One) {
						continue;
					}

					if (!isMirrored && position.Y < GameConfig.BoardFieldHeightHalf.ToFP() + FP.One) {
						continue;
					}
				}
				
				if (player.Elixir < cardConfig.Cost) {
					continue;
				}
				
				player.Elixir -= cardConfig.Cost.ToFP();
				
				foreach (var spawnConfig in cardConfig.Spawns) {
					var offset = new FVector2(spawnConfig.OffsetX, spawnConfig.OffsetY);
					if (isMirrored) {
						offset *= -1;
					}
					var spawnPosition = position + offset;
					
					if (spawnConfig is TroopConfig troopConfig) {
						CreateTroop(team, spawnPosition, troopConfig);
					}
					
					if (spawnConfig is BuildingConfig buildingConfig) {
						CreateBuilding(team, spawnPosition, buildingConfig);
					}
					
					if (spawnConfig is SpellConfig spellConfig) {
						World.ForEach((Entity entity, ref Team otherTeam, ref Transform transform, ref Hitbox hitbox) => {
							if (otherTeam.TeamIndex != team.TeamIndex && Area.Overlaps(spawnPosition, spellConfig.Radius, transform.Position, hitbox.Radius)) {
								World.CreateEntity(new Damage {
									Value = spellConfig.Damage,
									TargetEntifier = entity.Entifier
								});
							}
						});
					}
				}
				
				player.CardQueue[input.CardIndex] = player.CardQueue[4];
				for (var i = 4; i < player.CardQueue.Length - 1; i++) {
					player.CardQueue[i] = player.CardQueue[i + 1];
				}
				player.CardQueue[^1] = cardId;
			}
		}
	}
	
	private void CreateTroop(Team team, FVector2 field, TroopConfig config) {
		var entity = World.CreateEntity();
		entity.Set(team);
		entity.Set(new Transform { Position = field });
		entity.Set(new Hitbox { Radius = config.HitboxRadius, ElevationLayer = config.HitboxLayer });
		entity.Set(new DetectionRange { Value = config.DetectionRange });
		entity.Set(new NextAttack {
			Range = config.AttackRange,
			IsMelee = config.AttackRange <= FP.One,
			IsSplash = false,
			SplashRadius = FP.Zero,
			Damage = config.AttackDamage,
			Interval = config.AttackInterval,
			HitElevationLayer = config.AttackTargetLayer,
			TargetsOnlyBuildings = config.TargetsOnlyBuildings
		});
		entity.Set(new Movement { Speed = config.Speed });
		entity.Set(new PushWeight { Value = config.PushWeight });
		entity.Set(new Health { Current = config.Health.ToFP(), Max = config.Health.ToFP() });
		entity.Set(new ViewAsset(config.AssetId));
	}
	
	private Entity CreateBuilding(Team team, FVector2 field, BuildingConfig config) {
		var entity = World.CreateEntity(new Building());
		entity.Set(team);
		entity.Set(new Transform { Position = field });
		entity.Set(new Hitbox { Radius = config.HitboxRadius, ElevationLayer = config.HitboxLayer });
		entity.Set(new DetectionRange { Value = config.DetectionRange });
		entity.Set(new NextAttack {
			Range = config.AttackRange, 
			IsMelee = config.AttackRange <= FP.One,
			IsSplash = false,
			SplashRadius = FP.Zero,
			Damage = config.AttackDamage,
			Interval = config.AttackInterval,
			HitElevationLayer = config.AttackTargetLayer
		});
		entity.Set(new PushWeight { Value = 0 });
		entity.Set(new Health { Current = config.Health.ToFP(), Max = config.Health.ToFP() });
		entity.Set(new ViewAsset(config.AssetId));
		return entity;
	}
}
using Fixed64;
using Massive;
using Massive.Netcode;
using Massive.QoL;
using MassiveRoyale.Core.Components;
using MassiveRoyale.Core.Input;

namespace MassiveRoyale.Core;

public class SpawnTroopSystem : CoreSystem, IUpdate {
	public void Update() {
		var players = World.DataSet<Player>();
		
		foreach (var (channel, input) in Inputs.GetAllEvents<PlayerInput>()) {
			var team = StartSystem.Teams[channel % StartSystem.Teams.Length];
			var position = new FVector2(input.FieldX.ToFP() + FP.Half, input.FieldY.ToFP() + FP.Half);
			
			foreach (var playerId in players) {
				ref var player = ref players.Get(playerId);
				if (player.InputChannel != channel) {
					continue;
				}

				var cardId = player.CardQueue[input.CardIndex];
				var cardConfig = CardConfigTable.Table[cardId];
				
				if (player.Elixir < cardConfig.Cost) {
					continue;
				}
				
				player.Elixir -= cardConfig.Cost.ToFP();
				
				foreach (var spawnConfig in cardConfig.Spawns) {
					var isMirrored = team.TeamIndex % 2 == 0;
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
						World.ForEach((Entity entity, ref Team otherTeam, ref Transform transform) => {
							if (otherTeam.TeamIndex != team.TeamIndex && FVector2.LengthSqr(transform.Position - spawnPosition) <= spellConfig.Radius * spellConfig.Radius) {
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
		entity.Set(new NextAttack { Range = config.AttackRange, Damage = config.AttackDamage, Interval = config.AttackInterval, TargetElevationLayer = config.AttackTargetLayer, TargetsOnlyBuildings = config.TargetsOnlyBuildings });
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
		entity.Set(new NextAttack { Range = config.AttackRange, Interval = config.AttackInterval, Damage = config.AttackDamage, TargetElevationLayer = config.AttackTargetLayer });
		entity.Set(new PushWeight { Value = 0 });
		entity.Set(new Health { Current = config.Health.ToFP(), Max = config.Health.ToFP() });
		entity.Set(new ViewAsset(config.AssetId));
		return entity;
	}
}
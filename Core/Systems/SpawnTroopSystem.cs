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
					var spawnPosition = position + new FVector2(spawnConfig.OffsetX, spawnConfig.OffsetY);
					
					if (spawnConfig.TroopConfig != null) {
						CreateTroop(team, spawnPosition, spawnConfig.TroopConfig);
					}
				}
				
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
}
using Fixed64;
using Massive;
using Massive.Netcode;
using Massive.QoL;
using MassiveRoyale.Core.Components;
using MassiveRoyale.Core.Input;

namespace MassiveRoyale.Core;

public class SpawnTroopSystem : CoreSystem, IUpdate {
	public void Update() {
		foreach (var (channel, input) in Inputs.GetAllEvents<PlayerInput>()) {
			var team = StartSystem.Teams[channel % StartSystem.Teams.Length];
			var field = input.Position;
			var config = TroopConfigTable.Table[input.Number - 1];
			CreateTroop(team, field, config);
		}
	}
	
	private void CreateTroop(Team team, FVector2 field, TroopConfig config) {
		var entity = World.CreateEntity();
		entity.Set(team);
		entity.Set(new Transform { Position = field });
		entity.Set(new Hitbox { Radius = config.HitboxRadius, ElevationLayer = config.HitboxLayer });
		entity.Set(new DetectionRange { Value = config.DetectionRange });
		entity.Set(new NextAttack { Range = config.AttackRange, Damage = config.AttackDamage, TargetElevationLayer = config.AttackTargetLayer });
		entity.Set(new Movement { Speed = config.Speed });
		entity.Set(new PushWeight { Value = config.PushWeight });
		entity.Set(new Health { Current = config.Health.ToFP(), Max = config.Health.ToFP() });
		entity.Set(new ViewAsset(config.AssetId));
	}
}
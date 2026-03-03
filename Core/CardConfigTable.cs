using Fixed64;

namespace MassiveRoyale.Core;

public static class CardConfigTable {
	public static readonly CardConfig[] Table = [
		new CardConfig {
			Name = "Knight",
			Cost = 3,
			Spawns = [
				new TroopConfig {
					Health = 690,
					AttackDamage = 79,
					AttackInterval = 1.2f.ToFP(),
					AssetId = 1,
				},
			]
		},
		new CardConfig {
			Name = "Archers",
			Cost = 3,
			Spawns = [
				new TroopConfig {
					Health = 119,
					AttackDamage = 42,
					AttackInterval = 1.1f.ToFP(),
					DetectionRange = 5.ToFP(),
					AttackRange = 5.ToFP(),
					AttackTargetLayer = ElevationLayer.GroundAir,
					AssetId = 1,
					OffsetX = -FP.Half
				},
				new TroopConfig {
					Health = 119,
					AttackDamage = 42,
					AttackInterval = 1.1f.ToFP(),
					DetectionRange = 5.ToFP(),
					AttackRange = 5.ToFP(),
					AttackTargetLayer = ElevationLayer.GroundAir,
					AssetId = 1,
					OffsetX = FP.Half
				},
			]
		},
		new CardConfig {
			Name = "Giant",
			Cost = 5,
			Spawns = [
				new TroopConfig {
					Health = 1930,
					AttackDamage = 120,
					AttackInterval = 1.5f.ToFP(),
					AssetId = 1,
					Speed = FP.One,
					TargetsOnlyBuildings = true
				}
			]
		},
		new CardConfig {
			Name = "Minions",
			Cost = 3,
			Spawns = [
				new TroopConfig {
					HitboxLayer = ElevationLayer.Air,
					Health = 90,
					AttackDamage = 42,
					AttackInterval = FP.One,
					AttackTargetLayer = ElevationLayer.GroundAir,
					Speed = FP.Three,
					AssetId = 1,
					OffsetY = -FP.Half
				},
				new TroopConfig {
					HitboxLayer = ElevationLayer.Air,
					Health = 90,
					AttackDamage = 42,
					AttackInterval = FP.One,
					AttackTargetLayer = ElevationLayer.GroundAir,
					Speed = FP.Three,
					AssetId = 1,
					OffsetX = -FP.Half,
					OffsetY = FP.Half
				},
				new TroopConfig {
					HitboxLayer = ElevationLayer.Air,
					Health = 90,
					AttackDamage = 42,
					AttackInterval = FP.One,
					AttackTargetLayer = ElevationLayer.GroundAir,
					Speed = FP.Three,
					AssetId = 1,
					OffsetX = FP.Half,
					OffsetY = FP.Half
				},
			]
		},
		new CardConfig {
			Name = "Cannon",
			Cost = 3,
			Spawns = [
				new BuildingConfig {
					Health = 350,
					DetectionRange = 5.5.ToFP(),
					AttackRange = 5.5.ToFP(),
					AttackDamage = 83,
					AttackInterval = 0.9.ToFP(),
					AssetId = 1,
				}
			]
		},
		new CardConfig {
			Name = "Arrows",
			Cost = 3,
			Spawns = [
				new SpellConfig {
					Radius = 4.ToFP(),
					Damage = 48
				}
			]
		}
	];
}

public class CardConfig {
	public string Name;
	public int Cost;
	public SpawnConfig[] Spawns;
}

public abstract class SpawnConfig {
	public FP OffsetX;
	public FP OffsetY;
}

public class TroopConfig : SpawnConfig {
	public ElevationLayer HitboxLayer = ElevationLayer.Ground;
	public FP HitboxRadius = FP.Half;
	public FP Speed = FP.Two;
	public int PushWeight = 1;
	public int Health;
	public FP DetectionRange = 5.ToFP();
	public FP AttackRange = FP.One;
	public int AttackDamage;
	public FP AttackInterval;
	public ElevationLayer AttackTargetLayer = ElevationLayer.Ground;
	public bool TargetsOnlyBuildings;
	public int AssetId;
}

public class BuildingConfig : SpawnConfig {
	public ElevationLayer HitboxLayer = ElevationLayer.Ground;
	public FP HitboxRadius = FP.One;
	public int Health;
	public FP DetectionRange = FP.One;
	public FP AttackRange = FP.One;
	public int AttackDamage;
	public FP AttackInterval;
	public ElevationLayer AttackTargetLayer = ElevationLayer.Ground;
	public int AssetId;
}

public class SpellConfig : SpawnConfig {
	public FP Radius;
	public int Damage;
}
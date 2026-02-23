using Fixed64;

namespace MassiveRoyale.Core;

public static class CardConfigTable {
	public static CardConfig[] Table = [
		new CardConfig {
			Name = "Knight",
			Cost = 3,
			Spawns = [
				new SpawnConfig {
					TroopConfig = new TroopConfig {
						Health = 690,
						AttackDamage = 79,
						AttackInterval = 1.2f.ToFP(),
						AssetId = 1,
					},
				},
			]
		},
		new CardConfig {
			Name = "Archers",
			Cost = 3,
			Spawns = [
				new SpawnConfig {
					TroopConfig = new TroopConfig {
						Health = 119,
						AttackDamage = 42,
						AttackInterval = 1.1f.ToFP(),
						DetectionRange = 5.ToFP(),
						AttackRange = 5.ToFP(),
						AttackTargetLayer = ElevationLayer.GroundAir,
						AssetId = 1,
					},
					OffsetX = -FP.Half
				},
				new SpawnConfig {
					TroopConfig = new TroopConfig {
						Health = 119,
						AttackDamage = 42,
						AttackInterval = 1.1f.ToFP(),
						DetectionRange = 5.ToFP(),
						AttackRange = 5.ToFP(),
						AttackTargetLayer = ElevationLayer.GroundAir,
						AssetId = 1,
					},
					OffsetX = FP.Half
				}
			]
		},
		new CardConfig {
			Name = "Giant",
			Cost = 5,
			Spawns = [
				new SpawnConfig {
					TroopConfig = new TroopConfig {
						Health = 1930,
						AttackDamage = 120,
						AttackInterval = 1.5f.ToFP(),
						AssetId = 1,
						Speed = FP.One,
						TargetsOnlyBuildings = true
					}
				}]
		},
		new CardConfig {
			Name = "Minions",
			Cost = 3,
			Spawns = [
				new SpawnConfig {
					TroopConfig = new TroopConfig {
						HitboxLayer = ElevationLayer.Air,
						Health = 90,
						AttackDamage = 42,
						AttackInterval = FP.One,
						AttackTargetLayer = ElevationLayer.GroundAir,
						Speed = FP.Three,
						AssetId = 1,
					},
					OffsetY = FP.Half
				},
				new SpawnConfig {
					TroopConfig = new TroopConfig {
						HitboxLayer = ElevationLayer.Air,
						Health = 90,
						AttackDamage = 42,
						AttackInterval = FP.One,
						AttackTargetLayer = ElevationLayer.GroundAir,
						Speed = FP.Three,
						AssetId = 1,
					},
					OffsetX = -FP.Half,
					OffsetY = -FP.Half
				},
				new SpawnConfig {
					TroopConfig = new TroopConfig {
						HitboxLayer = ElevationLayer.Air,
						Health = 90,
						AttackDamage = 42,
						AttackInterval = FP.One,
						AttackTargetLayer = ElevationLayer.GroundAir,
						Speed = FP.Three,
						AssetId = 1,
					},
					OffsetX = FP.Half,
					OffsetY = -FP.Half
				},
			]
		},
	];
}

public class CardConfig {
	public string Name;
	public int Cost;
	public SpawnConfig[] Spawns;
}

public class SpawnConfig {
	public TroopConfig? TroopConfig;
	public BuildingConfig? BuildingConfig;
	public FP OffsetX;
	public FP OffsetY;
}

public class TroopConfig {
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
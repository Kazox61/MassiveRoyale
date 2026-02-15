using Fixed64;

namespace MassiveRoyale.Core;

public static class BuildingConfigTable {
	public static BuildingConfig[] Table = [
		new BuildingConfig {
			Health = 10,
			DetectionRange = 5,
			AttackRange = 5,
			AttackDamage = 2,
			AssetId = 2
		},
		new BuildingConfig {
			Health = 20,
			DetectionRange = 5,
			AttackRange = 5,
			AttackDamage = 2,
			AssetId = 3
		},
	];
}

public class BuildingConfig {
	public ElevationLayer HitboxLayer = ElevationLayer.Ground;
	public FP HitboxRadius = FP.One;
	public int Health;
	public int DetectionRange = 1;
	public int AttackRange = 1;
	public int AttackDamage;
	public ElevationLayer AttackTargetLayer = ElevationLayer.Ground;
	public int AssetId;
}
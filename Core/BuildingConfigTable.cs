using Fixed64;

namespace MassiveRoyale.Core;

public static class BuildingConfigTable {
	public static BuildingConfig[] Table = [
		new BuildingConfig {
			Health = 1400,
			DetectionRange = 7.5.ToFP(),
			AttackRange = 7.5.ToFP(),
			AttackDamage = 50,
			AttackInterval = 0.8.ToFP(),
			AttackTargetLayer = ElevationLayer.GroundAir,
			AssetId = 2
		},
		new BuildingConfig {
			Health = 2400,
			DetectionRange = 7.ToFP(),
			AttackRange = 7.ToFP(),
			AttackDamage = 50,
			AttackInterval = FP.One,
			AttackTargetLayer = ElevationLayer.GroundAir,
			AssetId = 3
		},
	];
}

public class BuildingConfig {
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
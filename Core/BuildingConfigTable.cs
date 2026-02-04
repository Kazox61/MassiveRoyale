using Fixed64;

namespace MassiveRoyale.Core;

public static class BuildingConfigTable {
	public static BuildingConfig[] Table = [
		new BuildingConfig {
			Health = 10,
			DetectionRange = 5,
			AttackRange = 5,
			AttackDamage = 2,
			PackedScenePath = "uid://cq5qowhu6bcnt"
		},
		new BuildingConfig {
			Health = 20,
			DetectionRange = 5,
			AttackRange = 5,
			AttackDamage = 2,
			PackedScenePath = "uid://ch70oh23or4k3"
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
	public string PackedScenePath = "";
}
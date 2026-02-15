using Fixed64;

namespace MassiveRoyale.Core;

public static class TroopConfigTable {
	public static TroopConfig[] Table = [
		new TroopConfig {
			Health = 10,
			AttackDamage = 2,
			AssetId = 1,
		},
		new TroopConfig {
			HitboxLayer = ElevationLayer.Air,
			Health = 10,
			AttackDamage = 2,
			AttackTargetLayer = ElevationLayer.Any,
			AssetId = 1,
		}
	];
}

public class TroopConfig {
	public ElevationLayer HitboxLayer = ElevationLayer.Ground;
	public FP HitboxRadius = FP.Half;
	public FP Speed = 4.ToFP();
	public int PushWeight = 1;
	public int Health;
	public int DetectionRange = 5;
	public int AttackRange = 1;
	public int AttackDamage;
	public ElevationLayer AttackTargetLayer = ElevationLayer.Ground;
	public int AssetId;
}
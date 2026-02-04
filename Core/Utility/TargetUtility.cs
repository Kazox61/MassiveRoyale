namespace MassiveRoyale.Core;

public static class TargetUtility {
	public static bool CanTarget(ElevationLayer attackerMask, ElevationLayer targetLayer) {
		return (attackerMask & targetLayer) != 0;
	}
}
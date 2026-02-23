using Fixed64;

namespace MassiveRoyale.Core.Components;

public struct NextAttack {
	public FP Range;
	public int Damage;
	public FP Interval;
	public ElevationLayer TargetElevationLayer;
	public bool TargetsOnlyBuildings;
}
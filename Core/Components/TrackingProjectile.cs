using Fixed64;
using Massive;

namespace MassiveRoyale.Core.Components;

public struct TrackingProjectile {
	public FP Speed;
	public Entifier TargetEntifier;
	public int Damage;
	public bool IsSplash;
	public FP SplashRadius;
	public ElevationLayer HitElevationLayer;
	public Entifier SourceEntifier;
}
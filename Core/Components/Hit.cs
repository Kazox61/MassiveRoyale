using Fixed64;
using Massive;

namespace MassiveRoyale.Core.Components;

public struct Hit {
	public int Damage;
	public bool IsSplash;
	public FP SplashRadius;
	public ElevationLayer HitElevationLayer;
	public Entifier TargetEntifier;
	public Entifier SourceEntifier;
}
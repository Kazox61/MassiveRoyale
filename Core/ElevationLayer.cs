namespace MassiveRoyale.Core;

[Flags]
public enum ElevationLayer {
	None   = 0,
	Ground = 1 << 0,
	Air    = 1 << 1,
	Any    = Ground | Air
}
using Fixed64;

namespace MassiveRoyale.Core;

public static class GameConfig {
	public const int BoardFieldWidth = 18;
	public const int BoardFieldHeight = 30;
	public const int PixelPerField = 48;
	
	public const int TicksPerSecond = 30;
	public static FP DeltaTime => FP.One / TicksPerSecond;
}
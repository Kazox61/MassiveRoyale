using Fixed64;

namespace MassiveRoyale.Core;

public static class GameConfig {
	public const int BoardFieldWidth = 18;
	public const int BoardFieldWidthHalf = BoardFieldWidth / 2;
	public const int BoardFieldHeight = 30;
	public const int BoardFieldHeightHalf = BoardFieldHeight / 2;
	public const int PixelPerField = 48;
	
	// session config has the tick rate as well, you need to change it there as well
	public const int TicksPerSecond = 60;
	public static FP DeltaTime => FP.One / TicksPerSecond;
}
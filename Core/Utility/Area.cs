using Fixed64;

namespace MassiveRoyale.Core;

public class Area {
	public static bool Overlaps(FVector2 position1, FP radius1, FVector2 position2, FP radius2) {
		var distanceSqr = FVector2.DistanceSqr(position1, position2);
		var radiusSum = radius1 + radius2;
		return distanceSqr < radiusSum * radiusSum;
	}
}
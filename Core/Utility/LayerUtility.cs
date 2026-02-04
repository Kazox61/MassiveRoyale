namespace MassiveRoyale.Core;

public static class LayerUtility {
	public static bool HasLayer(ElevationLayer mask, ElevationLayer targetLayer) {
		return (mask & targetLayer) != 0;
	}
}
using Godot.Collections;
using Godot;

namespace massive_godot_integration.view_synchronizer;

public partial class ViewDataBase : Resource {
	[Export] public Dictionary<int, PackedScene> ViewPrefabs;

	public PackedScene GetViewPrefab(int viewId) {
		if (ViewPrefabs.TryGetValue(viewId, out var value)) {
			return value;
		}
		
		GD.PrintErr($"Prefab for view ID {viewId} not found.");
		return null;
	}
}
using Godot;
using Massive;

namespace massivegodotintegration.addons.massive_godot_integration.synchronizer;

public class GodotEntitySynchronization {
	private readonly World _world;
	private readonly EntityViewSynchronizer _entityViewSynchronizer;

	public GodotEntitySynchronization(Node parentRoot, World world) {
		_world = world;
		_entityViewSynchronizer = new EntityViewSynchronizer(parentRoot, world);
	}

	public void SubscribeViews() {
		_world.DataSet<ViewAsset>().AfterAdded += OnAfterViewAdded;
		_world.DataSet<ViewAsset>().BeforeRemoved += OnBeforeViewRemoved;
	}

	public void UnsubscribeViews() {
		_world.DataSet<ViewAsset>().AfterAdded -= OnAfterViewAdded;
		_world.DataSet<ViewAsset>().BeforeRemoved -= OnBeforeViewRemoved;
	}

	public void SynchronizeViews() {
		_entityViewSynchronizer.SynchronizeAll();
	}

	private void OnAfterViewAdded(int entityId) {
		_entityViewSynchronizer.SynchronizeView(entityId);
	}

	private void OnBeforeViewRemoved(int entityId) {
		_entityViewSynchronizer.DestroyView(entityId);
	}
}
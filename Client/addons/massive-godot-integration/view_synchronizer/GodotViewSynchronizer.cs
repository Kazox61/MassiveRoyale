using Massive.QoL;

namespace massive_godot_integration.view_synchronizer;

public class GodotViewSynchronizer : ViewSynchronizer<EntityView> {
	public GodotViewSynchronizer() : this(new EntityViewFactory()) { }
	public GodotViewSynchronizer(IViewFactory<EntityView> viewFactory) : base(viewFactory) { }
}
using Godot;
using Massive.QoL;

namespace massive_godot_integration.view_synchronizer;

public class EntityViewFactory : IViewFactory<EntityView> {
	private const string ViewDataBasePath = "res://core/view_data_base.tres";
	private static ViewDataBase _viewDataBase;
	
	public EntityView CreateView(ViewAsset viewAsset) {
		_viewDataBase ??= GD.Load<ViewDataBase>(ViewDataBasePath);
		var prefab = _viewDataBase.GetViewPrefab(viewAsset.Id);
		var view = prefab.Instantiate<EntityView>();
		return view;
	}

	public void DestroyView(EntityView view) {
		view.QueueFree();
	}
}
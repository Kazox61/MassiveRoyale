using System.Text;
using Godot;
using Massive.QoL;

namespace massive_godot_integration.view_synchronizer;

public class PrefabFactory<T> : IFactory<T> where T : Node {
	private readonly Node _parent;
	private readonly PackedScene _prefab;
	private readonly string _name;
	private readonly StringBuilder _nameBuilder;

	private int _objectIndex;

	public PrefabFactory(PackedScene prefab, Node parent, string name) {
		_prefab = prefab;
		_name = name;
		_parent = parent;
		_nameBuilder = new StringBuilder();
	}

	public PrefabFactory(PackedScene prefab, Node parent) : this(prefab, parent, prefab.ResourceName) { }

	public PrefabFactory(PackedScene prefab) : this(prefab, null, prefab.ResourceName) { }

	public T Create() {
		var instance = _prefab.Instantiate<T>();
		_parent?.AddChild(instance);
		instance.Name = _nameBuilder.Append(_name).Append(' ').Append(_objectIndex).ToString();
		_nameBuilder.Clear();
		_objectIndex++;
		return instance;
	}
}
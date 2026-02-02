using Massive;

namespace MassiveRoyale.Core;

public interface IUpdate : ISystemMethod<IUpdate> {
	void Update();

	void ISystemMethod<IUpdate>.Run() => Update();
}
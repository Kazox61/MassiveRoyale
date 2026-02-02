using Massive;

namespace MassiveRoyale.Core;

public interface IInitialize : ISystemMethod<IInitialize> {
	void Initialize();

	void ISystemMethod<IInitialize>.Run() => Initialize();
}
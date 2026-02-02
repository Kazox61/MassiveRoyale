using Massive;

namespace MassiveRoyale.Core;

public interface IFirstTick : ISystemMethod<IFirstTick> {
	void FirstTick();
	
	void ISystemMethod<IFirstTick>.Run() => FirstTick();
}
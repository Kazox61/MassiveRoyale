using Fixed64;
using Massive.Netcode;

namespace MassiveRoyale.Core.Input;

public struct PlayerInput : IEvent {
	public FVector2 Position;
	public int Number;
}
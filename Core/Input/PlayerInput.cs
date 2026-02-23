using Fixed64;
using Massive.Netcode;

namespace MassiveRoyale.Core.Input;

public struct PlayerInput : IEvent {
	public int FieldX;
	public int FieldY;
	public int CardIndex;
}
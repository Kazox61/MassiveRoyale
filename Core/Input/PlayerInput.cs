using Fixed64;
using Massive.Netcode;

namespace MassiveRoyale.Core.Input;

public struct PlayerInput : IInput {
	public FVector2 Position;
	public bool ShiftPressed;
}
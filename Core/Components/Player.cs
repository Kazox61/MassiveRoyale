using Fixed64;
using Massive;

namespace MassiveRoyale.Core.Components;

public struct Player : ICopyable<Player> {
	public int InputChannel;
	public FP Elixir;
	public int[] CardQueue;
	
	public void CopyTo(ref Player other) {
		other.InputChannel = InputChannel;
		other.Elixir = Elixir;

		if (other.CardQueue == null || other.CardQueue.Length != CardQueue.Length) {
			other.CardQueue = new int[CardQueue.Length];
		}

		Array.Copy(CardQueue, other.CardQueue, CardQueue.Length);
	}
}
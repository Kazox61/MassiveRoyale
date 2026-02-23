using Fixed64;
using Massive;
using Massive.Netcode;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class ElixirSystem : CoreSystem, IUpdate {
	public void Update() {
		World.ForEach((ref Player player) => {
			player.Elixir = FP.Min(10.ToFP(), player.Elixir + GameConfig.DeltaTime);
		});
	}
}
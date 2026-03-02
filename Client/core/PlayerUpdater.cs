using Fixed64;
using Godot;
using Massive;
using MassiveRoyale.Client.core.ui;
using MassiveRoyale.Core;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Client.core;

public partial class PlayerUpdater : Node {
	[Export] private ClientGame _clientGame;
	[Export] private Card[] _cards;
	[Export] private ProgressBar _elixirBar;

	public override void _Process(double delta) {
		if (_clientGame.Client == null || !_clientGame.Client.Connection.IsConnected) {
			return;
		}
		
		var players = _clientGame.Session.World.DataSet<Player>();
		
		foreach (var playerId in players) {
			var player = players.Get(playerId);
			
			if (player.InputChannel != _clientGame.LocalPlayerChannel) {
				continue;
			}

			for (var i = 0; i < 4; i++) {
				var cardTableIndex = player.CardQueue[i];
				var cardConfig = CardConfigTable.Table[cardTableIndex];
				_cards[i].Update(cardConfig);
			}
			
			_elixirBar.Value = player.Elixir.ToFloat();
		}
	}
}
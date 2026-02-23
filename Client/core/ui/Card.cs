using Godot;
using MassiveRoyale.Core;
using MassiveRoyale.Core.Input;

namespace MassiveRoyale.Client.core.ui;

public partial class Card : DraggableCard {
	[Export] public int Index;
	[Export] public TextureRect Portrait;
	
	[Export] private ClientGame _clientGame;

	private CardConfig _cardConfig;
	private Sprite2D _tmpBody;

	protected override void OnCardExited(Vector2 screenPosition) {
		var worldPosition = GetViewport().GetCanvasTransform().AffineInverse() * screenPosition;

		_tmpBody = new Sprite2D {
			Texture = GD.Load<Texture2D>("res://icon.svg"),
			Scale = new Vector2(0.5f, 0.5f)
		};
		
		GetTree().Root.AddChild(_tmpBody);
		_tmpBody.GlobalPosition = FieldToWorld(WorldToField(worldPosition));
	}

	protected override void OnCardExitDrag(Vector2 screenPosition) {
		if (_cardConfig == null || _tmpBody == null) {
			return;
		}

		var worldPosition = GetViewport().GetCanvasTransform().AffineInverse() * screenPosition;
		_tmpBody.GlobalPosition = FieldToWorld(WorldToField(worldPosition));;
	}

	protected override void OnCardExitRelease(Vector2 screenPosition) {
		if (_cardConfig == null || _tmpBody == null) {
			return;
		}

		_tmpBody.QueueFree();
		_tmpBody = null;

		var worldPosition = GetViewport().GetCanvasTransform().AffineInverse() * screenPosition;
		var field = WorldToField(worldPosition);
		
		_clientGame.Client.Session.Inputs.AppendPredictionEvent(
			_clientGame.LocalPlayerChannel,
			new PlayerInput {
				FieldX = field.X,
				FieldY = field.Y,
				CardIndex = Index
			}
		);
	}

	public void Update(CardConfig newConfig) {
		_cardConfig = newConfig;
		Portrait.Texture = GD.Load<Texture2D>("res://icon.svg");
	}
	
	private Vector2I WorldToField(Vector2 worldPosition) {
		var roundedX = Mathf.RoundToInt(worldPosition.X / GameConfig.PixelPerField);
		var roundedY = Mathf.RoundToInt(worldPosition.Y / GameConfig.PixelPerField);
		return new Vector2I(roundedX, roundedY);
	}
	
	private Vector2 FieldToWorld(Vector2I fieldPosition) {
		var worldX = fieldPosition.X * GameConfig.PixelPerField + GameConfig.PixelPerField / 2f;
		var worldY = fieldPosition.Y * GameConfig.PixelPerField + GameConfig.PixelPerField / 2f;
		return new Vector2(worldX, worldY);
	}
}
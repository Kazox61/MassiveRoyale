using Godot;

namespace MassiveRoyale.Client.core.ui;

public abstract partial class DraggableCard : Control {
	private bool _hasRectExited;

	public override void _GuiInput(InputEvent @event) {
		if (@event is InputEventScreenTouch screenTouch) {
			if (screenTouch.Pressed) {
				HandleScreenTouchStart(screenTouch);
			}
			if (!screenTouch.Pressed) {
				HandleScreenTouchEnd(screenTouch);
			}
		}

		if (@event is InputEventScreenDrag screenDrag) {
			HandleScreenDrag(screenDrag);
		}
	}

	private void HandleScreenTouchStart(InputEventScreenTouch screenTouch) {
		_hasRectExited = false;
		OnTouchStarted(GlobalPosition + screenTouch.Position);
	}

	private void HandleScreenTouchEnd(InputEventScreenTouch screenTouch) {
		var screenPos = GlobalPosition + screenTouch.Position;

		if (!_hasRectExited) {
			OnPressed(screenPos);
		}
		else {
			OnCardExitRelease(screenPos);
		}
	}

	private void HandleScreenDrag(InputEventScreenDrag screenDrag) {
		var screenPosition = GlobalPosition + screenDrag.Position;

		if (_hasRectExited) {
			OnCardExitDrag(screenPosition);
			return;
		}

		if (!GetGlobalRect().HasPoint(screenPosition)) {
			_hasRectExited = true;
			OnCardExited(screenPosition);
		}
	}

	protected virtual void OnTouchStarted(Vector2 screenPosition) { }

	protected virtual void OnPressed(Vector2 screenPosition) { }

	protected virtual void OnCardExited(Vector2 screenPosition) { }

	protected virtual void OnCardExitDrag(Vector2 screenPosition) { }

	protected virtual void OnCardExitRelease(Vector2 screenPosition) { }
}

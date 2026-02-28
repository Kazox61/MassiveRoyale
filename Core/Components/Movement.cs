using Fixed64;

namespace MassiveRoyale.Core.Components;

public struct Movement {
	public FP Speed;
	public FP ProgressRatio;
	
	public void MoveTowards(ref Transform transform, FVector2 targetPosition) {
		var direction = targetPosition - transform.Position;
		var distanceSqr = FVector2.LengthSqr(direction);

		if (distanceSqr <= 0.0001f.ToFP()) {
			transform.Position = targetPosition;
			return;
		}
		
		var distance = FP.Sqrt(distanceSqr);
		var maxStep = Speed * GameConfig.DeltaTime;
		
		if (maxStep >= distance) {
			transform.Position = targetPosition;
			return;
		}
		
		var step = direction / distance * maxStep;
		// Failsafe: never allow zero step if not at target
		if (FVector2.LengthSqr(step) == FP.Zero) {
			step = direction / distance; // minimal 1 unit direction
		}
		
		transform.Position += step;
	}

	public void UpdateMovementProgress() {
		ProgressRatio += GameConfig.DeltaTime * 1.5.ToFP();
		if (ProgressRatio >= FP.One) {
			ProgressRatio -= FP.One;
		}
	}
}
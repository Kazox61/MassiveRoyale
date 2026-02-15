using Fixed64;

namespace MassiveRoyale.Core.Components;

public struct Movement {
	public FP Speed;
	public FP ProgressRatio;
	
	public void MoveTowards(ref Transform transform, FVector2 targetPosition) {
		var direction = targetPosition - transform.Position;
		var step = FVector2.NormalizeSafe(direction) * Speed * GameConfig.DeltaTime;
		// we don't care about overshooting for now
		// if (FVector2.LengthSqr(step) >= FVector2.LengthSqr(direction)) { }
		transform.Position += step;
	}

	public void UpdateMovementProgress() {
		ProgressRatio += GameConfig.DeltaTime * 1.5.ToFP();
		if (ProgressRatio >= FP.One) {
			ProgressRatio -= FP.One;
		}
	}
}
using Fixed64;

namespace MassiveRoyale.Core.Components;

public struct Movement {
	public FP Speed;
	
	public void MoveTowards(ref Transform transform, FVector2 targetPosition) {
		var direction = targetPosition - transform.Position;
		var step = FVector2.Normalize(direction) * Speed * GameConfig.DeltaTime;
		// we don't care about overshooting for now
		// if (FVector2.LengthSqr(step) >= FVector2.LengthSqr(direction)) { }
		transform.Position += step;
	}
}
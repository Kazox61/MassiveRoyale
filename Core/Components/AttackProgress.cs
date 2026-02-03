using Fixed64;

namespace MassiveRoyale.Core.Components;

public struct AttackProgress {
	public FP ProgressRatio;
	public FP Duration;
	public FP AttackExecutionRatio;
	public bool HasExecuted;
}
using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class AttackProgressSystem : CoreSystem, IUpdate {
	public void Update() {
		World.ForEach((Entity entity, ref AttackProgress attackProgress) => {
			attackProgress.ProgressRatio += GameConfig.DeltaTime / attackProgress.Duration;
			if (!attackProgress.HasExecuted && attackProgress.ProgressRatio >= attackProgress.AttackExecutionRatio) {
				if (!entity.Has<Target>()) {
					entity.Remove<AttackProgress>();
					return;
				}
				
				var hitEntity = World.CreateEntity();
				hitEntity.Set(new Hit { 
					SourceEntifier = entity.Entifier,
					TargetEntifier = entity.Get<Target>().TargetEntifier
				});
				hitEntity.Set(new Damage { Value = 1 });
				
				attackProgress.HasExecuted = true;
			}
			
			if (attackProgress.ProgressRatio >= FP.One) {
				entity.Remove<AttackProgress>();
				entity.Remove<Target>();
			}
		});
	}
}
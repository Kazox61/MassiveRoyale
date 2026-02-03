using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class DamageSystem : CoreSystem, IUpdate {
	public void Update() {
		World.ForEach((Entity hitEntity, ref Hit hit, ref Damage damage) => {
			var targetEntity = hit.TargetEntifier.In(World);
			if (!targetEntity.IsAlive) {
				return;
			}

			ref var health = ref targetEntity.Get<Health>();
			health.Current -= damage.Value.ToFP();
			
			if (health.Current <= FP.Zero) {
				targetEntity.Destroy();
			}
			
			hitEntity.Destroy();
		});
	}
}
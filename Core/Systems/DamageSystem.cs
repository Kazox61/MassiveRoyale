using Fixed64;
using Massive;
using Massive.Netcode;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class DamageSystem : CoreSystem, IUpdate {
	public void Update() {
		World.ForEach((Entity hitEntity, ref Damage damage) => {
			var targetEntity = damage.TargetEntifier.In(World);
			if (!targetEntity.IsAlive) {
				hitEntity.Destroy();
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
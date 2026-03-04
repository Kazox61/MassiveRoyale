using Fixed64;
using Massive;
using Massive.Netcode;
using Massive.QoL;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class AttackProgressSystem : CoreSystem, IUpdate {
	public void Update() {
		World.ForEach((Entity entity, ref Transform transform, ref AttackProgress attackProgress, ref NextAttack nextAttack) => {
			attackProgress.ProgressRatio += GameConfig.DeltaTime / attackProgress.Duration;
			if (!attackProgress.HasExecuted && attackProgress.ProgressRatio >= attackProgress.AttackExecutionRatio) {
				if (!entity.Has<Target>()) {
					entity.Remove<AttackProgress>();
					return;
				}

				var target = entity.Get<Target>();

				if (nextAttack.IsMelee) {
					World.CreateEntity(new Hit {
						Damage = nextAttack.Damage,
						IsSplash = nextAttack.IsSplash,
						SplashRadius = nextAttack.SplashRadius,
						TargetEntifier = target.TargetEntifier,
						SourceEntifier = entity.Entifier
					});
				}
				else {
					var projectile = World.CreateEntity(new TrackingProjectile {
						Speed = 6.ToFP(),
						TargetEntifier = target.TargetEntifier,
						Damage = nextAttack.Damage,
						IsSplash = nextAttack.IsSplash,
						SplashRadius = nextAttack.SplashRadius,
						HitElevationLayer = nextAttack.HitElevationLayer,
						SourceEntifier = entity.Entifier
					});
					projectile.Set(new Transform {
						Position = transform.Position
					});
					projectile.Set(new ViewAsset(4));
				}
				
				attackProgress.HasExecuted = true;
			}
			
			if (attackProgress.ProgressRatio >= FP.One) {
				entity.Remove<AttackProgress>();
				entity.Remove<Target>();
			}
		});
	}
}
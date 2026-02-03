using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class SeparationSystem : CoreSystem, IUpdate {
	public void Update() {
		Span<int> buffer = stackalloc int[256];
		var count = 0;

		foreach (var entity in World.Include<Transform, Hitbox, PushWeight>().Entities) {
			buffer[count++] = entity.Id;
		}

		for (var i = 0; i < count; i++) {
			var a = World.GetEntity(buffer[i]);

			ref var ta = ref a.Get<Transform>();
			ref var ha = ref a.Get<Hitbox>();
			ref var wa = ref a.Get<PushWeight>();

			for (var j = i + 1; j < count; j++) {
				var b = World.GetEntity(buffer[j]);

				ref var tb = ref b.Get<Transform>();
				ref var hb = ref b.Get<Hitbox>();
				ref var wb = ref b.Get<PushWeight>();

				var dx = tb.Position.X - ta.Position.X;
				var dy = tb.Position.Y - ta.Position.Y;
				var distSqr = dx * dx + dy * dy;

				var minDist = ha.Radius + hb.Radius;
				if (distSqr >= minDist * minDist) {
					continue;
				}

				var dist = FMath.Sqrt(distSqr);
				if (dist == FP.Zero) {
					continue;
				}

				if (wa.Value == FP.Zero && wb.Value == FP.Zero) {
					throw new InvalidOperationException($"Two immovable entities overlap: {a.Id} and {b.Id}");
				}

				var overlap = minDist - dist;
				var nx = dx / dist;
				var ny = dy / dist;

				if (wa.Value == FP.Zero) {
					tb.Position.X += nx * overlap;
					tb.Position.Y += ny * overlap;
					continue;
				}

				if (wb.Value == FP.Zero) {
					ta.Position.X -= nx * overlap;
					ta.Position.Y -= ny * overlap;
					continue;
				}

				var totalWeight = wa.Value + wb.Value;
				var pushA = overlap * wb.Value / totalWeight;
				var pushB = (overlap * wa.Value) / totalWeight;

				ta.Position.X -= nx * pushA;
				ta.Position.Y -= ny * pushA;

				tb.Position.X += nx * pushB;
				tb.Position.Y += ny * pushB;
			}
		}
	}
}
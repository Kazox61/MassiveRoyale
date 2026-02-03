using Fixed64;
using Massive;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class SeparationSystem : CoreSystem, IUpdate {
	public void Update() {
		var entityIds = new List<int>();

		foreach (var entity in World.Include<Transform, Hitbox, PushWeight>().Entities) {
			entityIds.Add(entity.Id);
		}

		for (var i = 0; i < entityIds.Count; i++) {
			var a = World.GetEntity(entityIds[i]);

			ref var transformA = ref a.Get<Transform>();
			ref var hitboxA = ref a.Get<Hitbox>();
			ref var pushWeightA = ref a.Get<PushWeight>();

			for (var j = i + 1; j < entityIds.Count; j++) {
				var b = World.GetEntity(entityIds[j]);

				ref var transformB = ref b.Get<Transform>();
				ref var hitboxB = ref b.Get<Hitbox>();
				ref var pushWeightB = ref b.Get<PushWeight>();

				var deltaX = transformB.Position.X - transformA.Position.X;
				var deltaY = transformB.Position.Y - transformA.Position.Y;
				var distSqr = deltaX * deltaX + deltaY * deltaY;

				var minDist = hitboxA.Radius + hitboxB.Radius;
				if (distSqr >= minDist * minDist) {
					continue;
				}

				var dist = FMath.Sqrt(distSqr);
				if (dist == FP.Zero) {
					continue;
				}

				if (pushWeightA.Value == FP.Zero && pushWeightB.Value == FP.Zero) {
					throw new InvalidOperationException($"Two immovable entities overlap: {a.Id} and {b.Id}");
				}

				var overlap = minDist - dist;
				var normalizedDeltaX = deltaX / dist;
				var normalizedDeltaY = deltaY / dist;

				if (pushWeightA.Value == FP.Zero) {
					transformB.Position.X += normalizedDeltaX * overlap;
					transformB.Position.Y += normalizedDeltaY * overlap;
					continue;
				}

				if (pushWeightB.Value == FP.Zero) {
					transformA.Position.X -= normalizedDeltaX * overlap;
					transformA.Position.Y -= normalizedDeltaY * overlap;
					continue;
				}

				var totalWeight = pushWeightA.Value + pushWeightB.Value;
				var pushAmountA = overlap * pushWeightB.Value / totalWeight;
				var pushAmountB = overlap * pushWeightA.Value / totalWeight;

				transformA.Position.X -= normalizedDeltaX * pushAmountA;
				transformA.Position.Y -= normalizedDeltaY * pushAmountA;

				transformB.Position.X += normalizedDeltaX * pushAmountB;
				transformB.Position.Y += normalizedDeltaY * pushAmountB;
			}
		}
	}
}
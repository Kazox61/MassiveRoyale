using Fixed64;
using Massive;
using Massive.Netcode;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class SeparationSystem : CoreSystem, IUpdate {
	public void Update() {
		var entityIds = new List<int>();

		foreach (var entityId in World.Include<Transform, Hitbox, PushWeight>()) {
			entityIds.Add(entityId);
		}

		var transforms = World.DataSet<Transform>();
		var hitboxes = World.DataSet<Hitbox>();
		var pushWeights = World.DataSet<PushWeight>();

		for (var i = 0; i < entityIds.Count; i++) {
			var idA = entityIds[i];

			ref var transformA = ref transforms.Get(idA);
			ref var hitboxA = ref hitboxes.Get(idA);
			ref var pushWeightA = ref pushWeights.Get(idA);

			for (var j = i + 1; j < entityIds.Count; j++) {
				var idB = entityIds[j];

				ref var transformB = ref transforms.Get(idB);
				ref var hitboxB = ref hitboxes.Get(idB);
				ref var pushWeightB = ref pushWeights.Get(idB);
				
				if (!LayerUtility.HasLayer(hitboxA.ElevationLayer, hitboxB.ElevationLayer)) {
					continue;
				}

				var deltaX = transformB.Position.X - transformA.Position.X;
				var deltaY = transformB.Position.Y - transformA.Position.Y;
				var distSqr = deltaX * deltaX + deltaY * deltaY;

				var minDist = hitboxA.Radius + hitboxB.Radius;
				if (distSqr >= minDist * minDist) {
					continue;
				}

				var dist = FP.Sqrt(distSqr);
				if (dist == FP.Zero) {
					continue;
				}

				if (pushWeightA.Value == FP.Zero && pushWeightB.Value == FP.Zero) {
					throw new InvalidOperationException($"Two immovable entities overlap: {idA} and {idB}");
				}

				var overlap = minDist - dist;
				var normalizedDeltaX = deltaX / dist;
				var normalizedDeltaY = deltaY / dist;

				if (pushWeightA.Value == FP.Zero) {
					transformB.Position.X += normalizedDeltaX * overlap;
					transformB.Position.Y += normalizedDeltaY * overlap;

					/*
					// Add perpendicular slide bias
					var tangentX = -normalizedDeltaY;
					var tangentY = normalizedDeltaX;

					var slideBias = 0.005f.ToFP(); // tune carefully
					transformB.Position.X += tangentX * slideBias;
					transformB.Position.Y += tangentY * slideBias;
					*/

					continue;
				}

				if (pushWeightB.Value == FP.Zero) {
					transformA.Position.X -= normalizedDeltaX * overlap;
					transformA.Position.Y -= normalizedDeltaY * overlap;

					/*
					// Add tiny perpendicular nudge to break deadlocks
					var tangentX = -normalizedDeltaY;
					var tangentY = normalizedDeltaX;

					var slideBias = 0.005f.ToFP(); // tune carefully
					transformA.Position.X += tangentX * slideBias;
					transformA.Position.Y += tangentY * slideBias;
					*/

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
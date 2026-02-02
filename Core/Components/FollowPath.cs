using Fixed64;

namespace MassiveRoyale.Core.Components;

public struct FollowPath {
	public int CurrentIndex;
	public Waypoint[] Waypoints;
}

public struct Waypoint {
	public FP X;
	public FP Y;
}
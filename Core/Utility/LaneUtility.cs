using Fixed64;
using MassiveRoyale.Core.Components;

namespace MassiveRoyale.Core;

public class LaneUtility {
	public static FVector2 LeftBridgeTop = new(3.5.ToFP(), 14.ToFP());
	public static FVector2 LeftBridgeBottom = new(3.5.ToFP(), 16.ToFP());
	public static FVector2 RightBridgeTop = new(14.5.ToFP(), 14.ToFP());
	public static FVector2 RightBridgeBottom = new(14.5.ToFP(), 16.ToFP());
	
	public static FP LineGateWidth = FP.One;

	public static readonly FVector2[] TopLeftLane = [
		LeftBridgeTop,
		LeftBridgeBottom
	];
	
	public static readonly FVector2[] TopRightLane = [
		RightBridgeTop,
		RightBridgeBottom
	];
	
	public static readonly FVector2[] BottomLeftLane = [
		LeftBridgeBottom,
		LeftBridgeTop
	];
	
	public static readonly FVector2[] BottomRightLane = [
		RightBridgeBottom,
		RightBridgeTop
	];
	
	public static readonly FVector2[][] LeftLanes = [
		TopLeftLane,
		BottomLeftLane
	];
	
	public static readonly FVector2[][] RightLanes = [
		TopRightLane,
		BottomRightLane
	];
	
	public static readonly FVector2[][][] AllLanes = [
		LeftLanes,
		RightLanes
	];
	
	public enum Lane {
		Left,
		Right
	}

	public static Lane GetLane(FVector2 position) {
		return position.X < GameConfig.BoardFieldWidthHalf ? Lane.Left : Lane.Right;
	}

	public static FVector2[] GetWaypoints(Team team, FVector2 position) {
		var lane = GetLane(position);
		return AllLanes[(int)lane][team.TeamIndex];
	}
}
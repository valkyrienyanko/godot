using Godot;
using System;

public partial class enemy : CharacterBody2D
{
	private TileMap tileMap;
	private NavigationAgent2D navAgent;
	protected virtual int Speed { get; set; } = 200;
	protected virtual int DistanceToTarget { get; set; } = 100;
	private Timer FindPlayerTimer = new Timer(1, active: true);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tileMap = GetParent().GetParent().GetNodeOrNull<TileMap>("TileMap");
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		navAgent.SetNavigationMap(tileMap.GetNavigationMap(0));
		navAgent.TargetDesiredDistance = DistanceToTarget;

		MoveTowardsPlayer();
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		if (FindPlayerTimer.Process(delta)) {
			GD.Print("Moving towards player", delta);
			MoveTowardsPlayer();
			FindPlayerTimer.Reset();
		}

		if (navAgent.DistanceToTarget() <= DistanceToTarget) {
			//MoveAndSlide();
			return;
		}

		Velocity =  (navAgent.GetNextPathPosition() - GlobalPosition).Normalized() * Speed;
		MoveAndSlide();
	}

	private void MoveTowardsPlayer() {
		navAgent.TargetPosition = GetParent().GetParent().GetNode<player>("Player").GlobalPosition;
	}
}
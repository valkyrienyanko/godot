namespace MyGodotGame;

public partial class projectile : RigidBody2D
{
    public double MaxLifetime { get; protected set; }
    public double Lifetime { get; protected set; }
    public decimal Damage { get;  protected set; }
    public int MaxEnemiesHit { get; protected set; } = 1;
    public int MaxBounces { get; protected set; } = 20;
    protected bool Active { get; set; } = false;
    private uint collision = Helpers.GenerateCollisionMask(true, true, false, false, false);

    public override void _Ready()
    {
        CollisionLayer = collision;
        CollisionMask = collision;
    }

    // public virtual bool HandleHitEnemy(Enemy node) {
    //     node.Hit(Damage);

    //     MaxEnemiesHit--;
    //     if (MaxEnemiesHit > 0) return true;

    //     Die();
    //     return false;
    // }

    public virtual bool HandleHitWall(TileMap node) {
        MaxBounces--;
        if (MaxBounces > 0) return true;

        Die();
        return false;
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        // for (var i = 0; i < state.GetContactCount(); i++) {
        //     var node = state.GetContactColliderObject(i);
        //     if (node is Enemy enemy) {
        //         if (!HandleHitEnemy(enemy))
        //             return;
        //     }
        //     else if (node is TileMap tilemap) {
        //         if (!HandleHitWall(tilemap))
        //             return;
        //     }
        // }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Lifetime += delta;
        if (Lifetime >= MaxLifetime)
            Die();
    }

    protected void Die() {
        GetParent().RemoveChild(this);
    }
}
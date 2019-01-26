using UnityEngine;

public class GranadeShell : Shell
{
    public Granade granadePrefab;

    public override void Shoot(Vector2 direction)
    {
        base.Shoot(direction);
        var granade = Instantiate(granadePrefab);
        granade.transform.position = transform.position;
    }
}

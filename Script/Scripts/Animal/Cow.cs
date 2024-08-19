using UnityEngine;

public class Cow : Animal
{
    public override void Reward()
    {
        Instantiate(rewards[0], transform.position + new Vector3Int(0, 1), Quaternion.identity);
    }
}

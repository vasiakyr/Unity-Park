using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player;
    public float height = 40f;

    void LateUpdate()
    {
        if(player == null) return;

        transform.position = new Vector3(
            player.position.x,
            player.position.y + height,
            player.position.z
        );

        transform.rotation = Quaternion.Euler(90f,0f,0f);
    }
}
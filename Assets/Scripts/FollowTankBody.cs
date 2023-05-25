using UnityEngine;

public class FollowTankBody : MonoBehaviour
{
    [SerializeField] Transform tankBody;

    private void Update()
    {
        transform.position = tankBody.position;
    }
}

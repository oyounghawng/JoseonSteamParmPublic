using Cinemachine;
using UnityEngine;

public class WeatherObject : MonoBehaviour
{
    CinemachineVirtualCamera vCam;

    [SerializeField]
    private ParticleSystem particle;

    private void Start()
    {
        vCam = (Managers.Scene.CurrentScene as GameScene)._camera;
    }
    private void OnEnable()
    {
        Managers.Game.onChangedPlayerLocation -= FollowPlayer;

        Managers.Game.onChangedPlayerLocation += FollowPlayer;
    }

    private void FollowPlayer(string tag)
    {
        if (tag.Equals("Building"))
        {
            transform.parent = null;
            transform.position = Vector3.zero;
        }
        else
        {
            transform.parent = vCam.transform;
            transform.localPosition = new Vector3(6, 13, 0);
        }
    }

}

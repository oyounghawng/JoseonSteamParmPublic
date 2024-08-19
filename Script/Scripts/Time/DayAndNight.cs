using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayAndNight : MonoBehaviour
{
    public Light2D light2D;

    public Gradient gradient;

    private void OnEnable()
    {
        TimeManager.Instance.onChangedTime -= ChangeLight;

        TimeManager.Instance.onChangedTime += ChangeLight;

    }

    private void ChangeLight(int time)
    {
        if(time % 60 == 0)
        {
            Color color = gradient.Evaluate(time / 1440f);

            light2D.color = color;
        }
    }
}

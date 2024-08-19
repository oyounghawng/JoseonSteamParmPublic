using UnityEngine;
using UnityEngine.UI;

public class FishingRodController : MonoBehaviour
{
    public Transform fishingBobber; // ���� ���� Transform
    public Slider powerGauge; // ������ �����̴�
    public float minThrowDistance = 3f; // �ּ� ������ �Ÿ�
    public float maxThrowDistance = 6f; // �ִ� ������ �Ÿ�
    public float gaugeSpeed = 1f; // ������ ���� �ӵ�

    private bool isCharging = false;
    private bool isThrowing = false;
    private float currentGaugeValue = 0f;


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư�� ������ ������ ���� ����
        {
            isCharging = true;
            currentGaugeValue = 0f;
            powerGauge.value = currentGaugeValue;
        }

        if (Input.GetMouseButton(0) && isCharging) // ���콺 ���� ��ư�� ������ �ִ� ���� ������ ����
        {
            currentGaugeValue += gaugeSpeed * Time.deltaTime;
            currentGaugeValue = Mathf.Clamp(currentGaugeValue, 0f, 1f);
            powerGauge.value = currentGaugeValue;
        }

        if (Input.GetMouseButtonUp(0) && isCharging) // ���콺 ���� ��ư�� ���� � ����
        {
            isCharging = false;
            isThrowing = true;
        }
    }
}
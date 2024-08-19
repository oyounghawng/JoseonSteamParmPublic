using UnityEngine;
using UnityEngine.UI;

public class FishingRodController : MonoBehaviour
{
    public Transform fishingBobber; // 낚시 찌의 Transform
    public Slider powerGauge; // 게이지 슬라이더
    public float minThrowDistance = 3f; // 최소 던지기 거리
    public float maxThrowDistance = 6f; // 최대 던지기 거리
    public float gaugeSpeed = 1f; // 게이지 증가 속도

    private bool isCharging = false;
    private bool isThrowing = false;
    private float currentGaugeValue = 0f;


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 누르면 게이지 충전 시작
        {
            isCharging = true;
            currentGaugeValue = 0f;
            powerGauge.value = currentGaugeValue;
        }

        if (Input.GetMouseButton(0) && isCharging) // 마우스 왼쪽 버튼을 누르고 있는 동안 게이지 증가
        {
            currentGaugeValue += gaugeSpeed * Time.deltaTime;
            currentGaugeValue = Mathf.Clamp(currentGaugeValue, 0f, 1f);
            powerGauge.value = currentGaugeValue;
        }

        if (Input.GetMouseButtonUp(0) && isCharging) // 마우스 왼쪽 버튼을 떼면 찌를 던짐
        {
            isCharging = false;
            isThrowing = true;
        }
    }
}
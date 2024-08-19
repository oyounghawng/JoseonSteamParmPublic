using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Rod : Tool, ITool
{
    public Slider powerGauge; // 게이지 슬라이더
    public GameObject bobber;

    public float minThrowDistance = 1.5f; // 최소 던지기 거리
    public float maxThrowDistance = 2.5f; // 최대 던지기 거리

    public float gaugeSpeed = 0.25f; // 게이지 증가 속도

    private float currentGaugeValue = 0f;

    [SerializeField]
    Transform rodRoot = null;


    [SerializeField]
    FishingController fishingController;

    PlayerAnimationController playerAnimationController;

    private bool isFising = false;
    private bool isCharging = false;
    private bool isThrowing = false;
    private bool isMouseDown = false;

    private Coroutine coThrow = null;
    private Coroutine coCharge = null;

    private void Start()
    {
        minThrowDistance = 1.5F;
        maxThrowDistance = 2.5f;
        powerGauge.minValue = minThrowDistance;
        powerGauge.maxValue = maxThrowDistance;
    }

    private void OnEnable()
    {
        TimeManager.Instance.onEndDay -= EndAnimation;
        TimeManager.Instance.onEndDay -= CancelFishing;

        TimeManager.Instance.onEndDay += EndAnimation;
        TimeManager.Instance.onEndDay += CancelFishing;
    }

    private void OnDisable()
    {
        isFising = false;
    }

    public bool Use()
    {
        return true;
    }

    private void Update()
    {
        if (isCharging)
            isMouseDown = Input.GetMouseButton(0);
    }

    public override void UseAnimation(PlayerAnimationController animationController)
    {
        if (!isFising && !isCharging && !isThrowing)
        {
            playerAnimationController = animationController;
            float x = 0;
            float y = 0;

            if (animationController != null)
            {
                x = animationController.GetXVelocity();
                y = animationController.GetYVelocity();
            }

            animator.SetFloat(hashXVelocity, x);
            animator.SetFloat(hashYVelocity, y);
            animationController.ActionTool(toolType);
            animator.SetBool(hashAction, true);

            if (coCharge != null)
                StopCoroutine(coCharge);
            coCharge = StartCoroutine(Fishing(playerAnimationController));
        }
        else if (isThrowing && !isFising)
        {
            CancelFishing();
        }
    }



    // Charge -> Throw -> Recognize Water -> StartFishing
    private IEnumerator Fishing(PlayerAnimationController controller)
    {
        isCharging = true;
        isThrowing = false;

        currentGaugeValue = powerGauge.minValue;
        powerGauge.gameObject.SetActive(true);

        isMouseDown = true;

        while (isMouseDown)
        {
            if (currentGaugeValue >= powerGauge.maxValue)
                gaugeSpeed = -0.01f;
            else if (currentGaugeValue <= powerGauge.minValue)
                gaugeSpeed = 0.01f;

            currentGaugeValue += gaugeSpeed;
            powerGauge.value = currentGaugeValue;

            yield return null;
        }

        yield return CoroutineHelper.WaitForSeconds(0.2f);

        isCharging = false;

        powerGauge.gameObject.SetActive(false);

        isThrowing = true;

        yield return ThrowBobber();

        // TODO : 물 여부 파악
        Collider2D[] colliders = Physics2D.OverlapCircleAll(bobber.transform.position, 1f);
        bool isWater = false;
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Sea"))
            {
                fishingController.onStartFishing += WaitToFish;
                fishingController.onEndFishing += CancelFishing;

                fishingController.FishingCorotuine(Define.WaterSpot.sea);
                isWater = true;
                break;
            }
            if (col.CompareTag("River"))
            {
                fishingController.onStartFishing += WaitToFish;
                fishingController.onEndFishing += CancelFishing;

                fishingController.FishingCorotuine(Define.WaterSpot.river);
                isWater = true;
                break;
            }
        }
        if (!isWater) CancelFishing();

        yield break;

    }

    // Throwing Bobber
    private IEnumerator ThrowBobber()
    {
        ActiveBobber();
        bobber.transform.localPosition = rodRoot.transform.localPosition;
        Vector3 dest = bobber.transform.localPosition + new Vector3(playerAnimationController.GetXVelocity(), playerAnimationController.GetYVelocity(), 0) * powerGauge.value;

        dest *= powerGauge.value;

        dest += playerAnimationController.GetYVelocity() != 0 ? Vector3.zero : new Vector3(0, -0.65f, 0);


        while (Vector3.Distance(bobber.transform.localPosition, dest) >= 0.05f && isThrowing)
        {
            bobber.transform.localPosition = Vector3.Lerp(bobber.transform.localPosition, dest, 0.1f);
            yield return null;
        }
        yield break;
    }

    // Active On Bobber
    public void ActiveBobber() => bobber.SetActive(true);

    // Active Off Bobber
    public void UnActiveBobber() => bobber.SetActive(false);

    // Stop to Fishing.
    private void CancelFishing()
    {
        fishingController.StopFishing();

        playerAnimationController.ActionTool(toolType, false);
        animator.SetBool(hashAction, false);

        ResetRod();
    }

    private void ResetRod()
    {
        isFising = false;
        isThrowing = false;
        isCharging = false;
        UnActiveBobber();
    }

    // 캐릭터 행동 제한 해제 이벤트
    public void EndAnimation()
    {
        onEndAnimation?.Invoke(false);
        onEndAnimation = null;
    }

    private void WaitToFish(bool active) => isFising = active;
    // Wait for Charging motion
    public void WaitToThrow()
    {
        if (coThrow != null)
            StopCoroutine(coThrow);
        coThrow = StartCoroutine(WaitThrow());
    }

    // Wait for Charging motion
    private IEnumerator WaitThrow()
    {
        animator.SetBool(hashAction, false);

        yield return new WaitUntil(() => isThrowing);
        /* 캐릭터와 낚시대 모두 던지기 실행 */
        animator.SetBool(hashAction, true);
        playerAnimationController?.ThrowRod();
        yield break;
    }
}
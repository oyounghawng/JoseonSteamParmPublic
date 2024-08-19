using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettleAccount : UI_Popup
{
    [Header("���� ������Ʈ")]
    public GameObject dDayObj = null;

    public GameObject content = null;
    public GameObject obj_Profit = null;
    public GameObject obj_Population = null;
    public GameObject obj_Reputation = null;
    public GameObject obj_Level = null;
    public GameObject obj_Apply = null;

    [Space(10)]

    [Header("�ؽ�Ʈ")]
    public TMP_Text txt_DDay = null;
    public TMP_Text txt_Profit = null; // ����
    public TMP_Text txt_CurrentTotalPopulation = null; // ���� �ֹ� ��
    public TMP_Text txt_CurrentVillageReputation = null; // ���� ���ǵ�
    public TMP_Text txt_CurrentVillageLevel = null; // ���� ����

    [Space(10)]

    [Header("��ư")]
    public Button btn_Apply;

    public bool isFinished = false;

    private Coroutine coSettleDay = null;

    private float delay = 1.0f;


    public void StartSettleDay(int gold, int population, int reputation, int level)
    {
        if (coSettleDay != null)
            StopCoroutine(coSettleDay);
        coSettleDay = StartCoroutine(SettleDay(gold, population, reputation, level));
    }

    private IEnumerator SettleDay(int gold, int population, int reputation, int level)
    {
        yield return new WaitUntil(() => Managers.Game.SaveData != null);
        dDayObj.SetActive(true);
        txt_DDay.text = $"���� ¡������ D - {28 - (TimeManager.Instance.Day)}";
        content.SetActive(true);
        SetResult(txt_Profit, obj_Profit, gold);
        yield return CoroutineHelper.WaitForSeconds(delay);
        SetResult(txt_CurrentTotalPopulation, obj_Population, population);
        yield return CoroutineHelper.WaitForSeconds(delay);
        SetResult(txt_CurrentVillageReputation, obj_Reputation, reputation);
        yield return CoroutineHelper.WaitForSeconds(delay);
        SetResult(txt_CurrentVillageLevel, obj_Level, level);
        yield return CoroutineHelper.WaitForSeconds(delay);

        txt_DDay.text = "";

        if (TimeManager.Instance.Day >= 28)
            txt_DDay.DOTMPText($"���� ¡������ D - 28", 1.0f);
        else
            txt_DDay.DOTMPText($"���� ¡������ D - {28 - (TimeManager.Instance.Day + 1)}", 1.0f);

        yield return CoroutineHelper.WaitForSeconds(delay);

        obj_Apply.SetActive(true);
    }
    public void SetResult(TMP_Text text, GameObject obj, int number)
    {
        text.text = "";
        obj.SetActive(true);
        text.gameObject.SetActive(true);
        StartCoroutine(number.CountNumber(text, 1));
    }

    // TODO : �������� �ѱ�� ���� �õ��ϴ� �������� �Ѿ�� �ؾ���
    public void Apply()
    {
        isFinished = true;
        ClosePopupUI();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_Dialogue : UI_Popup
{
    public NPCDialogue dialogue;
    public List<Sprite> npcExpression = null;

    [SerializeField] private Image Img_Profile;
    [SerializeField] private TMP_Text txt_ProfileName;
    [SerializeField] private TMP_Text txt_Context;
    [SerializeField] private Image Img_NextIndicator;
    [SerializeField] private List<Choice> choices = new List<Choice>();

    public event Action onEndDialogue = null;

    private int contextIndex = 0;
    private int choiceIndex = 0;

    private bool isNext = false;
    private bool isTyping = false;
    private bool isChoice = false;
    private bool isEnd = false;
    private float typingDelay = 0.05f;

    /* Initialize Dialogue */
    public override void Init()
    {
        base.Init();
        Reset();
        SetContext();
    }

    private void OnEnable()
    {
        TimeManager.Instance.onEndDay -= ClosePopupUI;
        TimeManager.Instance.onEndDay += ClosePopupUI;
    }
    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }

    public void SetDialogue(NPCDialogue dialogue)
    {
        this.dialogue = dialogue;
    }
    public void SetDialogue(NPCDialogue dialogue, List<Sprite> npcExpression)
    {
        SetDialogue(dialogue);
        this.npcExpression = npcExpression;
    }

    public void NextContext(int index)
    {
        isChoice = false;
        contextIndex = index;
        OffChoice();
        SetContext();
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (context.phase == InputActionPhase.Started)
            {
                // 타이핑 중이면 다음 대사로 넘기지 않음
                if (isTyping || isChoice || !isNext) return;

                // 타이핑이 끝난 상태라면 다음 대사를 설정
                if (dialogue.context != null && dialogue.context.Count > contextIndex)
                {
                    SetContext();
                }
                else
                {
                    EndDialogue();
                }
            }

        }

    }

    private void SetContext()
    {
        txt_Context.text = "";

        if (dialogue.context != null && dialogue.context.Count > contextIndex)
        {
            txt_ProfileName.text = dialogue.displayName;

            StartCoroutine(Typing(dialogue.context[contextIndex]));
            contextIndex++;
        }
    }

    // Typing the Text
    private IEnumerator Typing(string context)
    {
        if (string.IsNullOrEmpty(context)) yield break;

        isTyping = true;

        isNext = false;
        Img_NextIndicator.gameObject.SetActive(false);

        string modifiedContext = ProcessContext(context);
        txt_Context.text = "";

        foreach (char letter in modifiedContext)
        {
            txt_Context.text += letter;
            Managers.Sound.Play(Define.Sound.Effect, "Effect/DialogueTyping", Managers.Sound.EffectVolume, 2.0f);
            yield return CoroutineHelper.WaitForSeconds(typingDelay);
        }

        if (isChoice)
        {
            DisplayChoices();
            Img_NextIndicator.gameObject.SetActive(false);
        }
        else
        {
            Img_NextIndicator.gameObject.SetActive(true);
        }

        isTyping = false;
        isNext = true;
        yield break;

    }

    // Process Special Symbol
    private string ProcessContext(string context)
    {
        if (string.IsNullOrEmpty(context)) return string.Empty;

        var contextActions = new Dictionary<char, Action>
        {
            { '⒮', () => isChoice = true },
            { '⒫', () => ReplacePlayerName(ref context) },
            { '⒨', () => OpenShop() },
            { '⒲', () => { /* TODO: 추가 작업 */ } },
            { '⒠', () => {isEnd = true;} },
            { '⒱', () => ReplaceVillageName(ref context) },
            { '⒩', () => ReplaceWithRandomNormalDialogue(ref context) },
            { '①', () => SetProfileImage(0) },
            { '②', () => SetProfileImage(1) },
            { '③', () => SetProfileImage(2) },
            { '④', () => SetProfileImage(3) },
            { 'ⓢ', () =>  ShowVillageInfo() },
            { 'ⓥ', () => ShowVillageNPCInfo()}
        };

        foreach (var action in contextActions)
        {
            int symbolIndex = context.IndexOf(action.Key);
            if (symbolIndex >= 0)
            {
                action.Value.Invoke();
                context = context.Replace(action.Key, ' ');
            }
        }

        return context;
    }

    private void ReplacePlayerName(ref string context)
    {
        context = context.Replace("⒫", Managers.Game.SaveData.playerName);
    }

    /* Shopper NPC */
    private async void OpenShop()
    {
        onEndDialogue?.Invoke();
        ClosePopupUI();
        await Managers.UI.ShowTaskPopupUI<UI_Shop>();

    }

    private async void ShowVillageInfo()
    {
        onEndDialogue?.Invoke();
        ClosePopupUI();
        await Managers.UI.ShowTaskPopupUI<UI_VillageStatus>();
    }

    private async void ShowVillageNPCInfo()
    {

    }
    private void ReplaceVillageName(ref string context)
    {
        context = context.Replace("⒱", Managers.Game.SaveData.villageName);
    }

    /* Find Normal NPC Normal Context */
    private void ReplaceWithRandomNormalDialogue(ref string context)
    {
        if (Managers.Data.NPCDialogueContainer.TryGetValue(dialogue.rcode, out NPCDialogueData value))
        {
            var randomDialogue = value.Value.dialogueGroup.Find(x => x.displayName.Equals(dialogue.displayName) && x.key.Equals("Normal_Context"));
            SetDialogue(randomDialogue);

            if (randomDialogue != null && randomDialogue.context.Count > 0)
            {
                contextIndex = Random.Range(0, randomDialogue.context.Count);
                SetContext();
            }
        }
    }

    private void SetProfileImage(int index)
    {
        if (npcExpression != null && index >= 0 && index < npcExpression.Count)
        {
            Img_Profile.sprite = npcExpression[index];
        }
    }

    /* Display Choices Text And Button */
    private void DisplayChoices()
    {
        if (dialogue.choices != null && dialogue.choices.Count > choiceIndex)
        {
            for (int i = 0; i < dialogue.choices[choiceIndex].choiceText.Count && i < choices.Count; i++)
            {
                choices[i].SetChoice(dialogue.choices[choiceIndex], ProcessContext(dialogue.choices[choiceIndex].choiceText[i]));
            }
            choiceIndex++;
        }
    }

    private void OffChoice()
    {
        choices.ForEach(choice => choice.gameObject.SetActive(false));
    }

    private void Reset()
    {
        contextIndex = 0;
        choiceIndex = 0;
        isTyping = false;
        isChoice = false;
        isEnd = false;
        Img_NextIndicator.gameObject.SetActive(false);
        OffChoice();
    }

    private void EndDialogue()
    {
        onEndDialogue?.Invoke();
        ClosePopupUI();
    }
}
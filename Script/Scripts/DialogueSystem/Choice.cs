using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Choice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UI_Dialogue controller;
    public NPCChoice choice;

    public Button choiceButton;

    public Outline choiceOutline;

    public TMP_Text choiceText;

    public int index;
    public void SetChoice(NPCChoice choice, string text)
    {
        this.choice = choice;
        choiceText.text = text;
        gameObject.SetActive(true);
        choiceButton.onClick.AddListener(SelectChoice);
    }

    private void SelectChoice()
    {
        choiceButton.onClick.RemoveListener(SelectChoice);
        controller.NextContext(choice.nextDialogue[index]);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        choiceOutline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        choiceOutline.enabled = false;
    }
}

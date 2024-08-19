using UnityEngine;

public class Bed : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowUI();
        }
    }
    private async void ShowUI()
    {
        await UIManager.Instance.ShowTaskPopupUI<UI_Bed>();
    }
}

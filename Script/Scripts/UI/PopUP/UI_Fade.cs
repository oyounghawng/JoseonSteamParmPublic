using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UI_Fade : UI_Popup
{
    public Image Img_Fade;

    private void OnEnable()
    {
        Managers.Game.onGameStart += OnCompletedTransition;
    }

    public async Task FadeIn()
    {
        Img_Fade.color = new Color(0, 0, 0, 0);

        await Img_Fade.DOFade(1.0f, 2.0f).AsyncWaitForCompletion();
    }

    public async Task FadeOut()
    {
        Img_Fade.color = new Color(0, 0, 0, 1);

        await Img_Fade.DOFade(0.0f, 1.0f).AsyncWaitForCompletion();
    }



    public void FadeIn(float duration)
    {
        Sequence fadeSequence = DOTween.Sequence()
            .SetAutoKill(false)
            .OnRewind(() =>
            {
                Img_Fade.color = new Color(0, 0, 0, 0);
            })
            .Append(Img_Fade.DOFade(1, duration))
            .OnComplete(() => ClosePopupUI());
    }


    private void OnCompletedTransition() => FadeOut(1);
    public void FadeOut(float duration)
    {
        Sequence fadeSequence = DOTween.Sequence()
            .SetAutoKill(false)
            .OnRewind(() =>
            {
                Img_Fade.color = new Color(0, 0, 0, 1);
            })
            .Append(Img_Fade.DOFade(0.0f, duration))
            .OnComplete(() => ClosePopupUI());
    }

}

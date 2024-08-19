using UnityEngine;
using UnityEngine.UI;
public interface IAnimalReward
{
    void Reward();
}
public abstract class Animal : MonoBehaviour, IAnimalReward
{
    protected float affection;
    protected readonly float maxAffection = 100;
    public Image affectionImage;

    public float Affection
    {
        get
        {
            return affection;
        }
        set
        {
            affection += value;

            if (affection >= maxAffection)
            {
                affection = maxAffection;
            }
            affectionImage.fillAmount = affection / maxAffection;
        }
    }
    public GameObject[] rewards;

    protected void Awake()
    {
        affection = 30;
        affectionImage.fillAmount = affection / maxAffection;
    }

    public virtual void Reward()
    {

    }
}

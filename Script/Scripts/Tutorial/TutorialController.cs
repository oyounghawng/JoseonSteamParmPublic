using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private List<TutorialBase> tutorials = new List<TutorialBase>();
    [SerializeField]
    private string nextSceneName = string.Empty;

    private TutorialBase currentTutorial = null;
    private int currentIndex = -1;


    public Queue<TutorialCheck> tutorialQueue = new Queue<TutorialCheck>();
    public delegate bool TutorialCheck(GameObject targetObject);

    private void Start()
    {
        foreach (Transform child in this.transform)
        {
            tutorials.Add(child.GetComponent<TutorialBase>());
        }
        InitQueueCondition();
        SetNextTutorial();
    }

    private void InitQueueCondition()
    {
        tutorialQueue.Enqueue(CheckPlowTile);
        tutorialQueue.Enqueue(CheckPlant);
        tutorialQueue.Enqueue(CheckWet);
        tutorialQueue.Enqueue(HasItem);
    }

    private void Update()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Exit();
        }

        //마지막 튜토리얼을 진행했다면 게임 시작
        if (currentIndex >= tutorials.Count - 1)
        {
            CompletedAllTutorials();
            return;
        }

        currentIndex++;
        currentTutorial = tutorials[currentIndex];
        currentTutorial.Enter();
    }

    public void CompletedAllTutorials()
    {
        currentTutorial = null;
        (Managers.Scene.CurrentScene as GameScene).player.GetComponent<PlayerInputController>().isMovementRestricted = false;
        Debug.Log("Completed All");
        Managers.Resource.Destroy(this.gameObject.transform.parent.gameObject);
    }

    #region FarmTutorial
    private bool CheckPlowTile(GameObject gameObject)
    {
        if (GridManager.Instance.CompareTile(gameObject.transform.position, true))
        {
            return true;
        }
        return false;
    }

    private bool CheckPlant(GameObject gameObject)
    {
        if (GridManager.Instance.IsCropPlanted(gameObject.transform.position))
        {
            return true;
        }
        return false;
    }

    private bool CheckWet(GameObject gameObject)
    {
        if (GridManager.Instance.CompareTile(gameObject.transform.position, false))
        {
            return true;
        }
        return false;
    }

    private bool HasItem(GameObject gameObject)
    {
        if ((Managers.Scene.CurrentScene as GameScene).player.HasPlayerItem(gameObject.GetComponent<ItemObject>().itemObjectData))
        {
            return true;
        }
        return false;
    }
    #endregion
}

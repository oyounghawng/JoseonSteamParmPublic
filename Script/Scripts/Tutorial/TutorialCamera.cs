using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCamera : TutorialBase
{
    private float currentTime = 0;
    public float maxTime = 3.0f;
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera riverCamera;
    public CinemachineBrain brain;

    public override void Enter()
    {
        playerCamera = (Managers.Scene.CurrentScene as GameScene)._camera;
        brain = Camera.main.GetComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Linear;
        playerCamera.enabled = false;
        riverCamera.enabled = true;
    }

    public override void Execute(TutorialController controller)
    {
        currentTime += Time.deltaTime;

        if(currentTime > maxTime)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        playerCamera.enabled = true;
        riverCamera.enabled = false;
    }
}

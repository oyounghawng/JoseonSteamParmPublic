using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCData
{
    [Header("�⺻ ����")]
    // ���ҽ� ��ȣ
    public string rcode;

    // ��µǴ� �̸�
    public string displayName;

    // NPC ����
    public string description;

    public List<Sprite> portraits = new List<Sprite>();

    // NPC ����
    public Define.Gender gender;

    // NPC ����
    public Define.NPCType type;

    // NPC ����
    public Define.NPCRole role;

    public Define.NPCRank rank;

    public Vector3 homePos;
    public string homeCode;
    [Space(10)]

    [Header("�����")]
    public CharacterLooks looks;

    [Space(10)]

    [Header("ȣ����")]
    public int companionshipExp = 0;

    [Space(10)]

    [Header("���õ�")]
    // ��� ���õ�
    public float farmingSkillLevel;

    // ���� ���õ�
    public float fishingSkillLevel;

    public string preferredItem;

    public string dislikedItem;
}

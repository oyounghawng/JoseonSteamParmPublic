using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCData
{
    [Header("기본 상태")]
    // 리소스 번호
    public string rcode;

    // 출력되는 이름
    public string displayName;

    // NPC 설명
    public string description;

    public List<Sprite> portraits = new List<Sprite>();

    // NPC 성별
    public Define.Gender gender;

    // NPC 종류
    public Define.NPCType type;

    // NPC 역할
    public Define.NPCRole role;

    public Define.NPCRank rank;

    public Vector3 homePos;
    public string homeCode;
    [Space(10)]

    [Header("생김새")]
    public CharacterLooks looks;

    [Space(10)]

    [Header("호감도")]
    public int companionshipExp = 0;

    [Space(10)]

    [Header("숙련도")]
    // 농사 숙련도
    public float farmingSkillLevel;

    // 낚시 숙련도
    public float fishingSkillLevel;

    public string preferredItem;

    public string dislikedItem;
}

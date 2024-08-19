using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDataSO : ScriptableObject
{
    #region Status
    /* NPC 기본 정보 */
    public string rcode;
    public string displayname;
    public string description;
    public Define.Gender gender;
    public Define.NPCType type;
    public Define.NPCRank rank;
    public Define.NPCRole role;
    #endregion

    #region

    /* NPC 의상 정보 */

    public string hat;
    public string hair;
    public string blush;
    public string eye;
    public string lipstick;
    public string body;
    public string top;
    public string under;
    public string shoes;
    public string accessory1;
    public string accessory2;

    #endregion

}
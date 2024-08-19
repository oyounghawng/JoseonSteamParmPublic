using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public abstract class CharacterCustomSelector : MonoBehaviour
{
    protected List<SpriteLibraryAsset> assets;

    // 캐릭터의 Resolver
    public SpriteResolver charResolver;

    // Selector의 담당 부위
    public Define.CharacterParts parts;

    public Toggle toggle;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        assets = Managers.Resource.LoadAll<SpriteLibraryAsset>($"CharacterParts/{gameObject.name}").ToList();
    }

    protected virtual void OnEnable()
    {
        toggle.onValueChanged.AddListener(SetResolverActive);
    }

    protected virtual void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(SetResolverActive);
    }

    // 다음으로 넘기는 기능
    protected virtual void NextParts()
    {

    }

    // 이전으로 되돌리는 기능
    protected virtual void PrevParts()
    {

    }

    public void SwapParts(SpriteLibraryAsset asset)
    {
        charResolver.spriteLibrary.spriteLibraryAsset = null;
        charResolver.spriteLibrary.RefreshSpriteResolvers();
        charResolver.spriteLibrary.spriteLibraryAsset = asset;
    }
    public virtual void ResetCharacterParts()
    {

    }
    protected void SetResolverActive(bool active)
    {
        charResolver.gameObject.SetActive(active);
    }
}
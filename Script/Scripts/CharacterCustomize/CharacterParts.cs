using UnityEngine;
using UnityEngine.U2D.Animation;

public class CharacterParts : MonoBehaviour
{
    public Define.CharacterParts parts;

    public SpriteResolver resolver;

    public void SetParts(SpriteLibraryAsset asset)
    {
        resolver.spriteLibrary.spriteLibraryAsset = null;
        resolver.spriteLibrary.RefreshSpriteResolvers();
        resolver.spriteLibrary.spriteLibraryAsset = asset;
        resolver.spriteLibrary.RefreshSpriteResolvers();

    }
}
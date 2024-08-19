public class Define
{
    public enum PlayerBehavior
    {
        None,
        Action
    }
    public enum SceneType
    {
        Unknown = -1,
        LogoScene = 0,
        GameScene,
        LoadingScene,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum Tag
    {
        Item,
        WoodFloor,
        MudGround,
        River,
        Sea,
        Building,
        TutorialTrigger,
        NPC,
    }
    //bit flag
    public enum LayerMask
    {
        Player = 6,
        NPC,
        Animal,
        Interactable,
        Level,
        Resources
    }
    public enum UIEvent
    {
        Click,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag
    }

    public enum ItemType
    {
        FarmItem = 1,
        FishingItem,
        AnimalItem,
        ToolItem,
        BuildItem,
        GoldItem,
        ResourceItem,
    }

    #region NPC

    public enum NPCRole
    {
        Farmer,
        Fisher,
        Seller,
        Laborer,
        Costumier,
        Gersang,
        Overpacker,
        MaxCount
    }

    public enum NPCType
    {
        Normal,
        Main,
    }

    public enum Gender
    {
        Male,
        Female,
        MaxCount
    }

    public enum NPCRank
    {
        Slave = 1,
        Ordinary = 2,
        Middle = 3,
        Novle = 4,
        Royal = 5
    }

    public enum NPCBehavior
    {
        Look = -1,
        Axe,
        PickAxe,
        Hoe,
        Water,
        Fishing,
        SwordSwing,
        ManagerShop
    }

    public enum NPCExpression
    {
        Normal,
        Smile,
        Sad,
        Angry
    }
    #endregion

    #region Tilemap 

    public enum TilemapType
    {
        WalkableTileMap,
        InteractableMap,
        WaterInteractableMap,
    }

    public enum WaterSpot
    {
        sea,
        river
    }

    #endregion

    #region TimeManager

    public enum Weather
    {
        Sunny = 1,
        Rainy,
        ThunderStorm,
        Snow
    }

    public enum Season
    {
        Spring = 1,
        Summer = 2,
        Fall = 3,
        Winter = 4
    }

    public enum Day
    {
        Mon = 1,
        Tue = 2,
        Wed = 3,
        Thu = 4,
        Fri = 5,
        Sat = 6,
        Sun = 0
    }
    #endregion

    #region CharacterCustomize

    public enum CharacterParts
    {
        Hat,
        Hair,
        Eye,
        Blush,
        Lipstick,
        Body,
        Top,
        Under,
        Shoes,
        Accessory1,
        Accessory2
    }

    public enum CharacterDirection
    {
        Front,
        Right,
        Back,
        Left,
    }

    #endregion

    #region Tool

    public enum Tool
    {
        Axe,
        PickAxe,
        Hoe,
        Rod,
        WateringCan,
        Sword,
        FishTrap
    }

    #endregion
}

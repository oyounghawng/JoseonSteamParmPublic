[System.Serializable]
public class CharacterLooks
{
    public string hatType;
    public string hairType;
    public string eyeType;
    public string blushType;
    public string lipType;
    public string bodyType;
    public string topType;
    public string underType;
    public string shoesType;
    public string accessory1Type;
    public string accessory2Type;

    public CharacterLooks()
    {
        hatType = "";
        hairType = "bob_Black";
        eyeType = "eyes_Black";
        blushType = "";
        lipType = "lipstick_1";
        bodyType = "char1";
        topType = "basic_Black";
        underType = "pants_Black";
        shoesType = "shoes_Black";
        accessory1Type = "";
        accessory2Type = "";
    }

    public CharacterLooks(string hatType, string hairType, string eyeType, string blushType, string lipType, string bodyType, string topType, string underType, string shoesType, string accessory1Type, string accessory2Type)
    {
        this.hatType = hatType;
        this.hairType = hairType;
        this.eyeType = eyeType;
        this.blushType = blushType;
        this.lipType = lipType;
        this.bodyType = bodyType;
        this.topType = topType;
        this.underType = underType;
        this.shoesType = shoesType;
        this.accessory1Type = accessory1Type;
        this.accessory2Type = accessory2Type;
    }

}

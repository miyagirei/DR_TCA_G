using UnityEngine;

//ここで新しいキャラクターを増やした後、Resource/Characterの中に画像を入れて、画像のPivotをTopにして、MaxSizeを512にしてください
public enum CharacterType
{
    NULL,
    Monokuma,
    NaegiMakoto,
    CelestiaLudenberg,
    YamadaHifumi,
    EnoshimaJunko,
    MAX,
}

public class CharacterTypeInfomation
{
    public string GetCharacterFile(CharacterType character, bool hope = false, bool despair = false)
    {
        string file_name = "Character/";
        switch (character)
        {
            case CharacterType.Monokuma:
                file_name += "monokuma_" + GetNumber(hope, despair);
                break;
            case CharacterType.NaegiMakoto:
                file_name += "naegi_" + GetNumber(hope , despair);
                break;
            case CharacterType.CelestiaLudenberg:
                file_name += "celestia_" + GetNumber(hope , despair);
                break;
            case CharacterType.YamadaHifumi:
                file_name += "yamada_" + GetNumber(hope , despair);
                break;
            case CharacterType.EnoshimaJunko:
                file_name += "enoshima_" + GetNumber(hope , despair);
                break;
            default:
                file_name = null;
                break;
        }

        return file_name;
    }

    public string GetCharacterImage(CharacterType character)
    {
        string file_name = "Character/";
        switch (character)
        {
            case CharacterType.Monokuma:
                file_name += "monokuma_03";
                break;
            case CharacterType.NaegiMakoto:
                file_name += "naegi_03";
                break;
            case CharacterType.CelestiaLudenberg:
                file_name += "celestia_03";
                break;
            case CharacterType.YamadaHifumi:
                file_name += "yamada_03";
                break;
            case CharacterType.EnoshimaJunko:
                file_name += "enoshima_03";
                break;
            default:
                file_name = null;
                break;
        }

        return file_name;
    }

    string GetNumber(bool hope, bool despair) {
        if (hope)
        {
             return "01";
        }
        else if (despair)
        {
            return "02";
        }
        return "00";
    }
}
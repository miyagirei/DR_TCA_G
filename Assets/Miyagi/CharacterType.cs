using UnityEngine;
public enum CharacterType
{
    NULL,
    Monokuma,
    NaegiMakoto,
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
public enum CharacterType
{
    NULL,
    Monokuma,
    NaegiMakoto,
    MAX,
}

public class CharacterTypeInfomation
{
    public string GetCharacterFile(CharacterType character, Player player)
    {
        if (player.GetHopeCondition())
        {
            switch (character)
            {
                case CharacterType.Monokuma:
                    return "monokuma_01";
                case CharacterType.NaegiMakoto:
                    return "naegi_01";
            }
        }        
        
        if (player.GetDespairCondition())
        {
            switch (character)
            {
                case CharacterType.Monokuma:
                    return "monokuma_02";
                case CharacterType.NaegiMakoto:
                    return "naegi_02";
            }
        }

        switch (character)
        {
            case CharacterType.Monokuma:
                return "monokuma_00";
            case CharacterType.NaegiMakoto:
                return "naegi_00";
            default:
                return null;
        }
    }

}
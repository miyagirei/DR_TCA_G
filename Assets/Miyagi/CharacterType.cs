public enum CharacterType {
    NULL,
    Monokuma,
    NaegiMakoto,
    MAX,
}

public class CharacterTypeInfomation{
    public string GetCharacterFile(CharacterType character)
    {
        switch (character) {
            case CharacterType.Monokuma:
                return "stand_15_00";
            case CharacterType.NaegiMakoto:
                return "stand_00_18";
            default:
                return null;
        }
    }
  
}
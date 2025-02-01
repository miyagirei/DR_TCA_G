
public class Situation
{
    public enum PlayingSituation
    {
        Hopeful,
        Desperate
    }

    PlayingSituation _situation = PlayingSituation.Hopeful;
    public PlayingSituation GetSituation() => _situation;

    public void SetSituation(PlayingSituation situation) => _situation = situation; 
}

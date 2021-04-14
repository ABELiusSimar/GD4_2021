public static class PlayerStats
{
    private static string level;


    public static string Level
    {
        get
        {
            if(level == null){

                return "Seliana";
            }
            return level;
        }
        set
        {
            level = value;
        }
    }
}
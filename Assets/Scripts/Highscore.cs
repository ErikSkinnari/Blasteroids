using System;

[Serializable]
public class Highscore
{
    public Highscore(string playername, float score, 
                    int asteroidsBlasted,int missilesFired,
                    int levelReached, float skillLevel)
    {
        Playername = playername;
        Score = score;
        AsteroidsBlasted = asteroidsBlasted;
        MissilesFired = missilesFired;
        LevelReached = levelReached;
        SkillLevel = skillLevel;
    }

    public string Playername;
    public float Score;
    public int AsteroidsBlasted;
    public int MissilesFired;
    public int LevelReached;
    public float SkillLevel;
}
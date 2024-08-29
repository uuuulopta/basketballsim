using System.Text.Json.Serialization;

namespace basketballSim;

public class Team
{
    public Team(string name, string isoCode, int fibaRanking)
    {
        Name = name;
        ISOCode = isoCode;
        FIBARanking = fibaRanking;
    }

    [JsonPropertyName("Team")]
   public string Name { get; init; }
   public string ISOCode { get; init; }
   public int FIBARanking { get; init; }
   public int Loses { get; private set; } = 0;
   public int Wins { get; private set; } = 0;
   public int Points { get; private set; } = 0;
   public int ScoreGained { get; private set; } = 0;
   public int ScoreLost { get; private set; } = 0;
   public int MatchPointsGained { get; private set; } = 0;
   public int MatchPointsLost { get; private set; } = 0;
   private Dictionary<string, int> scoreAgainstTeam = new();
   private Dictionary<string, int> pointsAgainstTeam = new();

   /// <summary>
   /// Returns Wins - Loses against a team
   /// </summary>
   public int GetScoreAgainstTeam(Team team)
   {
       return scoreAgainstTeam.GetValueOrDefault(team.ISOCode);
   }

   /// <summary>
   /// Retruns amount of points gained in all matches against a team
   /// </summary>
   public int GetPointsAgainstTeam(Team team)
   {
       return scoreAgainstTeam.GetValueOrDefault(team.ISOCode);
   }

   public bool PlayedAgainst(Team team)
   {
       return scoreAgainstTeam.ContainsKey(team.ISOCode);
   }
   public void ImportMatch(MatchStats stats)
   {
       switch (stats.Result)
       {
           case MatchResult.Win:
               Points += 2;
               Wins++;
               scoreAgainstTeam[stats.Against.ISOCode] = GetScoreAgainstTeam(stats.Against) + 1;
               break;
           case MatchResult.Lose:
               Points += 1;
               Loses++;
               scoreAgainstTeam[stats.Against.ISOCode] = GetScoreAgainstTeam(stats.Against) - 1;
               break;
           case MatchResult.Forfeit:
               Loses++;
               scoreAgainstTeam[stats.Against.ISOCode] = GetScoreAgainstTeam(stats.Against) - 1;
               break;
       }

       ScoreGained += stats.ScoreGained;
       ScoreLost += stats.ScoreLost;
       MatchPointsGained += stats.Points;
       MatchPointsLost += stats.PointsLost;
       pointsAgainstTeam[stats.Against.ISOCode] = GetPointsAgainstTeam(stats.Against) + stats.Points;
   }

   public static int CompareTeamsForGroups(Team a, Team b)
   {
       //Timovi u okviru grupe se rangiraju na osnovu broja bodova
       if (a.Points > b.Points)
           return 1;
       if (a.Points < b.Points)
           return -1;
/*     U Slučaju da dva tima iz iste grupe imaju isti broj bodova, rezultat međusobnog susreta će biti korišćen kao
       kriterijum za rangiranje. U slučaju da 3 tima iz iste grupe imaju isti broj bodova, kriterijum za rangiranje biće
       razlika u poenima u međusobnim utakmicama između ta 3 tima
 */
       int aPoints = a.pointsAgainstTeam.GetValueOrDefault(b.ISOCode);
       int bPoints = b.pointsAgainstTeam.GetValueOrDefault(a.ISOCode);
       if (aPoints > bPoints)
           return 1;
       if (aPoints < bPoints)
           return -1;
       return 0;

   }

   public static int CompareTeamsForPlasman(Team a, Team b)
   {
       //Rangiraju po broju bodova,
       if (a.Points > b.Points)
           return 1;
       if (a.Points < b.Points)
           return -1;
       //Zatim koš razlici
       if (a.MatchPointsGained - a.MatchPointsLost > b.MatchPointsGained - b.MatchPointsLost)
           return 1;
       if (a.MatchPointsGained - a.MatchPointsLost < b.MatchPointsGained - b.MatchPointsLost)
           return -1;
       //Zatim broja postignutih koševa
       if (a.ScoreGained > b.ScoreGained)
           return 1;
       if (a.ScoreGained < b.ScoreGained)
           return -1;
       return 0;
   }
   public double CalculateShotChangeAgainst(Team opposingTeam)
   {
       double againstFactor = LogisticFunc.calculate(GetScoreAgainstTeam(opposingTeam));
       double totalScoreFactor = LogisticFunc.calculate(Wins - Loses);
       double fibaFactor = Math.Clamp(opposingTeam.FIBARanking - FIBARanking, 0, 20) / 100d;
       return 0.3d + fibaFactor + againstFactor + totalScoreFactor;
   }
}
namespace basketballSim;

public class Match
{

    public Team a { get; }
    public Team b { get; }
    private int pointsA;
    private int scoreA;
    private int pointsB;
    private int scoreB;
    public Team? Winner { get; private set; }
    public Team? Loser { get; private set; }

    public Match(Team a, Team b)
    {
        this.a = a;
        this.b = b;
    }
    public Match Simulate()
    {
        int shotsA = Random.Shared.Next(90, 100);
        int shotsB = Random.Shared.Next(90, 100);
        double chanceA = a.CalculateShotChangeAgainst(b);
        double chanceB = b.CalculateShotChangeAgainst(a);
        (pointsA,scoreA) = SimulateShots(chanceA,shotsA);
        (pointsB,scoreB) = SimulateShots(chanceB,shotsB);
        while (pointsA == pointsB)
        {
            shotsA = Random.Shared.Next(1,5);
            shotsB = Random.Shared.Next(1,5);
            (int pA, int scA) = SimulateShots(chanceA, shotsA);
            (int pB, int scB) = SimulateShots(chanceA, shotsA);
            pointsA += pA;
            pointsB += pB;
            scoreA += scA;
            scoreB += scB;
        }

        Winner = pointsA > pointsB ? a : b;
        Loser = pointsA < pointsB ? a : b;
        (MatchStats statsA, MatchStats statsB) = GetStats();
        a.ImportMatch(statsA);
        b.ImportMatch(statsB);

        return this;
    }

    public (MatchStats statsA,MatchStats statsB) GetStats()
    {
        return (new MatchStats(a,b, pointsA,pointsB, scoreA, scoreB), new MatchStats(b, a, pointsB,pointsA, scoreB, scoreA));
    }

    public override string ToString()
    {
        return $"{a.Name} - {b.Name} ({pointsA}:{pointsB})";
    }
    private (int points,int shotsMade) SimulateShots(double chance, int shots)
    {
        int points = 0;
        int shotsMade = 0;
        for (int i = 0; i < shots; i++)
        {
            if (chance*100 >= Random.Shared.Next(100))
            { 
                points += Random.Shared.Next(0,4);
                shotsMade += 1;
            }
        }
        return ( points, shotsMade );
    }

}
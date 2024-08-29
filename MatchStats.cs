namespace basketballSim;
    public struct MatchStats
    {
        public MatchStats(Team team,Team against, int points, int pointsLost, int scoreGained, int scoreLost)
        {
            Team = team;
            Against = against;
            Points = points;
            PointsLost = pointsLost;
            ScoreGained = scoreGained;
            ScoreLost = scoreLost;
            if (scoreGained > scoreLost) Result = MatchResult.Win;
            else if (scoreGained < scoreLost) Result = MatchResult.Lose;
        }

        public Team Team { get; }
        public Team Against { get; }
        public int Points { get; }      = 0;
        public int PointsLost { get; }      = 0;
        public int ScoreGained { get; } = 0 ;
        public int ScoreLost { get; }   = 0;

        public MatchResult Result { get; }


    }

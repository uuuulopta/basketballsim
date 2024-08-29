using System.Text.Json;
using System.Text.Json.Nodes;
using basketballSim;


// Importovanje grupa
string groupsJson = File.ReadAllText("./groups.json");
var parsedJson = JsonNode.Parse(groupsJson)!.AsObject();
Dictionary<string, List<Team>> groups = new();
foreach (var group in parsedJson)
{
   groups[group.Key] = group.Value!.AsArray().Select(t => t.Deserialize<Team>()).ToList()!;
}

// Pravljenje meceva po grupama
Dictionary<string,Stack<Match>> matches = new();
foreach (var entry in groups)
{
   var teams = entry.Value;
   Stack<Match> groupMatches = new();
   groupMatches.Push(new Match(teams[0],teams[1]));
   groupMatches.Push(new Match(teams[2],teams[3]));
   groupMatches.Push(new Match(teams[0],teams[2]));
   groupMatches.Push(new Match(teams[1],teams[3]));
   groupMatches.Push(new Match(teams[0],teams[3]));
   groupMatches.Push(new Match(teams[1],teams[2]));
   matches.Add(entry.Key,groupMatches);
}

// Igranje grupnih meceva
for (int i = 1; i < 4; i++)
{
   Console.WriteLine($"Grupna faza {i}. kolo:");
   foreach (var entry in matches)
   {
      Console.WriteLine($"\tGrupa {entry.Key}:");
      Console.WriteLine($"\t\t{entry.Value.Pop().Simulate()}");
      Console.WriteLine($"\t\t{entry.Value.Pop().Simulate()}");
   }
}

Console.WriteLine("Konacan plasman u grupama:");
List<List<Team>> Plasmani = [ [], [], [] ];
foreach (var group in groups)
{
   var teams = group.Value;
   teams.Sort(Team.CompareTeamsForGroups);
   teams.Reverse();
   Console.WriteLine($"\tGrupa {group.Key}\t (Ime - pobede / porazi / bodovi / postignuti koševi / primljeni koševi / koš razlika)");
   for (int i = 0; i < teams.Count; i++)
   {
      var team = teams[i];
      Console.WriteLine($" \t \t{i+1}. {team.Name}  {team.Wins} / {team.Loses} / {team.Points} / {team.ScoreGained} / {team.ScoreLost} / {team.MatchPointsGained - team.MatchPointsLost}");
   }

   Plasmani[0].Add(teams[0]);
   Plasmani[1].Add(teams[1]);
   Plasmani[2].Add(teams[2]);
}
// Rangiranje za plasmane
Plasmani.ForEach(lt => {lt.Sort(Team.CompareTeamsForPlasman); lt.Reverse(); });

Dictionary<string, Team[]> Hats = new();
Hats.Add("D",new Team[]{Plasmani[0][0],Plasmani[0][1]});
Hats.Add("E",new Team[]{Plasmani[0][2],Plasmani[1][0]});
Hats.Add("F",new Team[]{Plasmani[1][1],Plasmani[1][2]});
Hats.Add("G",new Team[]{Plasmani[2][0],Plasmani[2][1]});

Console.WriteLine("Šeširi:");
foreach (var entry in Hats)
{
   Console.WriteLine($"\tŠešir {entry.Key}:");
   Console.WriteLine($"\t\t{entry.Value[0].Name}");
   Console.WriteLine($"\t\t{entry.Value[1].Name}");
}


// D sa G i E sa F
(Match m1, Match m2) = CreateMatchesFromHats("D","G");
(Match m3, Match m4) = CreateMatchesFromHats("E","F");

Console.WriteLine("Cetvrt Finale:");
Console.WriteLine($"\t{m1.Simulate()}");
Console.WriteLine($"\t{m4.Simulate()}");
Console.WriteLine("");
Console.WriteLine($"\t{m2.Simulate()}");
Console.WriteLine($"\t{m3.Simulate()}");

Console.WriteLine("Polu Finale:");
var semi1 = new Match(m1.Winner!, m4.Winner!);
var semi2 = new Match(m2.Winner!, m3.Winner!);
Console.WriteLine($"\t{semi1.Simulate()}");
Console.WriteLine($"\t{semi2.Simulate()}");

var thirdPlace = new Match(semi1.Loser!, semi2.Loser!);
Console.WriteLine("Utakmica za 3. mesto:");
Console.WriteLine($"\t{thirdPlace.Simulate()}");

var final = new Match(semi1.Winner!, semi2.Winner!);
Console.WriteLine("Finale:");
Console.WriteLine($"\t{final.Simulate()}");

Console.WriteLine("Medalje:");
Console.WriteLine($"\t1.{final.Winner!.Name}");
Console.WriteLine($"\t2.{final.Loser!.Name}");
Console.WriteLine($"\t3.{thirdPlace.Winner!.Name}");




(Match m1, Match m2) CreateMatchesFromHats(string h1,string h2)
{
   var a1 = Hats[h1][0];
   int aIndex = Random.Shared.Next(2);
   var a2 = Hats[h2][aIndex];
   var b1 = Hats[h1][1];
   var b2 = Hats[h2][otherIndexPair(aIndex)];
   if (a1.PlayedAgainst(a2))
   {
      var temp = a2;
      a2 = b2;
      b2 = temp;
   }
   if (b1.PlayedAgainst(b2))
   {
      var temp = b2;
      b2 = a2;
      a2 = temp;
   }
   return (new Match(a1, a2), new Match(b1, b2));

}

int otherIndexPair(int i)
{
   if (i == 1) return 0;
   return 1;
}


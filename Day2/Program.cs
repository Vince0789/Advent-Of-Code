internal class Program
{
    public enum Shape
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3,
    }

    public enum Outcome
    {
        Lose = 0,
        Draw = 3,
        Win = 6,
    }

    private static void Main(string[] args)
    {
        List<string> lines = File.ReadLines("inputs.txt").ToList();

        #region Part 1
        int score = 0;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] actions = line.Split(' ');
            Shape? opponentShape = MapInputToShape(actions[0]);
            Shape? yourShape = MapInputToShape(actions[1]);

            if (opponentShape == null || yourShape == null)
                continue;

            // The score for a single round is the score for the shape you
            // selected (1 for Rock, 2 for Paper, and 3 for Scissors) plus
            // the score for the outcome of the round (0 if you lost, 3 if
            // the round was a draw, and 6 if you won).
            score += CalculateRoundScore(yourShape.Value, opponentShape.Value);
        }

        Console.WriteLine("Part 1 - Total Score: {0}", score);
        #endregion

        #region Part 2
        score = 0;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] actions = line.Split(' ');
            Shape? opponentShape = MapInputToShape(actions[0]);
            Outcome? yourAction = MapInputToOutcome(actions[1]);

            if (opponentShape == null || yourAction == null)
                continue;

            Shape? yourShape = opponentShape; // draw

            if (yourAction != Outcome.Draw)
            {
                switch (opponentShape)
                {
                    case Shape.Rock:
                        yourShape = (yourAction == Outcome.Win) ? Shape.Paper : Shape.Scissors;
                        break;
                    case Shape.Paper:
                        yourShape = (yourAction == Outcome.Win) ? Shape.Scissors : Shape.Rock;
                        break;
                    case Shape.Scissors:
                        yourShape = (yourAction == Outcome.Win) ? Shape.Rock : Shape.Paper;
                        break;
                }
            }

            score += CalculateRoundScore(yourShape.Value, opponentShape.Value);
        }

        Console.WriteLine("Part 2 - Total Score: {0}", score);
        #endregion

        Console.ReadLine();
    }

    private static int CalculateRoundScore(Shape yourShape, Shape opponentShape)
    {
        int score = (int)yourShape;

        if (opponentShape == yourShape)
        {
            score += (int)Outcome.Draw;
        }
        else
        {
            switch (opponentShape)
            {
                case Shape.Rock:
                    score += (yourShape == Shape.Paper) ? (int)Outcome.Win : (int)Outcome.Lose;
                    break;
                case Shape.Paper:
                    score += (yourShape == Shape.Scissors) ? (int)Outcome.Win : (int)Outcome.Lose;
                    break;
                case Shape.Scissors:
                    score += (yourShape == Shape.Rock) ? (int)Outcome.Win : (int)Outcome.Lose;
                    break;
            }
        }

        return score;
    }

    private static Shape? MapInputToShape(string input)
    {
        return input switch
        {
            "A" or "X" => Shape.Rock,
            "B" or "Y" => Shape.Paper,
            "C" or "Z" => Shape.Scissors,
            _ => null
        };
    }

    private static Outcome? MapInputToOutcome(string input)
    {
        return input switch
        {
            "X" => Outcome.Lose,
            "Y" => Outcome.Draw,
            "Z" => Outcome.Win,
            _ => null
        };
    }
}
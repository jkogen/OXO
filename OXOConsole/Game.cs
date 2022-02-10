namespace OXO;

public class Game
{
    public bool IsFinished { get; set; }
    public bool IsStarted { get; set; }

    public Player Player1 { get; set; } = new Player();
    public Player Player2 { get; set; } = new Player();
    public Gamefield Gamefield { get; set; } = new Gamefield();

    public Game()
    {
        Player1.FigureType = Player.Figure.X;
        Player2.FigureType = Player.Figure.O;
    }

    /// <summary>
    /// Startet das Spiel.
    /// </summary>
    /// <param name="playerOneMoves">True, wenn der este Spieler beginnen soll</param>
    public void Start(bool playerOneMoves)
    {
        Gamefield = new Gamefield();

        IsStarted = true;
        IsFinished = false;

        bool PlayerOneMoves = playerOneMoves;

        while (!IsFinished)
        {
            this.Draw();

            if (PlayerOneMoves)
                Player1.Move(Gamefield);
            else
                Player2.Move(Gamefield);

            var possibleWinner = Gamefield.IsWon(Player1, Player2);
            if (possibleWinner != null)
            {
                IsFinished = true;
                possibleWinner.Wins++;
                this.Draw();
                possibleWinner.DrawPlayerText("Du hast gewonnen - glückwunsch!", false);
            }
            else if (AllFiguresMoved)
            {
                IsFinished = true;
                this.Draw();
                Player1.DrawPlayerText("Spiel vorbei - unentschieden", false);
            }

            //Nächster Zug gehört O und nicht X
            PlayerOneMoves = !PlayerOneMoves;
        }
    }

    public bool AllFiguresMoved => Gamefield.Field.Count(a => a != null) == Gamefield.Field.Length;
}

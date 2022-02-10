using System;
using System.Linq;
using System.Threading;

namespace OXO;
public class Player
{
    public ConsoleColor PlayerColor => FigureType == Figure.X ? ConsoleColor.Green : ConsoleColor.Red;

    public enum Figure { X, O };
    public Figure FigureType = Figure.X;
    public char FigureTypeCharaacter => FigureType.ToString()[0];
    public string Name { get; set; } = "";
    private bool isBot = false;
    public bool IsBot
    {
        get => isBot;
        set
        {
            if (value == true)
                Name = "Bot";

            isBot = value;
        }
    }
    public int Wins { get; set; } = 0;

    /// <summary>
    /// Lässt den aktiven Spieler ein Spielfeld besetzen. 
    /// Der Bot setzt automatisch
    /// </summary>
    /// <param name="gamefield"></param>
    public void Move(Gamefield gamefield)
    {
        int index = -1;
        int[] allowedIndexes = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        string startMessage = "Besetze ein Feld.";
        bool validInput = false;

        while (!validInput)
        {
            validInput = true;
            index = -1;

            if (!IsBot)
            {
                var input = this.DrawPlayerText(startMessage, true);

                if (input.Length == 1 && int.TryParse(input[0].ToString(), out _))
                {
                    //Gültig eingegebenes Spielfeld wird -1 genommen,
                    //da das interne Spielfeld-Array bei Index 0 beginnt
                    index = input[0] - '1';
                }
            }
            //Bot setzt
            else
            {
                //Bot nachdenken lassen
                for (int i = 0; i < 5; i++)
                {
                    var textPoints = string.Concat(Enumerable.Repeat(".", i));
                    this.DrawPlayerText(startMessage + textPoints, false);
                    Thread.Sleep(600);
                }

                //so lange setzen, bis es passt
                do
                {
                    Random x = new Random();
                    index = x.Next(0, 9);
                } while (gamefield.Field[index] != null);
            }

            if (!allowedIndexes.Contains(index))
            {
                startMessage = "Bitte verwende ein gültiges Feld (1-9).";
                validInput = false;
            }
            else if (gamefield.Field[index] != null)
            {
                startMessage = "Bitte verwende ein freies Feld.";
                validInput = false;
            }
            else
            {
                startMessage = "Besetze ein Feld.";
            }
        }

        gamefield.Field[index] = FigureTypeCharaacter;
    }

    /// <summary>
    /// Setzt den Spielernamen über die Eingabe 
    /// </summary>
    public void GetName()
    {
        var name = this.DrawPlayerText("Wie ist dein Name? ", true, false);
        Name = name;
    }
}

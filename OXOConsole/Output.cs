using System;
using System.Linq;
using System.Threading;

namespace OXO;

public static class Output
{
    /// <summary>
    /// Zeigt eine Willkommensnachricht, die ein paar Sekunden angezeigt wird.
    /// </summary>
    /// <param name="waitingSeconds"></param>
    public static void ShowStartMessage(int waitingSeconds)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.SetCursorPosition(88, 10);
        Console.Write("\n\n\t\tWillkommen bei");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" OXO");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("!\n\n\t\tDas Spiel startet in ..");

        for (int i = waitingSeconds; i > 0; i--)
        {
            Thread.Sleep(1000);
            Console.WriteLine("\t\t\t" + i.ToString() + "...");
        }

        Thread.Sleep(1000);
    }

    /// <summary>
    /// Zeigt das Spielfeld auf der Konsole in der einfachen Variante.
    /// </summary>
    /// <param name="game"></param>
    public static void ShowGamefield(this Game game)
    {
        int index = 1;
        Console.Clear();

        foreach (var cell in game.Gamefield.Field)
        {

            //Zelle ausgaben, wenn belegt. Ansonsten Leerzeichen
            Console.Write($"[ {(cell == '\0' ? " " : cell)} ]({index})   ");

            if (index++ % 3 == 0)
                Console.WriteLine("\n");
        }
    }

    /// <summary>
    /// Sorgt für die Spielausgabe
    /// </summary>
    /// <param name="game"></param>
    public static void Draw(this Game game)
    {
        Console.Clear();
        DrawGamefield();
        game.DrawFieldNumbers();
        DrawTextBorder();
        game.DrawGamestate();
        game.DrawFigures();
    }

    /// <summary>
    /// Zeigt das Spielfeld
    /// </summary>
    private static void DrawGamefield()
    {
        //37 Zeien durchlaufen für das Spielfeld
        for (int i = 0; i < 38; i++)
        {
            if (i != 12 && i != 25)
            {
                //zwei aufrechte Striche setzen
                Console.SetCursorPosition(16, i);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("██");

                Console.SetCursorPosition(34, i);
                Console.Write("██");
            }
            //bei jeder 12. Zeile einen senkrechten Strich setzen
            else
            {
                Console.SetCursorPosition(0, i);
                Console.Write(string.Concat(Enumerable.Repeat("█", 52)));
            }
        }
    }

    /// <summary>
    /// Zeigt die Spielfeld-Nummern
    /// </summary>
    /// <param name="game"></param>
    private static void DrawFieldNumbers(this Game game)
    {
        int startNumberPerRow = 1;

        for (int i = 11; i < 38; i += 13)
        {
            if (i == 24)
                startNumberPerRow = 4;
            else if (i == 37)
                startNumberPerRow = 7;

            //erste Spalte
            if (game.Gamefield.Field[startNumberPerRow - 1] == null)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(13, i);
                Console.Write($"[{startNumberPerRow}]");
            }

            //zweite Spalte
            if (game.Gamefield.Field[startNumberPerRow] == null)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(31, i);
                Console.Write($"[{startNumberPerRow + 1}]");
            }

            //dritte Spalte
            if (game.Gamefield.Field[startNumberPerRow + 1] == null)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(49, i);
                Console.Write($"[{startNumberPerRow + 2}]");
            }

            startNumberPerRow += 3;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }

    /// <summary>
    /// Zeigt in der rechten Bildschirmhälfte einen Rahmen für die Text-Anzeige
    /// </summary>
    private static void DrawTextBorder()
    {
        //Senkrechte Striche setzen
        Console.ForegroundColor = ConsoleColor.DarkGray;
        for (int i = 1; i < 38; i++)
        {
            Console.SetCursorPosition(60, i);
            Console.Write("██");
            Console.SetCursorPosition(111, i);
            Console.Write("██");
        }

        //3 waagerechte Striche setzen
        int yPosition = 1;

        for (int i = 0; i < 3; i++)
        {
            if (i == 1)
                yPosition = 7;
            else if (i == 2)
                yPosition = 37;

            Console.SetCursorPosition(60, yPosition);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"◄{string.Concat(Enumerable.Repeat("█", 51))}►");
        }
    }

    /// <summary>
    /// Zeigt den aktuelen Spielstand an der rechten oberen Bildschirm-Hälfte
    /// </summary>
    /// <param name="game"></param>
    private static void DrawGamestate(this Game game)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(83, 3);
        Console.Write("Spielstand");
        Console.SetCursorPosition(83, 5);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(" " + game.Player1.Wins);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("  -");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("  " + game.Player2.Wins);
    }

    /// <summary>
    /// Gibt einen definierten Text aus.
    /// </summary>
    /// <param name="player">Der Spieler, der angesprochen wird.</param>
    /// <param name="text">Der Ausgabe-Text.</param>
    /// <param name="playerInput">True, wenn der Spieler eine Eingabe machen soll</param>
    /// <param name="keyInput">True, wenn nur ein Buchstabe eingegeben werden soll</param>
    /// <returns></returns>
    public static string DrawPlayerText(this Player player, string text, bool playerInput, bool keyInput = true)
    {
        ResetText();

        var textBySpaces = text.Split(' ');
        Console.SetCursorPosition(65, 10);

        Console.ForegroundColor = player.PlayerColor;
        Console.Write($"@{player.Name}: ");
        Console.ForegroundColor = ConsoleColor.White;
        int row = 10;
        string currentLine = "";

        //Für jedes Wort
        for (int i = 0; i < textBySpaces.Count(); i++)
        {
            //Wenn das Wort noch in die aktuelle Zeile passt
            if (currentLine.Length + textBySpaces[i].Length + 1 < 40)
            {
                //Zwischenspeichern
                currentLine += textBySpaces[i] + " ";
            }
            //Ansonsten wenn das Wort nicht mehr in die Zeile passt
            else
            {
                //Satzteil ausgeben
                Console.SetCursorPosition(65 + player.Name.Length + 3, row);
                Console.Write(currentLine);
                //Neue Zeile erzeugen und Satzteil mit dem Wort neu beginnen
                row++;
                currentLine = textBySpaces[i] + " ";
            }
        }

        //Wenn der Satz noch nicht beendet wurde
        if (!string.IsNullOrWhiteSpace(currentLine))
        {
            //letzten Satzteil ausgeben
            Console.SetCursorPosition(65 + player.Name.Length + 3, row);
            Console.Write(currentLine);
        }

        Console.ForegroundColor = ConsoleColor.Gray;

        if (playerInput)
        {
            row += 2;

            //Unterstrich setzen
            Console.SetCursorPosition(65, row);
            Console.Write("________________________");

            //Eingabe
            Console.SetCursorPosition(65, row);
            Console.ForegroundColor = player.PlayerColor;
            var input = keyInput ? Console.ReadKey().KeyChar.ToString() : Console.ReadLine() ?? "";

            //Darstellung zurück setzen
            return input;
        }

        //Darstellung zurück setzen
        return "";
    }

    /// <summary>
    /// Setzt die Text-Darstellung für neue Texte zurück
    /// </summary>
    private static void ResetText()
    {
        for (int i = 10; i < 37; i++)
        {
            //Den Text visuell zurück setzen
            Console.SetCursorPosition(65, i);
            Console.WriteLine(string.Concat(Enumerable.Repeat(" ", 46)));
        }
    }

    /// <summary>
    /// Zeigt die Figuren in wunderbarer Darstellung
    /// </summary>
    /// <param name="game"></param>
    private static void DrawFigures(this Game game)
    {
        int row = 0, column = 0;

        for (int i = 0; i < game.Gamefield.Field.Length; i++)
        {
            var figure = game.Gamefield.Field[i];
            switch (i + 1)
            {
                case 1:
                    row = 2; column = 1;
                    break;
                case 2:
                    row = 2; column = 19;
                    break;
                case 3:
                    row = 2; column = 37;
                    break;
                case 4:
                    column = 1; row = 15;
                    break;
                case 5:
                    column = 19; row = 15;
                    break;
                case 6:
                    column = 37; row = 15;
                    break;
                case 7:
                    column = 1; row = 28;
                    break;
                case 8:
                    column = 19; row = 28;
                    break;
                case 9:
                    column = 37; row = 28;
                    break;
            }

            if (figure == null) continue;

            if (figure == 'X')
                DrawXCharacter(column, row);
            else
                DrawOCharacter(column, row);
        }
    }
    /// <summary>
    /// Gibt die X-Figur aus
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    private static void DrawXCharacter(int column, int row)
    {
        Console.SetCursorPosition(column, row);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("▀████    ▐████▀");
        Console.SetCursorPosition(column, row + 1);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("  ███▌   ████▀ ");
        Console.SetCursorPosition(column, row + 2);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("   ███  ▐███   ");
        Console.SetCursorPosition(column, row + 3);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("   ▀███▄███▀  ");
        Console.SetCursorPosition(column, row + 4);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("   ████▀██▄    ");
        Console.SetCursorPosition(column, row + 5);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("  ▐███  ▀███  ");
        Console.SetCursorPosition(column, row + 6);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(" ▄███     ███▄ ");
        Console.SetCursorPosition(column, row + 7);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("████       ███▄");
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    /// <summary>
    /// Gibt die O-Figur aus
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    private static void DrawOCharacter(int column, int row)
    {
        Console.SetCursorPosition(column, row);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" ▄██████████▄");
        Console.SetCursorPosition(column, row + 1);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("███        ███ ");
        Console.SetCursorPosition(column, row + 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("███        ███ ");
        Console.SetCursorPosition(column, row + 3);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("███        ███ ");
        Console.SetCursorPosition(column, row + 4);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("███        ███ ");
        Console.SetCursorPosition(column, row + 5);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("███        ███ ");
        Console.SetCursorPosition(column, row + 6);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("███        ███ ");
        Console.SetCursorPosition(column, row + 7);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("▀██████████▀  ");
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}

namespace OXO
{
    public class Gamefield
    {
        public char?[] Field { get; set; } = new char?[9];

        /// <summary>
        /// Gibt den Spieler zurück, der das Spiel gewonnen hat.
        /// Gibt NULL zurück, wenn niemand gewonnen hat.
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <returns></returns>
        public Player IsWon(Player player1, Player player2)
        {
            //Waagerechten Zeilen auswerten
            for (int i = 0; i < 9; i += 3)
            {
                if (Field[i] != null && Field[i] == Field[i + 1] && Field[i] == Field[i + 2])
                {
                    if (Field[i] == player1.FigureTypeCharaacter)
                        return player1;

                    return player2;
                }
            }

            //Senkrechte Spalten auswerten
            for (int i = 0; i < 3; i++)
            {
                if (Field[i] != null && Field[i] == Field[i + 3] && Field[i] == Field[i + 6])
                {
                    if (Field[i] == player1.FigureTypeCharaacter)
                        return player1;

                    return player2;
                }
            }

            //Diagonale 1 auswerten
            if (Field[0] != null && Field[0] == Field[4] && Field[4] == Field[8])
            {
                if (Field[0] == player1.FigureTypeCharaacter)
                    return player1;

                return player2;
            }

            //Diagonale 2 auswerten
            if (Field[2] != null && Field[2] == Field[4] && Field[4] == Field[6])
            {
                if (Field[2] == player1.FigureTypeCharaacter)
                    return player1;

                return player2;
            }

            return null;
        }
    }
}

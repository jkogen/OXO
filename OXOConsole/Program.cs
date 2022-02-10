Console.Clear();
Console.Title = "OXO-Spielgenerator";
Console.SetWindowSize(115, 40);

var game = new Game();
Output.ShowStartMessage(4);

game.Draw();
game.Player1.GetName();
var botModus = game.Player1.DrawPlayerText("Möchtest du den Bot-Modus verwenden? [Y/N]", true);

if (botModus.ToLower() == "y")
    game.Player2.IsBot = true;
else
    game.Player2.GetName();

bool playerOneMoves = true;

while (true)
{
    game.Start(playerOneMoves);
    playerOneMoves = !playerOneMoves;
    Console.ReadKey();
}



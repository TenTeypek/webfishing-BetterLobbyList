using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;
namespace TenTeypek.BetterLobbyList;

public class SteamNetworkScript : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Singletons/SteamNetwork.gdc";
    
    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var extendsWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrExtends,
            t => t.Type is TokenType.Newline
        ], allowPartialMatch: true);
        
        var lobbyDistanceWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrFunction,
            t => t is IdentifierToken { Name: "_find_all_webfishing_lobbies"},
            // ...
            t => t is IdentifierToken { Name:"addRequestLobbyListDistanceFilter" },
            t => t.Type is TokenType.ParenthesisOpen
        ], allowPartialMatch: true);
        
        var consumer = new TokenConsumer(t => t.Type is TokenType.ParenthesisClose);

        // loop through all tokens in the script
        foreach (var token in tokens)
        {
            if (consumer.Check(token))
            {
                continue;
            }

            if (consumer.Ready)
            {
                consumer.Reset();
            }
            
            if (extendsWaiter.Check(token))
            {
                yield return token;
                
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("distance");
                yield return new Token(TokenType.Newline);
            }
            else if (lobbyDistanceWaiter.Check(token))
            {
                yield return token;
                yield return new IdentifierToken("distance");
                
                consumer.SetReady();
            }
            else
            {
                // return the original token
                yield return token;
            }

        }
    }
}
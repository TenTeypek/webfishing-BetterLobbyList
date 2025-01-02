using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace TenTeypek.BetterLobbyList;

public class MainMenuScript : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Menus/Main Menu/main_menu.gdc";
    
    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var lobbySortWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrExtends,
            t => t.Type is TokenType.Newline,
            t => t.Type is TokenType.PrEnum,
            t => t is IdentifierToken {Name:"LOBBY_SORT"},
            t => t.Type is TokenType.CurlyBracketOpen,
            t => t is IdentifierToken {Name:"PLAYER_HIGH"},
            t => t.Type is TokenType.Comma,
            t => t is IdentifierToken {Name:"PLAYER_LOW"},
            t => t.Type is TokenType.Comma,
            t => t is IdentifierToken {Name:"RANDOM"}
        ], allowPartialMatch: true);

        var ifLobbySearchSortWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken {Name:"lobby_search_sort"},
            t => t.Type is TokenType.OpGreater,
            t => t is ConstantToken {Value: IntVariant { Value: 2 } }
        ], allowPartialMatch: false);
        
        var matchLobbySearchSortWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken {Name:"LOBBY_SORT"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name:"RANDOM"},
            t => t.Type is TokenType.Colon,
            t => t is IdentifierToken {Name:"search_button"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name:"text"},
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken {Value: StringVariant { Value: "Random" } }
        ], allowPartialMatch: false);

        // loop through all tokens in the script
        foreach (var token in tokens)
        {
            if (lobbySortWaiter.Check(token))
            {
                yield return token;
                
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("PING");
            }
            else if (ifLobbySearchSortWaiter.Check(token))
            {
                yield return new ConstantToken(new IntVariant(3));
            }
            else if (matchLobbySearchSortWaiter.Check(token))
            {
                yield return token;
                
                yield return new Token(TokenType.Newline, 2 );
                yield return new IdentifierToken("LOBBY_SORT");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("PING");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("search_button");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("text");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("Closest"));
                yield return new Token(TokenType.Newline);
            }
            else
            {
                // return the original token
                yield return token;
            }

        }
    }
}
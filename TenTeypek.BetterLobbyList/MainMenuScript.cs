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

        var validatedListWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken { Name: "validated_list" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name: "append" },
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.BracketOpen,
            t => t is IdentifierToken { Name: "lobby" },
            t => t.Type is TokenType.Comma,
            t => t is IdentifierToken { Name: "lobby_num_members" }
        ], allowPartialMatch: false);

        var matchLobbySorterWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken { Name: "LOBBY_SORT" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name: "RANDOM" },
            t => t.Type is TokenType.Colon,
            t => t is IdentifierToken { Name: "sorted_list" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name: "sort_custom" },
            t => t.Type is TokenType.ParenthesisOpen,
            // ...
            t => t.Type is TokenType.ParenthesisClose
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
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("NAME");
            }
            else if (validatedListWaiter.Check(token))
            {
                yield return token;
                
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("Steam");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("getLobbyData");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("lobby");
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("lobby_name"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("Steam");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("getLobbyData");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("lobby");
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("lobby_name"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpNotEqual);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.CfElse);
                yield return new IdentifierToken("Steam");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("getLobbyData");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("lobby");
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("name"));
                yield return new Token(TokenType.ParenthesisClose);
            }
            else if (matchLobbySorterWaiter.Check(token))
            {
                yield return token;
                
                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("LOBBY_SORT");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("NAME");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("sorted_list");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("sort_custom");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.Self);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("_lobby_sort_name"));
                yield return new Token(TokenType.ParenthesisClose);
            }
            else if (ifLobbySearchSortWaiter.Check(token))
            {
                yield return new ConstantToken(new IntVariant(4));
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
                yield return new Token(TokenType.Newline, 2);
                
                yield return new IdentifierToken("LOBBY_SORT");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("NAME");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("search_button");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("text");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("A-Z"));
                yield return new Token(TokenType.Newline);
                
                yield return new Token(TokenType.PrFunction);
                yield return new IdentifierToken("_lobby_sort_name");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("a");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("b");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.CfReturn);
                yield return new IdentifierToken("a");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(2));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("nocasecmp_to");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("b");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(2));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpLess);
                yield return new ConstantToken(new IntVariant(0));
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
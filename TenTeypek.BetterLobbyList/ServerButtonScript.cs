using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace TenTeypek.BetterLobbyList;

public class ServerButtonScript : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Menus/Main Menu/ServerButton/server_button.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var updateLobbyNameWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrFunction,
            t => t is IdentifierToken { Name:"_update_lobby_name" },
            // ...
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken { Name: "label_toggle" },
            t => t.Type is TokenType.OpAnd,
            t => t is IdentifierToken { Name: "custom_name" },
            t => t.Type is TokenType.OpNotEqual,
            t => t is ConstantToken,
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline
        ], allowPartialMatch: true);

        // loop through all tokens in the script
        foreach (var token in tokens)
        {
            if (updateLobbyNameWaiter.Check(token))
            {
                yield return token;

                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("OptionsMenu");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("chat_filter");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("new_name");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("custom_name");
                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);
                

                
            }
            else
            {
                // return the original token
                yield return token;
            }

        }
    }
}
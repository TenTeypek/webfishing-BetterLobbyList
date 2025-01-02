using System.Text.Json.Serialization;

namespace TenTeypek.BetterLobbyList;

public class Config {
    [JsonInclude] public bool SomeSetting = true;
}

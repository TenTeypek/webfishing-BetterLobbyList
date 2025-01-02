using GDWeave;

namespace TenTeypek.BetterLobbyList;

public class Mod : IMod {
    public Mod(IModInterface modInterface) {
        modInterface.Logger.Information("Hello, world!");
        modInterface.RegisterScriptMod(new MainMenuScript());
        modInterface.RegisterScriptMod(new ServerButtonScript());
        modInterface.RegisterScriptMod(new SteamNetworkScript());
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}

using GDWeave;

namespace GDWeave.BottleOpener;
public class Mod : IMod {

    public Config Config;
    public Mod(IModInterface modInterface) {

        this.Config = modInterface.ReadConfig<Config>();
        modInterface.RegisterScriptMod(new DrunkPatch());
    }

    public void Dispose() {/*cleanup*/}
}

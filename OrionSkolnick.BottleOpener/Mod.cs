/*
 * This file is your "main", it reads a config and then adds the modified scripts to the game
 *
 */

using GDWeave;

namespace GDWeave.BottleOpener;
public class Mod : IMod {

    public Config Config;
    public Mod(IModInterface modInterface) {

        this.Config = modInterface.ReadConfig<Config>(); //load config

        //register scripts
        modInterface.RegisterScriptMod(new DrunkPatch()); //script that removes drink time limit
    }

    public void Dispose() {/*cleanup*/}
}

/*
 * This file is what is editing the drunk timer code in the player file ("player.gd")
 * It removes the lines 2022 and 2025, which fix the drunk time limit below an arbitrary amount
 * It does this by tokenizing the script and then skipping over the line where the code needed to go
 */

using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace GDWeave.BottleOpener;

public class DrunkPatch() : IScriptMod {

    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) { //the goal of this is to remove the "target" line in stage 2

        var waiter = new MultiTokenWaiter([ //stage 1, finds line preceding target
			      // tokenized "drunk_timer += NUMBER;"
            //
            t => t is IdentifierToken{Name: "drunk_timer"},
            t => t.Type is TokenType.OpAssignAdd,
            t => t is ConstantToken {Value: IntVariant},
            t => t.Type is TokenType.Newline
        ]);

        var eater = new TokenWaiter( //stage 2, targets newline at end of target line
            t => t.Type is TokenType.Newline
        , waitForReady: true);

        foreach (var token in tokens) {
            if (eater.Ready && waiter.Matched) { //when skipping over the line
                if (eater.Check(token)) { //case for when its the end of the line (token is TokenType.Newline here)
                    waiter.Reset();
                    eater.Reset();
                    yield return token;
                }
                continue;
            } if (waiter.Check(token)) { //when stage 1 completes, activates stage 2 loop by setting eater to ready
                eater.SetReady();
            } else { //passes normal lines
                yield return token;
            }
        }
    }
}

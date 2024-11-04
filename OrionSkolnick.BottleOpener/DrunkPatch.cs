/*
 * 
 */

using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace GDWeave.BottleOpener;

public class DrunkPatch() : IScriptMod {

    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var waiter = new MultiTokenWaiter ([ //its getting the code line "drunk_timer = clamp(drunk_timer, 0, 50000" from lines 2022 and 2025 in the source code
            t => t is IdentifierToken {Name: "drunk_timer"},
            t => t.Type is TokenType.OpAssign,
            t => t.AssociatedData is 53, //built in function ID for clamp, this is just the clamp function token
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken {Name: "drunk_timer"},
            t => t.Type is TokenType.Comma,
            t => t is ConstantToken {Value: IntVariant{ Value: 0}},
            t => t.Type is TokenType.Comma,
            t => t is ConstantToken {Value: IntVariant{ Value: 50000}} //stop here so you can replace at the check
        ]);

        foreach (var token in tokens) {
            if (waiter.Check(token)) {
                yield return new ConstantToken(new IntVariant(long.MaxValue)); //replacing the last token (the constant 50000) with our own value
                waiter.Reset(); //reset so it can do it with the next match
                continue; //continue so it won't try and add the constant token at the "else" block
            } else {
                yield return token;
            }
        }
    }
}

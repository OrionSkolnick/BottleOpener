using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace GDWeave.BottleOpener;

public class DrunkPatch() : IScriptMod {

    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var waiter = new MultiTokenWaiter ([
            t => t is IdentifierToken {Name: "drunk_timer"},
            t => t.Type is TokenType.OpAssign,
            t => t.AssociatedData is 53,
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken {Name: "drunk_timer"},
            t => t.Type is TokenType.Comma,
            t => t is ConstantToken {Value: IntVariant{ Value: 0}},
            t => t.Type is TokenType.Comma,
            t => t is ConstantToken {Value: IntVariant{ Value: 50000}}
        ]);

        foreach (var token in tokens) {
            if (waiter.Check(token)) {
                yield return new ConstantToken(new IntVariant(long.MaxValue));
                waiter.Reset();
                continue;
            } else {
                yield return token;
            }
        }
    }
}

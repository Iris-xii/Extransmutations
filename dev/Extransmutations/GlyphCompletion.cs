
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;

public static class GlyphCompletion {
  public static PartType LoadPuzzleContent(Textures t) {
    QApi.AddPuzzlePermission("extransmutations-cardinal-completion",
    "Glyph of Cardinal Completion",
    "Extransmutations");
    PartType cardinalCompletion = new() {
      field_1528 = "extransmutations-cardinal-completion", // ID
      field_1529 = class_134.method_253("Glyph of Cardinal Completion", string.Empty), // Name
      field_1530 = class_134.method_253("The Glyph of Cardinal Completion transmutes 3 different cardinals and 1 salt atom into 4 atoms of the missing cardinal.", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = t.cardinalCompletionGlow, // Shadow/glow
      field_1550 = t.cardinalCompletionStroke, // Stroke/outline
      field_1547 = t.completionPanel, // Panel icon
      field_1548 = t.completionPanel, // Hovered panel icon
      field_1540 = new HexIndex[]{
                new(0, 0),
                new(1, 0),
                new(0, 1),
                new(1, -1),
            }, // Spaces used
      field_1551 = Permissions.None,
      CustomPermissionCheck = perms => perms.Contains("extransmutations-cardinal-completion"),
    };
    QApi.AddPartType(cardinalCompletion, (part, pos, editor, renderer) => {
      Vector2 centre = new(/*82.5f*/41.25f, 119.5f);// new Vector2(42f, 49f) + new Vector2(0f,-1f);
      renderer.method_523(t.cardinalCompletionBase, new Vector2(-1, -1), centre, 0);
      renderer.method_528(t.bowlProjTexture, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_528(t.bowlProjTexture, new HexIndex(0, 0), Vector2.Zero);
      renderer.method_528(t.bowlProjTexture, new HexIndex(1, -1), Vector2.Zero);
      renderer.method_528(t.bowlProjTexture, new HexIndex(0, 1), Vector2.Zero);

      renderer.method_529(t.cardinalCompletionGlyphAny, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_529(t.cardinalCompletionGlyphAny, new HexIndex(0, 0), Vector2.Zero);
      renderer.method_529(t.cardinalCompletionGlyphAny, new HexIndex(1, -1), Vector2.Zero);
      renderer.method_529(t.saltBowlGlyphSpot, new HexIndex(0, 1), Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(cardinalCompletion, false);
    return cardinalCompletion;
  }
  public static void Activate(Sim sim, SolutionEditorBase seb, Part part, Textures t) {
    if (sim.FindAtomRelative(part, new HexIndex(1, 0)).method_99(out AtomReference c1) &&
            sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference c2) &&
            sim.FindAtomRelative(part, new HexIndex(1, -1)).method_99(out AtomReference c3) &&
            sim.FindAtomRelative(part, new HexIndex(0, 1)).method_99(out AtomReference salt)) {
      AtomReference[] cardinalRefs = { c1, c2, c3 };
      AtomReference[] allAtomRefs = { c1, c2, c3, salt };
      bool hasSalt = salt.field_2280 == Brimstone.API.VanillaAtoms.salt;
      AtomType[] cardinals_types = {
                        Brimstone.API.VanillaAtoms.water,
                        Brimstone.API.VanillaAtoms.fire,
                        Brimstone.API.VanillaAtoms.air,
                        Brimstone.API.VanillaAtoms.earth,
                    };
      byte[] cardinalsCount = { 0, 0, 0, 0 };
      for (int i = 0; i < 4; i++) {
        foreach (AtomReference ar in cardinalRefs) {
          if (ar.field_2280 == cardinals_types[i]) {
            cardinalsCount[i] += 1;
          }
        }
      }
      AtomType maybeTarget = Brimstone.API.VanillaAtoms.salt; //this crashes if null, for some reason, so dummy salt it is
      if (cardinalsCount.SequenceEqual(new byte[] { 1, 1, 1, 0 })) {
        maybeTarget = Brimstone.API.VanillaAtoms.earth;
      }
      if (cardinalsCount.SequenceEqual(new byte[] { 0, 1, 1, 1 })) {
        maybeTarget = Brimstone.API.VanillaAtoms.water;
      }
      if (cardinalsCount.SequenceEqual(new byte[] { 1, 0, 1, 1 })) {
        maybeTarget = Brimstone.API.VanillaAtoms.fire;
      }
      if (cardinalsCount.SequenceEqual(new byte[] { 1, 1, 0, 1 })) {
        maybeTarget = Brimstone.API.VanillaAtoms.air;
      }
      if (hasSalt && (maybeTarget != Brimstone.API.VanillaAtoms.salt)) { //transmute!
        foreach (AtomReference ar in allAtomRefs) {
          ar.field_2277.method_1106(maybeTarget, ar.field_2278);
          ar.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, ar.field_2280, class_238.field_1989.field_81.field_614, 60f);
        }
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), t.calcifyAnimation, 30f, Vector2.Zero, 0f));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 0))), t.calcifyAnimation, 30f, Vector2.Zero, 0f));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, -1))), t.calcifyAnimation, 30f, Vector2.Zero, 0f));
        class_238.field_1991.field_1844.method_28(seb.method_506());
      }
    }
  }
}
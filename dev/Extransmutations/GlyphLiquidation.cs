using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VanillaAtoms = Brimstone.API.VanillaAtoms;

public static class GlyphLiquidation {

  public static PartType LoadPuzzleContent(Textures t) {
    QApi.AddPuzzlePermission("extransmutations-liquidation",
    "Glyph of Liquidation",
    "Extransmutations");
    PartType glyphLiquidation = new() {
      field_1528 = "extransmutations-liquidation", // ID
      field_1529 = class_134.method_253("Glyph of Liquidation", string.Empty), // Name
      field_1530 = class_134.method_253("The Glyph of Liquidation transmutes an atom of quicksilver into lead in exchange for transmuting one OR two salt atoms into water.", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = t.liquidationGlow, // Shadow/glow
      field_1550 = t.liquidationStroke, // Stroke/outline
      field_1547 = t.liquidationBase, // Panel icon
      field_1548 = t.liquidationBase, // Hovered panel icon
      field_1540 = new HexIndex[]{
                new(0, 0),
                new(-1, 1),
                new(-1, 2),
            }, // Spaces used
      field_1551 = Permissions.None,
      CustomPermissionCheck = perms => perms.Contains("extransmutations-liquidation"),
    };

    QApi.AddPartType(glyphLiquidation, (part, pos, editor, renderer) => {
      Vector2 centre = t.liquidationBase.method_691();
      var time = editor.method_504();
      class_236 uco = editor.method_1989(part, pos);
      PartSimState pss = editor.method_507().method_481(part);
      renderer.method_523(t.liquidationBase, new Vector2(-1, -1), centre, 0f);
      renderer.method_528(t.bowlProjTexture, new HexIndex(0, 0), Vector2.Zero);
      renderer.method_528(t.bowlTexture, new(-1, 1), Vector2.Zero);
      renderer.method_528(t.bowlTexture, new(-1, 2), Vector2.Zero);
      renderer.method_529(t.saltBowlGlyphSpot, new(-1, 1), Vector2.Zero);
      renderer.method_529(t.saltBowlGlyphSpot, new(-1, 2), Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(glyphLiquidation, false);
    return glyphLiquidation;
  }
  public static void Activate(Sim sim, SolutionEditorBase seb, Part part, Textures t) { }
}
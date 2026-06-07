using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VanillaAtoms = Brimstone.API.VanillaAtoms;


public static class GlyphExtraordinary {


  public static PartType LoadPuzzleContent(Resources t) {
    QApi.AddPuzzlePermission("extransmutations-extraordinary",
    "Glyph of the Extraordinary",
    "Extransmutations");
    PartType glyphExtraordinary = new() {
      field_1528 = "extransmutations-extraordinary", // ID
      field_1529 = class_134.method_253("Glyph of the Extraordinary", string.Empty), // Name
      field_1530 = class_134.method_253("The Glyph of the Extraordinary enables extra transmutations on the Ordinals when it is present in the alchemical engine.", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = t.extraordinaryGlow, // Shadow/glow
      field_1550 = t.extraordinaryStroke, // Stroke/outline
      field_1547 = t.extraordinaryPanel, // Panel icon
      field_1548 = t.extraordinaryPanel, // Hovered panel icon
      field_1540 = new HexIndex[]{
                new(0, 0),
            }, // Spaces used
      field_1551 = Permissions.None,
      CustomPermissionCheck = perms => perms.Contains("extransmutations-extraordinary"),
    };

    QApi.AddPartType(glyphExtraordinary, (part, pos, editor, renderer) => {
      Vector2 centre = t.extraordinaryBase.method_691();
      var time = editor.method_504();
      class_236 uco = editor.method_1989(part, pos);
      PartSimState pss = editor.method_507().method_481(part);
      renderer.method_523(t.extraordinaryBase, new Vector2(-1, -1), centre, 0f);
      renderer.method_528(t.markerLighting, new(0, 0), Vector2.Zero); 
      renderer.method_529(t.extraordinaryGlyph, new(0, 0), Vector2.Zero); 
    });
    if (Brimstone.API.IsModLoaded("UncommonPrimes")) {
      QApi.AddPartTypeToPanel(glyphExtraordinary, false);
    }
    return glyphExtraordinary;
  }

}
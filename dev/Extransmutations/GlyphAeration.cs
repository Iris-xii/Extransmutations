using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VanillaAtoms = Brimstone.API.VanillaAtoms;

public static class GlyphAeration {
  public static PartType LoadPuzzleContent(Textures t) {
    QApi.AddPuzzlePermission("extransmutations-aeration",
    "Glyph of Aeration",
    "Extransmutations");

    PartType glyphAeration = new() {
      field_1528 = "extransmutations-aeration", // ID
      field_1529 = class_134.method_253("Glyph of Aeration", string.Empty), // Name
      field_1530 = class_134.method_253("The Glyph of Aeration turns an atom of salt into Air.", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = t.aerationGlow, // Shadow/glow
      field_1550 = t.aerationStroke, // Stroke/outline
      field_1547 = t.aerationBase, // Panel icon
      field_1548 = t.aerationBase, // Hovered panel icon
      field_1540 = new HexIndex[]{
                new(0, 0),
                new(1, 0),
                new(-1, 0),
                new(-1,1),
                new(0,1),
                new(0,-1),
                new(1,-1)
            }, // Spaces used
      field_1551 = Permissions.None,
      CustomPermissionCheck = perms => perms.Contains("extransmutations-aeration"),
    };
    QApi.AddPartType(glyphAeration, (part, pos, editor, renderer) => {
      Vector2 centre = t.aerationBase.method_691();
      var time = editor.method_504();
      class_236 uco = editor.method_1989(part, pos);
      PartSimState pss = editor.method_507().method_481(part);
      renderer.method_523(t.aerationBase, new Vector2(-1, -1), centre, 0f); 
      renderer.method_523(t.aerationBaseDetail, new Vector2(-1, -1), centre, 0f); 
      renderer.method_528(t.bowlHole, new HexIndex(0, 0), Vector2.Zero);
      renderer.method_529(t.saltBowlGlyphSpotWhite, new HexIndex(0, 0), Vector2.Zero);
      // iris

      var offset = uco.field_1984 + class_187.field_1742.method_492(new HexIndex(1, 0)).Rotated(uco.field_1985);
      renderer.method_528(class_238.field_1989.field_90.field_228.field_272, new HexIndex(1, 0), Vector2.Zero);

      int irisFrame = 15;
      bool afterIrisOpens = false;
      var targetAt = pss.field_2743 ? Molecule.method_1121(pss.field_2744[0]) : null;
      if (pss.field_2743) {
        irisFrame = class_162.method_404((int)(class_162.method_411(1f, -1f, time) * 16f), 0, 15);
        afterIrisOpens = time > 0.5f;
        if (!afterIrisOpens && targetAt != null) {
          Editor.method_925(targetAt, offset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null); 
        }
      }

      // class_238.field_1989.field_90.field_246[irisFrame] 
      renderer.method_529(t.airIris[irisFrame], new HexIndex(1, 0), Vector2.Zero);
      renderer.method_528(class_238.field_1989.field_90.field_228.field_271, new HexIndex(1, 0), Vector2.Zero);

      if (pss.field_2743 && afterIrisOpens) {
        Editor.method_925(targetAt, offset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
      }
    });
    QApi.AddPartTypeToPanel(glyphAeration, false);
    return glyphAeration;
  }

  public static void Activate(bool firstHalf, Sim sim, SolutionEditorBase seb, Part part, Textures t) {
    PartSimState pss = sim.field_3821[part];
    if (pss.field_2743) {
      Brimstone.API.AddAtom(sim, part, new(1, 0), pss.field_2744[0]);
    }
    if (firstHalf && sim.FindAtomRelative(part, new(0, 0)).method_99(out AtomReference saltSpot)) {
      var occupiedAir = sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference _);
      bool saltNotHeld = !saltSpot.field_2281 && !saltSpot.field_2282;
      bool doTransmute =
        !occupiedAir
        && saltNotHeld
        && saltSpot.field_2280 == VanillaAtoms.salt;
      if (doTransmute) {
        pss.field_2743 = true;
        pss.field_2744 = new AtomType[] { VanillaAtoms.air };
        Brimstone.API.RemoveAtom(saltSpot);
        Brimstone.API.DrawFallingAtom(seb,saltSpot);
      }
    }
  }
}
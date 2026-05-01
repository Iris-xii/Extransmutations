
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VanillaAtoms = Brimstone.API.VanillaAtoms;

public static class GlyphDejection {

  public static PartType LoadPuzzleContent(Textures t) {

    QApi.AddPuzzlePermission("extransmutations-dejection",
    "Glyph of Dejection",
    "Extransmutations");

    PartType glyphDejection = new() {
      field_1528 = "extransmutations-dejection", // ID
      field_1529 = class_134.method_253("Glyph of Dejection", string.Empty), // Name
      field_1530 = class_134.method_253("The Glyph of Dejection transmutes a cardinal into an atom of Ichor and produces two cardinals one step backwards along the Cardinal Cycle (Fire -> x2 Earth -> x2 Water -> x2 Air -> x2 Fire)", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = t.dejectionGlow, // Shadow/glow
      field_1550 = t.dejectionStroke, // Stroke/outline
      field_1547 = t.dejectionBase, // Panel icon
      field_1548 = t.dejectionBase, // Hovered panel icon
      field_1540 = new HexIndex[]{
                new(0, 0),
                new(1, 0),
                new(0, 1),
            }, // Spaces used
      field_1551 = Permissions.None,
      CustomPermissionCheck = perms => perms.Contains("extransmutations-dejection"),
    };
    QApi.AddPartType(glyphDejection, (part, pos, editor, renderer) => {
      Vector2 centre = t.dejectionBase.method_691();
      var time = editor.method_504();
      class_236 uco = editor.method_1989(part, pos);
      PartSimState pss = editor.method_507().method_481(part);
      renderer.method_523(t.dejectionBase, new Vector2(-1, -1), centre, 0f);
      renderer.method_528(t.bowlHole, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_528(t.bowlHole, new HexIndex(0, 1), Vector2.Zero);
      renderer.method_528(t.bowlTexture, new HexIndex(0, 0), Vector2.Zero);

      renderer.method_529(t.anyCardinal, new HexIndex(0, 0), Vector2.Zero);
      //ty Green/Halving Metallurgy for helping me figure out... all this :s
      var offset1 = uco.field_1984 + class_187.field_1742.method_492(new HexIndex(1, 0)).Rotated(uco.field_1985);
      var offset2 = uco.field_1984 + class_187.field_1742.method_492(new HexIndex(0, 1)).Rotated(uco.field_1985);

      // iris
      renderer.method_528(class_238.field_1989.field_90.field_228.field_272, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_528(class_238.field_1989.field_90.field_228.field_272, new HexIndex(0, 1), Vector2.Zero);
      int irisFrame = 15;
      bool afterIrisOpens = false;
      var targetAt = pss.field_2743 ? Molecule.method_1121(pss.field_2744[0]) : null;
      if (pss.field_2743 ) {
        irisFrame = class_162.method_404((int)(class_162.method_411(1f, -1f, time) * 16f), 0, 15);
        afterIrisOpens = time > 0.5f;
        if (!afterIrisOpens && targetAt != null) {
          Editor.method_925(targetAt, offset1, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
          Editor.method_925(targetAt, offset2, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
        }
      }
      renderer.method_529(class_238.field_1989.field_90.field_246[irisFrame], new HexIndex(1, 0), Vector2.Zero);
      renderer.method_528(class_238.field_1989.field_90.field_228.field_271, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_529(class_238.field_1989.field_90.field_246[irisFrame], new HexIndex(0, 1), Vector2.Zero);
      renderer.method_528(class_238.field_1989.field_90.field_228.field_271, new HexIndex(0, 1), Vector2.Zero);
      
      if (pss.field_2743 && afterIrisOpens) {
        Editor.method_925(targetAt, offset1, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
        Editor.method_925(targetAt, offset2, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
      }
      renderer.method_529(t.backCardinal, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_529(t.backCardinal, new HexIndex(0, 1), Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(glyphDejection, false);
    return glyphDejection;
  }
  //(Fire -> x2 Earth -> x2 Water -> x2 Air -> x2 Fire)
  public static void Activate(bool firstHalf, Sim sim, SolutionEditorBase seb, Part part, Textures t) {
    var output1 = new HexIndex(1, 0);
    var output2 = new HexIndex(0, 1);
    PartSimState pss = sim.field_3821[part];
    if (pss.field_2743) {
      Brimstone.API.AddAtom(sim, part, output1, pss.field_2744[0]);
      Brimstone.API.AddAtom(sim, part, output2, pss.field_2744[0]);
    }
    if (firstHalf && sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference anyCard)) {
      var occupied1 = sim.FindAtomRelative(part, new HexIndex(1, 0)).method_99(out AtomReference _);
      var occupied2 = sim.FindAtomRelative(part, new HexIndex(0, 1)).method_99(out AtomReference _);
      bool doTransmute = !occupied1 && !occupied2;
      AtomType target = VanillaAtoms.salt;
      if (anyCard.field_2280 == VanillaAtoms.fire) { target = VanillaAtoms.earth; }
      if (anyCard.field_2280 == VanillaAtoms.earth) { target = VanillaAtoms.water; }
      if (anyCard.field_2280 == VanillaAtoms.water) { target = VanillaAtoms.air; }
      if (anyCard.field_2280 == VanillaAtoms.air) { target = VanillaAtoms.fire; }
      doTransmute = doTransmute && (target != VanillaAtoms.salt);

      if (doTransmute) {
        pss.field_2743 = true;
        pss.field_2744 = new AtomType[] { target };
        Brimstone.API.ChangeAtom(anyCard, ExtransmutationsMod.Ichor);
        anyCard.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, anyCard.field_2280, class_238.field_1989.field_81.field_614, 60f);
      }
    }
  }

}
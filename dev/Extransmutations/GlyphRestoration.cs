using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VanillaAtoms = Brimstone.API.VanillaAtoms;


using static ExtransmutationsMod;

public static class GlyphRestoration {

  public static PartType LoadPuzzleContent(Resources t) {
    QApi.AddPuzzlePermission("extransmutations-restoration",
    "Glyph of Restoration",
    "Extransmutations: Ichor");
    PartType glyphRestoration = new() {
      field_1528 = "extransmutations-restoration", // ID
      field_1529 = class_134.method_253("Glyph of Restoration", string.Empty), // Name
      field_1530 = class_134.method_253("The Glyph of Restoration destroys all atoms of ichor present in the molecule over the ichor bowl and all atoms of the cardinal over the cardinal bowl when the number of ichor atoms and the number of said cardinal atoms present is exactly the same.", string.Empty), // Description
      field_1531 = 20, // EXPENSIVE!!! Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = t.restorationGlow, // Shadow/glow
      field_1550 = t.restorationStroke, // Stroke/outline
      field_1547 = t.restorationPanel, // Panel icon
      field_1548 = t.restorationPanel, // Hovered panel icon
      field_1540 = new HexIndex[]{
                new(0, 0),
                new(1, 0)
            }, // Spaces used
      field_1551 = Permissions.None,
      CustomPermissionCheck = perms => perms.Contains("extransmutations-restoration"),
    };

    QApi.AddPartType(glyphRestoration, (part, pos, editor, renderer) => {
      Vector2 centre = t.restorationBase.method_691();
      var time = editor.method_504();
      class_236 uco = editor.method_1989(part, pos);
      PartSimState pss = editor.method_507().method_481(part);
      renderer.method_523(t.restorationBase, new Vector2(-1, -1), centre, 0f);
      renderer.method_528(t.bowlTexture, new(0, 0), Vector2.Zero);
      renderer.method_528(t.bowlTexture, new(1, 0), Vector2.Zero);
      renderer.method_529(t.ichorIcon, new(0, 0), Vector2.Zero);
      renderer.method_529(t.anyCardinal, new(1, 0), Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(glyphRestoration, false);
    return glyphRestoration;
  }

  public static void Activate(Sim sim,
      SolutionEditorBase seb,
      Part part,
      Resources t) {
    var offsetFlash = new Vector2(-72, 0);

    var solution = seb.method_502();
    var puzzle = solution.method_1934();
    var partList = solution.field_3919;

    List<AtomType> allowedCardinals = API.restorationCardinals
      .Where(c => API.ConditionsOk(c.conditions, puzzle, partList))
      .Select(c => c.cardinal)
      .ToList();
    if (sim.FindAtomRelative(part, new(0, 0)).method_99(out var ichorRef)
        && sim.FindAtomRelative(part, new(1, 0)).method_99(out var cardinalRef)
        && allowedCardinals.Contains(cardinalRef.field_2280)
        && ichorRef.field_2280 == ExtransmutationsMod.Ichor) {
      var cardinalType = cardinalRef.field_2280;
      var moleculeIchor = ichorRef.field_2277;
      var moleculeCardinal = cardinalRef.field_2277;
      var ichorNumber = moleculeIchor.method_1100().Count(
        kv => kv.Value.field_2275 == ExtransmutationsMod.Ichor);
      var cardinalNumber = moleculeCardinal.method_1100().Count(
        kv => kv.Value.field_2275 == cardinalType);
      if (ichorNumber == cardinalNumber && ichorNumber != 0) { //transmute!
        foreach (var hex in moleculeIchor.method_1100().Where(
            kv => kv.Value.field_2275 == ExtransmutationsMod.Ichor)
            .Select(kv => kv.Key)
            .ToList()) {
          Brimstone.API.RemoveHexFromMolecule(moleculeIchor, hex);
          seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(hex), t.destroyAnim, 30f, offsetFlash, /*part.method_1163().ToRadians()*/ 0f));
        }
        foreach (var hex in moleculeCardinal.method_1100().Where(
            kv => kv.Value.field_2275 == cardinalType)
            .Select(kv => kv.Key)
            .ToList()) {
          Brimstone.API.RemoveHexFromMolecule(moleculeCardinal, hex);
          seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(hex), t.destroyAnim, 30f, offsetFlash, /*part.method_1163().ToRadians()*/ 0f));
        }
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), t.bowlGlow, 30f, Vector2.Zero, 0f));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 0))), t.bowlGlow, 30f, Vector2.Zero, 0f));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(1, 0))), t.anyGlowArray, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 0))), t.ichorGlowArray, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
        t.restorationSound.field_4062 = false;
        t.restorationSound.method_28(seb.method_506());
        Brimstone.API.ForceRecomputeBonds(moleculeCardinal);
        Brimstone.API.ForceRecomputeBonds(moleculeIchor);
      }
    }
  }
  public static void DefaultCardinals() {
    API.AddRestorationCardinal(VanillaAtoms.water);
    API.AddRestorationCardinal(VanillaAtoms.earth);
    API.AddRestorationCardinal(VanillaAtoms.fire);
    API.AddRestorationCardinal(VanillaAtoms.air);
    if (uncommonPrimesAtoms.bellum is not null) {
      API.AddRestorationCardinal(uncommonPrimesAtoms.bellum, API.ExtraordinaryConditions());
      API.AddRestorationCardinal(uncommonPrimesAtoms.pax, API.ExtraordinaryConditions());
      API.AddRestorationCardinal(uncommonPrimesAtoms.lux, API.ExtraordinaryConditions());
      API.AddRestorationCardinal(uncommonPrimesAtoms.obscurum, API.ExtraordinaryConditions());
    }
  }

}

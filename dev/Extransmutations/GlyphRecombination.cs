
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VanillaAtoms = Brimstone.API.VanillaAtoms;


public static class GlyphRecombination {

  public static PartType LoadPuzzleContent(Textures t) {
    QApi.AddPuzzlePermission("extransmutations-recombination",
    "Glyph of Recombination",
    "Extransmutations");

    PartType glyphRecombination = new() {
      field_1528 = "extransmutations-recombination", // ID
      field_1529 = class_134.method_253("Glyph of Recombination", string.Empty), // Name
      field_1530 = class_134.method_253("The Glyph of Recombination destroys two atoms of salt to turn an atom of Ichor back into salt.", string.Empty), // Description
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
      CustomPermissionCheck = perms => perms.Contains("extransmutations-recombination"),
    };
    QApi.AddPartType(glyphRecombination, (part, pos, editor, renderer) => {
      Vector2 centre = t.dejectionBase.method_691();
      var time = editor.method_504();
      class_236 uco = editor.method_1989(part, pos);
      PartSimState pss = editor.method_507().method_481(part);
      renderer.method_523(t.dejectionBase, new Vector2(-1, -1), centre, 0f);
      renderer.method_528(t.bowlHole, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_528(t.bowlHole, new HexIndex(0, 1), Vector2.Zero);
      renderer.method_528(t.bowlTexture, new HexIndex(0, 0), Vector2.Zero);

      renderer.method_529(t.ichorIcon, new HexIndex(0, 0), Vector2.Zero);
      renderer.method_529(t.saltBowlGlyphSpotWhite, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_529(t.saltBowlGlyphSpotWhite, new HexIndex(0, 1), Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(glyphRecombination, false);
    return glyphRecombination;
  }
  public static void Activate(Sim sim, SolutionEditorBase seb, Part part, Textures t) {
    var allPresent = sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference ichor);
    allPresent = sim.FindAtomRelative(part, new HexIndex(1, 0)).method_99(out AtomReference salt1) && allPresent;
    allPresent = sim.FindAtomRelative(part, new HexIndex(0, 1)).method_99(out AtomReference salt2) && allPresent;
    if (allPresent) {
      var saltNotHeld = !salt1.field_2281 && !salt1.field_2282 &&
        !salt2.field_2281 && !salt2.field_2282;
      var saltIsSalt = (salt1.field_2280 == VanillaAtoms.salt) &&
        (salt2.field_2280 == VanillaAtoms.salt);
      var ichorIsIchor = ichor.field_2280 == ExtransmutationsMod.Ichor;
      var transmute = saltNotHeld && saltIsSalt && ichorIsIchor;

      if (transmute) {

        Brimstone.API.ChangeAtom(ichor, VanillaAtoms.salt);
        Brimstone.API.RemoveAtom(salt1);
        Brimstone.API.RemoveAtom(salt2);
        var offsetFlash = new Vector2(-72,0);
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), t.destroyAnim, 30f, offsetFlash, /*part.method_1163().ToRadians()*/ 0f));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 1))), t.destroyAnim, 30f, offsetFlash, /*part.method_1163().ToRadians()*/ 0f));
      }
    }
  }

}
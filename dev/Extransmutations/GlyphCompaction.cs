using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VanillaAtoms = Brimstone.API.VanillaAtoms;

public static class GlyphCompaction {

  public static PartType LoadPuzzleContent(Textures t) {
    
    QApi.AddPuzzlePermission("extransmutations-compaction",
    "Glyph of Compaction",
    "Extransmutations");
    PartType glyphCompaction = new() {
      field_1528 = "extransmutations-compaction", // ID
      field_1529 = class_134.method_253("Glyph of Compaction", string.Empty), // Name
      field_1530 = class_134.method_253("The Glyph of Compaction transmutes an atom of salt into an atom of Earth if it has 3 or more bonds. A full triplex counts as three bonds.", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = t.compactionGlow, // Shadow/glow
      field_1550 = t.compactionStroke, // Stroke/outline
      field_1547 = t.compactionPanel, // Panel icon
      field_1548 = t.compactionPanel, // Hovered panel icon
      field_1540 = new HexIndex[]{
                new(0, 0), 
            }, // Spaces used
      field_1551 = Permissions.None,
      CustomPermissionCheck = perms => perms.Contains("extransmutations-compaction"),
    };
    QApi.AddPartType(glyphCompaction, (part, pos, editor, renderer) => {
      Vector2 centre = t.compactionBase.method_691();
      var time = editor.method_504();
      class_236 uco = editor.method_1989(part, pos);
      PartSimState pss = editor.method_507().method_481(part);
      renderer.method_523(t.compactionBase, new Vector2(-1, -1), centre, 0f); 
      renderer.method_528(t.bowlTexture, new(0, 0), Vector2.Zero); 
      renderer.method_529(t.earthGlyph, new(0, 0), Vector2.Zero); 
    });
    QApi.AddPartTypeToPanel(glyphCompaction, false);
    return glyphCompaction; 
  }

  public static void Activate(Sim sim, SolutionEditorBase seb, Part part, Textures t) {
    if(sim.FindAtomRelative(part,new(0,0)).method_99(out AtomReference salt)) {
      Molecule m = salt.field_2277; 
      HexIndex saltLocation = salt.field_2278;
      var bonds = m.method_1101();
      int count = 0;
      foreach (var bond in bonds)
      {
        if(bond.field_2187 == saltLocation || bond.field_2188 == saltLocation) {
          int counts_as = 0;
          enum_126 enum_126 = bond.field_2186;
          if((enum_126 & enum_126.Prisma0) > 0) counts_as += 1;
          if((enum_126 & enum_126.Prisma1) > 0) counts_as += 1;
          if((enum_126 & enum_126.Prisma2) > 0) counts_as += 1;
          if(counts_as == 0) counts_as = 1;
          count += counts_as;
        }
      }
      //Logger.Log($"[extransmutations-bond-count]: {count}");
      bool doTransmute = 
        (salt.field_2280 == VanillaAtoms.salt) 
        && count >= 3;
      if (doTransmute) {
        Brimstone.API.ChangeAtom(salt,VanillaAtoms.earth);
        salt.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, salt.field_2280, class_238.field_1989.field_81.field_614, 60f); //30f
        t.activationSound.field_4062 = false;
        t.activationSound.method_28(seb.method_506()); 
      }
    }
  }
}
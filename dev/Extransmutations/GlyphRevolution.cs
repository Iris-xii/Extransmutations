
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;

public static class GlyphRevolution {
  public static PartType LoadPuzzleContent(Textures t) {
    QApi.AddPuzzlePermission("extransmutations-cardinal-revolution",
    "Glyph of Cardinal Revolution",
    "Extransmutations");
    PartType cardinalCycle = new() {
      field_1528 = "extransmutations-cardinal-revolution", // ID
      field_1529 = class_134.method_253("Glyph of Cardinal Revolution", string.Empty), // Name
      field_1530 = class_134.method_253("The glyph of Cardinal Revolution transmutes three matching atoms of the same cardinal into two salt atoms, and the next cardinal in the sequence Air -> Water -> Earth -> Fire -> Air (Clockwise along the chart of alchemical primes)", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = t.cardinalCycleGlow, // Shadow/glow
      field_1550 = t.cardinalCycleStroke, // Stroke/outline
      field_1547 = t.cyclePanel, // Panel icon
      field_1548 = t.cyclePanel, // Hovered panel icon
      field_1540 = new HexIndex[]{
                new(-1, 0),
                new(0, 0),
                new(1, 0),
            }, // Spaces used
      field_1551 = Permissions.None,
      CustomPermissionCheck = perms => perms.Contains("extransmutations-cardinal-revolution"),
    };
    QApi.AddPartType(cardinalCycle, (part, pos, editor, renderer) => {
      Vector2 centre = new(123f, 47f);// new Vector2(42f, 49f) + new Vector2(0f,-1f);
      HexIndex[] calcSpots = new HexIndex[2] { new(-1, 0), new(1, 0) };
      HexIndex tranSpot = new(0, 0);
      renderer.method_523(t.cardinalCycleBase, new Vector2(-1, -1), centre, 0);
      foreach (var calcSpot in calcSpots) {
        renderer.method_528(t.bowlTexture, calcSpot, Vector2.Zero);
        renderer.method_529(t.calcifySpotTexture, calcSpot, Vector2.Zero);
      }
      renderer.method_528(t.bowlTexture, tranSpot, Vector2.Zero);
      renderer.method_529(t.transmutationCycleSpotTexture, tranSpot, Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(cardinalCycle, false);
    return cardinalCycle;
  }
  public static void Activate(Sim sim, SolutionEditorBase seb, Part part,Textures t) {
    if (sim.FindAtomRelative(part, new(-1, 0)).method_99(out AtomReference atomCal1) &&
              sim.FindAtomRelative(part, new(0, 0)).method_99(out AtomReference atomTransmute) &&
              sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference atomCal2) &&
              (atomTransmute.field_2280 == atomCal1.field_2280) &&
              (atomCal1.field_2280 == atomCal2.field_2280)
          ) {
      var sharedType = atomTransmute.field_2280;
      var saltType = AtomTypes.field_1675;
      var targetType = AtomTypes.field_1675;

      var transOk = true;
      if (sharedType == AtomTypes.field_1676)  // Air
           {
        targetType = AtomTypes.field_1679;
      }
      else if (sharedType == AtomTypes.field_1677) // Earth
      {
        targetType = AtomTypes.field_1678;
      }
      else if (sharedType == AtomTypes.field_1678) // Fire
      {
        targetType = AtomTypes.field_1676;
      }
      else if (sharedType == AtomTypes.field_1679) // Water
      {
        targetType = AtomTypes.field_1677;
      }
      else {
        transOk = false;
      }
      if (transOk) {
        atomTransmute.field_2277.method_1106(targetType, atomTransmute.field_2278);
        atomTransmute.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, atomTransmute.field_2280, class_238.field_1989.field_81.field_614, 60f); //30f

        atomCal1.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, atomCal1.field_2280, class_238.field_1989.field_81.field_614, 60f);
        atomCal1.field_2277.method_1106(saltType, atomCal1.field_2278);

        atomCal2.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, atomCal2.field_2280, class_238.field_1989.field_81.field_614, 60f);
        atomCal2.field_2277.method_1106(saltType, atomCal2.field_2278);
        class_238.field_1991.field_1844.method_28(seb.method_506());


        //Vector2 param_5378 = class_187.field_1742.method_492(part.method_1161() + new HexIndex(0, 0).Rotated(part.method_1163()));
        //Vector2 param_5378 = class_187.field_1742.method_492(part.method_1161() + new HexIndex(0, 0).Rotated(part.method_1163()));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(-1, 0))), t.calcifyAnimation, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));

        //Vector2 param_5378b = class_187.field_1742.method_492(part.method_1161() + new HexIndex(2, 0).Rotated(part.method_1163()));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), t.calcifyAnimation, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
      }
    }
  }
}
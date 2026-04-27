
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;

public class ExtransmutationsMod : QuintessentialMod {


  // Salt: AtomTypes.field_1675;
  // Air: AtomTypes.field_1676;
  // AtomTypes.field_1677; // EARTH
  // AtomTypes.field_1678; // FIRE
  // AtomTypes.field_1679; // Water
  // AtomTypes.field_1680; // Qs
  // AtomTypes.field_1681;  // LEAD
  // AtomTypes.field_1682;  // COPPER


  public override void Load() {
  }

  public override void PostLoad() {
  }

  public override void LoadPuzzleContent() {
    Texture[] calcifyAnimation =
   Brimstone.API.GetAnimation("textures/parts/calcification_glyph_flash.array", "calcify_glyph", 10);
    Texture bowlTexture = class_235.method_615("textures/parts/calcinator_bowl");
    Texture bowlProjTexture = class_235.method_615("textures/parts/projection_glyph/metal_bowl");
    Texture bondBowlTexture = class_235.method_615("textures/parts/bonder_ring");
    Texture transmutationInversionSpotTexture = class_235.method_615("textures/parts/cardinal_inversion_symbols");
    Texture transmutationCycleSpotTexture = class_235.method_615("textures/parts/cardinal_cycle_symbols");
    Texture calcifySpotTexture = class_235.method_615("textures/parts/calcinator_symbols");
    Texture saltBowlGlyphSpot = class_235.method_615("textures/parts/salt_glyph");
    Texture cardinalCompletionGlyphAny = class_235.method_615("textures/parts/any_cardinal_completion");
    Texture diamondGlow = class_235.method_615("textures/select/diamond_glow");
    Texture oneByThreeGlow = class_235.method_615("textures/select/1by3_glow");
    Texture completionPanel = class_235.method_615("textures/panel/completion_panel");
    Texture inversionPanel = class_235.method_615("textures/panel/invert_panel");
    Texture cyclePanel = class_235.method_615("textures/panel/cycle_panel");
    Texture cardinalCycleBase = class_235.method_615("textures/parts/cardinal_revolution/base");
    Texture cardinalCycleGlow = class_235.method_615("textures/parts/cardinal_revolution/glow");
    Texture cardinalCycleStroke = class_235.method_615("textures/parts/cardinal_revolution/stroke");
    Texture cardinalInversionBase = class_235.method_615("textures/parts/cardinal_inversion/base");
    Texture cardinalInversionGlow = class_235.method_615("textures/parts/cardinal_inversion/glow");
    Texture cardinalInversionStroke = class_235.method_615("textures/parts/cardinal_inversion/stroke");
    Texture cardinalCompletionBase = class_235.method_615("textures/parts/completion_base");
    Texture cardinalCompletionGlow = class_235.method_615("textures/parts/completion/glow");
    Texture cardinalCompletionStroke = class_235.method_615("textures/parts/completion/stroke");

    QApi.AddPuzzlePermission("extransmutations-cardinal-revolution",
    "Glyph of Cardinal Revolution",
    "Extransmutations");
    PartType cardinalCycle = new() {
      field_1528 = "extransmutations-cardinal-revolution", // ID
      field_1529 = class_134.method_253("Glyph of Cardinal Revolution", string.Empty), // Name
      field_1530 = class_134.method_253("The glyph of Cardinal Revolution transmutes three matching atoms of the same cardinal into two salt atoms, and the next cardinal in the sequence Air -> Water -> Earth -> Fire -> Air (Clockwise along the chart of alchemical primes)", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = cardinalCycleGlow, // Shadow/glow
      field_1550 = cardinalCycleStroke, // Stroke/outline
      field_1547 = cyclePanel, // Panel icon
      field_1548 = cyclePanel, // Hovered panel icon
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
      renderer.method_523(cardinalCycleBase, new Vector2(-1, -1), centre, 0);
      foreach (var calcSpot in calcSpots) {
        renderer.method_528(bowlTexture, calcSpot, Vector2.Zero);
        renderer.method_529(calcifySpotTexture, calcSpot, Vector2.Zero);
      }
      renderer.method_528(bowlTexture, tranSpot, Vector2.Zero);
      renderer.method_529(transmutationCycleSpotTexture, tranSpot, Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(cardinalCycle, false);

    QApi.AddPuzzlePermission("extransmutations-cardinal-inversion",
    "Glyph of Cardinal Inversion",
    "Extransmutations");
    PartType cardinalInversion = new() {
      field_1528 = "extransmutations-cardinal-inversion", // ID
      field_1529 = class_134.method_253("Glyph of Cardinal Inversion", string.Empty), // Name
      field_1530 = class_134.method_253("The glyph of cardinal inversion transmutes two cardinals into an opposing cardinal and salt.", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = cardinalInversionGlow, // Shadow/glow
      field_1550 = cardinalInversionStroke, // Stroke/outline
      field_1547 = inversionPanel, // Panel icon
      field_1548 = inversionPanel, // Hovered panel icon
      field_1540 = new HexIndex[]{
                new(0, 0),
                new(1, 0),
            }, // Spaces used
      field_1551 = Permissions.None,
      CustomPermissionCheck = perms => perms.Contains("extransmutations-cardinal-inversion"),
    };
    QApi.AddPartType(cardinalInversion, (part, pos, editor, renderer) => {
      Vector2 centre = new(123f, 47f);// new Vector2(42f, 49f) + new Vector2(0f,-1f);
      HexIndex calcSpot = new(1, 0);
      HexIndex tranSpot = new(0, 0);
      renderer.method_523(cardinalInversionBase, new Vector2(-1, -1), centre, 0);
      renderer.method_528(bowlTexture, calcSpot, Vector2.Zero);
      renderer.method_528(bowlTexture, tranSpot, Vector2.Zero);
      renderer.method_529(calcifySpotTexture, calcSpot, Vector2.Zero);
      renderer.method_529(transmutationInversionSpotTexture, tranSpot, Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(cardinalInversion, false);


    QApi.AddPuzzlePermission("extransmutations-cardinal-completion",
    "Glyph of Cardinal Completion",
    "Extransmutations");
    PartType cardinalCompletion = new() {
      field_1528 = "extransmutations-cardinal-completion", // ID
      field_1529 = class_134.method_253("Glyph of Cardinal Completion", string.Empty), // Name
      field_1530 = class_134.method_253("The Glyph of Cardinal Completion transmutes 3 different cardinals and 1 salt atom into 4 atoms of the missing cardinal.", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = cardinalCompletionGlow, // Shadow/glow
      field_1550 = cardinalCompletionStroke, // Stroke/outline
      field_1547 = completionPanel, // Panel icon
      field_1548 = completionPanel, // Hovered panel icon
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
      renderer.method_523(cardinalCompletionBase, new Vector2(-1, -1), centre, 0);
      renderer.method_528(bowlProjTexture, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_528(bowlProjTexture, new HexIndex(0, 0), Vector2.Zero);
      renderer.method_528(bowlProjTexture, new HexIndex(1, -1), Vector2.Zero);
      renderer.method_528(bowlProjTexture, new HexIndex(0, 1), Vector2.Zero);

      renderer.method_529(cardinalCompletionGlyphAny, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_529(cardinalCompletionGlyphAny, new HexIndex(0, 0), Vector2.Zero);
      renderer.method_529(cardinalCompletionGlyphAny, new HexIndex(1, -1), Vector2.Zero);
      renderer.method_529(saltBowlGlyphSpot, new HexIndex(0, 1), Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(cardinalCompletion, false);

    QApi.RunAfterCycle((sim, first) => {
      var seb = sim.field_3818;
      var solution = seb.method_502();
      var partList = solution.field_3919;
      //var partSimStates = sim.field_3821;
      //var struct122List = sim.field_3826;
      //var moleculeList = sim.field_3823;
      //var gripperList = sim.HeldGrippers;
      foreach (Part part in partList) {
        var partType = part.method_1159();
        if (partType == cardinalCycle /*&& first*/) {
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
              seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(-1, 0))), calcifyAnimation, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));

              //Vector2 param_5378b = class_187.field_1742.method_492(part.method_1161() + new HexIndex(2, 0).Rotated(part.method_1163()));
              seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), calcifyAnimation, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
            }
          }
        }
        if (partType == cardinalInversion) {
          if (sim.FindAtomRelative(part, new(0, 0)).method_99(out AtomReference atomTransmute) &&
            sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference atomCalcify) &&
            /*!atomTransmute.field_2282 && !atomCalcify.field_2282 && */
            (atomTransmute.field_2280 == atomCalcify.field_2280)) // same atom  
          {
            var sharedType = atomTransmute.field_2280;
            var saltType = AtomTypes.field_1675;
            var targetType = AtomTypes.field_1675;

            var transOk = true;
            if (sharedType == AtomTypes.field_1676) // Air
            {
              targetType = AtomTypes.field_1677;
            }
            else if (sharedType == AtomTypes.field_1677) // Earth
            {
              targetType = AtomTypes.field_1676;
            }
            else if (sharedType == AtomTypes.field_1678) // Fire
            {
              targetType = AtomTypes.field_1679;
            }
            else if (sharedType == AtomTypes.field_1679) // Water
            {
              targetType = AtomTypes.field_1678;
            }
            else {
              transOk = false;
            }
            if (transOk) {
              atomTransmute.field_2277.method_1106(targetType, atomTransmute.field_2278);
              atomCalcify.field_2277.method_1106(saltType, atomCalcify.field_2278);

              atomTransmute.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, atomTransmute.field_2280, class_238.field_1989.field_81.field_614, 60f); //30f
              atomCalcify.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, atomCalcify.field_2280, class_238.field_1989.field_81.field_614, 60f);


              seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), calcifyAnimation, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
              class_238.field_1991.field_1844.method_28(seb.method_506());
            }
          }
        }
        if (partType == cardinalCompletion) {
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
              seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), calcifyAnimation, 30f, Vector2.Zero, 0f));
              seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 0))), calcifyAnimation, 30f, Vector2.Zero, 0f));
              seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, -1))), calcifyAnimation, 30f, Vector2.Zero, 0f));
              class_238.field_1991.field_1844.method_28(seb.method_506());
            }
          }
        }
      }
    });
  }

  public override void Unload() {
  }
}

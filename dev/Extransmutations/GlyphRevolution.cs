
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VA = Brimstone.API.VanillaAtoms;
using static ExtransmutationsMod;

internal class GlyphRevolution {
  internal enum Version : byte { V1, V2 }
  private static T PerVersion<T>(Version v, T ifV1, T ifV2) =>
    v switch {
      Version.V1 => ifV1,
      Version.V2 => ifV2,
      _ => throw new NotImplementedException(),
    };
  private T PerVersion<T>(T ifV1, T ifV2) => PerVersion(this.v, ifV1, ifV2);

  internal Version v;
  internal PartType pt;

#pragma warning disable format
  public static HexIndex CalcSpot1(Version v) => PerVersion<HexIndex>(    v, new(-1, 0), new( 0, 0) );
  public static HexIndex CalcSpot2(Version v) => PerVersion<HexIndex>(    v, new( 1, 0), new( 1, 0) );
  public static HexIndex TransmuteSpot(Version v) => PerVersion<HexIndex>(v, new( 0, 0), new( 0, 1) );
#pragma warning restore format

  internal static GlyphRevolution LoadPuzzleContent(Resources t, Version v) {
    string ID = $"extransmutations-cardinal-revolution{PerVersion(v, "", "-v2")}";
    if (v == Version.V2) {
      QApi.AddPuzzlePermission(ID,
      "Glyph of Cardinal Revolution",
      "Extransmutations");
    }
    Texture glow = PerVersion(v, t.cardinalCycleGlow, t.cardinalCycleGlowV2);
    Texture outline = PerVersion(v, t.cardinalCycleStroke, t.cardinalCycleStrokeV2);
    Texture panel = PerVersion(v, t.cyclePanel, t.cyclePanelV2);
    Texture glyphBase = PerVersion(v, t.cardinalCycleBase, t.cardinalCycleBaseV2);
    Vector2 vecCenter = PerVersion(v, new(123f, 47f), glyphBase.method_691());
    PartType cardinalCycle = new() {
      field_1528 = ID, // ID
      field_1529 = class_134.method_253("Glyph of Cardinal Revolution", string.Empty), // Name
      field_1530 = class_134.method_253("The glyph of Cardinal Revolution transmutes three matching atoms of the same cardinal into two salt atoms, and the next cardinal in the sequence Air -> Water -> Earth -> Fire -> Air (Clockwise along the chart of alchemical primes)", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = glow, // Shadow/glow
      field_1550 = outline, // Stroke/outline
      field_1547 = panel, // Panel icon
      field_1548 = panel, // Hovered panel icon
      field_1540 = new HexIndex[]{ // Spaces used
                CalcSpot1(v),
                TransmuteSpot(v),
                CalcSpot2(v),
            },
      field_1551 = Permissions.None,
      CustomPermissionCheck = perms => perms.Contains(ID),
    };
    QApi.AddPartType(cardinalCycle, (part, pos, editor, renderer) => {
      HexIndex[] calcSpots = new HexIndex[2] { CalcSpot1(v), CalcSpot2(v) };
      HexIndex tranSpot = TransmuteSpot(v);
      renderer.method_523(glyphBase, new Vector2(-1, -1), vecCenter, 0);
      foreach (var calcSpot in calcSpots) {
        renderer.method_528(t.bowlTexture, calcSpot, Vector2.Zero);
        renderer.method_529(t.calcifySpotTexture, calcSpot, Vector2.Zero);
      }
      renderer.method_528(t.bowlTexture, tranSpot, Vector2.Zero);
      renderer.method_529(t.transmutationCycleSpotTexture, tranSpot, Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(cardinalCycle, false);
    return new GlyphRevolution() { pt = cardinalCycle, v = v };
  }

  internal void RunCycle(PartType partType, Sim sim, SolutionEditorBase seb, Part part, Resources t) {
    if (partType != this.pt) return;
    Activate(sim, seb, part, t);
  }

  private void Activate(Sim sim, SolutionEditorBase seb, Part part, Resources t) {
    if (sim.FindAtomRelative(part, CalcSpot1(this.v)).method_99(out AtomReference atomCal1) &&
              sim.FindAtomRelative(part, TransmuteSpot(this.v)).method_99(out AtomReference atomTransmute) &&
              sim.FindAtomRelative(part, CalcSpot2(this.v)).method_99(out AtomReference atomCal2) &&
              (atomTransmute.field_2280 == atomCal1.field_2280) &&
              (atomCal1.field_2280 == atomCal2.field_2280)
          ) {
      var solution = seb.method_502();
      var puzzle = solution.method_1934();
      var partList = solution.field_3919;

      var sharedType = atomTransmute.field_2280;
      foreach (var recipe in API.revolutionRecipes) {
        if (!API.ConditionsOk(recipe.conditions, puzzle, partList)) { continue; }
        if (recipe.cardinal != sharedType) { continue; }

        var targetType = recipe.transmutesTo;
        var saltType = recipe.saltOutput;


        atomTransmute.field_2277.method_1106(targetType, atomTransmute.field_2278);
        atomTransmute.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, atomTransmute.field_2280, class_238.field_1989.field_81.field_614, 60f); //30f

        atomCal1.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, atomCal1.field_2280, class_238.field_1989.field_81.field_614, 60f);
        atomCal1.field_2277.method_1106(saltType, atomCal1.field_2278);

        atomCal2.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, atomCal2.field_2280, class_238.field_1989.field_81.field_614, 60f);
        atomCal2.field_2277.method_1106(saltType, atomCal2.field_2278);
        class_238.field_1991.field_1844.method_28(seb.method_506());


        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(CalcSpot1(this.v))), t.calcifyAnimation, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));


        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(CalcSpot2(this.v))), t.calcifyAnimation, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(TransmuteSpot(this.v))), t.bowlGlow, 30f, Vector2.Zero, 0f));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(TransmuteSpot(this.v))), t.cycleGlowArray, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
        t.cardinalRotationSound.field_4062 = false;
        t.cardinalRotationSound.method_28(seb.method_506());
        break;
      }
    }
  }

  internal static void DefaultRecipes() {
    API.AddRevolutionRecipe(new() {
      conditions = API.NoConditions(),
      cardinal = VA.air,
      transmutesTo = VA.water,
      saltOutput = VA.salt,
    });
    API.AddRevolutionRecipe(new() {
      conditions = API.NoConditions(),
      cardinal = VA.water,
      transmutesTo = VA.earth,
      saltOutput = VA.salt,
    });
    API.AddRevolutionRecipe(new() {
      conditions = API.NoConditions(),
      cardinal = VA.earth,
      transmutesTo = VA.fire,
      saltOutput = VA.salt,
    });
    API.AddRevolutionRecipe(new() {
      conditions = API.NoConditions(),
      cardinal = VA.fire,
      transmutesTo = VA.air,
      saltOutput = VA.salt,
    });
    // EXTRAORDINARY
    if (uncommonPrimesAtoms.bellum is not null) {
      API.AddRevolutionRecipe(new() {
        conditions = API.ExtraordinaryConditions(),
        cardinal = uncommonPrimesAtoms.bellum,
        transmutesTo = uncommonPrimesAtoms.obscurum,
        saltOutput = VA.salt,
      });
      API.AddRevolutionRecipe(new() {
        conditions = API.ExtraordinaryConditions(),
        cardinal = uncommonPrimesAtoms.obscurum,
        transmutesTo = uncommonPrimesAtoms.pax,
        saltOutput = VA.salt,
      });
      API.AddRevolutionRecipe(new() {
        conditions = API.ExtraordinaryConditions(),
        cardinal = uncommonPrimesAtoms.pax,
        transmutesTo = uncommonPrimesAtoms.lux,
        saltOutput = VA.salt,
      });
      API.AddRevolutionRecipe(new() {
        conditions = API.ExtraordinaryConditions(),
        cardinal = uncommonPrimesAtoms.lux,
        transmutesTo = uncommonPrimesAtoms.bellum,
        saltOutput = VA.salt,
      });
    }
  }
}
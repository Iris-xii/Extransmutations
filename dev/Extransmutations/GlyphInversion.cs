
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VA = Brimstone.API.VanillaAtoms;

using static ExtransmutationsMod;

public static class GlyphInversion {
  public static PartType LoadPuzzleContent(Resources t) {
    QApi.AddPuzzlePermission("extransmutations-cardinal-inversion",
    "Glyph of Cardinal Inversion",
    "Extransmutations");
    PartType cardinalInversion = new() {
      field_1528 = "extransmutations-cardinal-inversion", // ID
      field_1529 = class_134.method_253("Glyph of Cardinal Inversion", string.Empty), // Name
      field_1530 = class_134.method_253("The glyph of cardinal inversion transmutes two matching cardinals into an opposing cardinal and salt.", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = t.cardinalInversionGlow, // Shadow/glow
      field_1550 = t.cardinalInversionStroke, // Stroke/outline
      field_1547 = t.inversionPanel, // Panel icon
      field_1548 = t.inversionPanel, // Hovered panel icon
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
      renderer.method_523(t.cardinalInversionBase, new Vector2(-1, -1), centre, 0);
      renderer.method_528(t.bowlTexture, calcSpot, Vector2.Zero);
      renderer.method_528(t.bowlTexture, tranSpot, Vector2.Zero);
      renderer.method_529(t.calcifySpotTexture, calcSpot, Vector2.Zero);
      renderer.method_529(t.transmutationInversionSpotTexture, tranSpot, Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(cardinalInversion, false);
    return cardinalInversion;
  }
  public static void Activate(Sim sim, SolutionEditorBase seb, Part part, Resources t) {
    var solution = seb.method_502();
    var puzzle = solution.method_1934();
    var partList = solution.field_3919;
    if (sim.FindAtomRelative(part, new(0, 0)).method_99(out AtomReference atomTransmute) &&
            sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference atomCalcify) &&
            /*!atomTransmute.field_2282 && !atomCalcify.field_2282 && */
            (atomTransmute.field_2280 == atomCalcify.field_2280)) {
      var sharedType = atomTransmute.field_2280;

      foreach (var recipe in API.inversionRecipes) {
        if (!API.ConditionsOk(recipe.conditions, puzzle, partList)) { continue; }
        if (sharedType != recipe.cardinal) { continue; }
        var targetType = recipe.invertsTo;
        var saltTarget = recipe.saltOutput;

        atomTransmute.field_2277.method_1106(targetType, atomTransmute.field_2278);
        atomCalcify.field_2277.method_1106(saltTarget, atomCalcify.field_2278);

        atomTransmute.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, atomTransmute.field_2280, class_238.field_1989.field_81.field_614, 60f); //30f
        atomCalcify.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, atomCalcify.field_2280, class_238.field_1989.field_81.field_614, 60f);


        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 0))), t.bowlGlow, 30f, Vector2.Zero, 0f));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(0, 0))), t.inversionGlowArray, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), t.calcifyAnimation, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
        class_238.field_1991.field_1844.method_28(seb.method_506());
        t.cardinalInversionSound.field_4062 = false; 
        t.cardinalInversionSound.method_28(seb.method_506());
        break;
      }
    }
  }

  internal static void DefaultRecipes() {
    API.AddInversionRecipe(new() {
      conditions = API.NoConditions(),
      cardinal = VA.water,
      invertsTo = VA.fire,
      saltOutput = VA.salt,
    });
    API.AddInversionRecipe(new() {
      conditions = API.NoConditions(),
      cardinal = VA.fire,
      invertsTo = VA.water,
      saltOutput = VA.salt,
    });
    API.AddInversionRecipe(new() {
      conditions = API.NoConditions(),
      cardinal = VA.earth,
      invertsTo = VA.air,
      saltOutput = VA.salt,
    });
    API.AddInversionRecipe(new() {
      conditions = API.NoConditions(),
      cardinal = VA.air,
      invertsTo = VA.earth,
      saltOutput = VA.salt,
    });
    // EXTRAORDINARY
    if (uncommonPrimesAtoms.bellum is not null) {
      API.AddInversionRecipe(new() {
        conditions = API.ExtraordinaryConditions(),
        cardinal = uncommonPrimesAtoms.bellum,
        invertsTo = uncommonPrimesAtoms.pax,
        saltOutput = VA.salt,
      });
      API.AddInversionRecipe(new() {
        conditions = API.ExtraordinaryConditions(),
        cardinal = uncommonPrimesAtoms.pax,
        invertsTo = uncommonPrimesAtoms.bellum,
        saltOutput = VA.salt,
      });
      API.AddInversionRecipe(new() {
        conditions = API.ExtraordinaryConditions(),
        cardinal = uncommonPrimesAtoms.lux,
        invertsTo = uncommonPrimesAtoms.obscurum,
        saltOutput = VA.salt,
      });
      API.AddInversionRecipe(new() {
        conditions = API.ExtraordinaryConditions(),
        cardinal = uncommonPrimesAtoms.obscurum,
        invertsTo = uncommonPrimesAtoms.lux,
        saltOutput = VA.salt,
      });
    }
  }
}
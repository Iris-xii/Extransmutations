
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VA = Brimstone.API.VanillaAtoms;

using static ExtransmutationsMod;

public static class GlyphCompletion {
  internal static readonly Dictionary<byte, string> versions = new() {
    {0,"extransmutations-cardinal-completion"},
    {1,"extransmutations-cardinal-completion-v2"},
  };
  internal static byte GetVersion(HashSet<string> perms) {
    var filtered = versions.Where((v) => perms.Contains(v.Value))
    .Select(v => v.Key);
    if (filtered.Count() <= 0) { return 0; }
    return filtered.Max();
  }

  public static PartType LoadPuzzleContent(Textures t) {
    QApi.AddPuzzlePermission("extransmutations-cardinal-completion-v2",
    "Glyph of Cardinal Completion",
    "Extransmutations");
    PartType cardinalCompletion = new() {
      field_1528 = "extransmutations-cardinal-completion", // ID
      field_1529 = class_134.method_253("Glyph of Cardinal Completion", string.Empty), // Name
      field_1530 = class_134.method_253("The Glyph of Cardinal Completion transmutes 3 different cardinals and 1 salt atom into 4 atoms of the missing cardinal. Berlo's Wheel may be placed over the bowls to mediate in the use of the glyph, fulfilling the requirement of said bowls without it itself being transmuted.", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = t.cardinalCompletionGlow, // Shadow/glow
      field_1550 = t.cardinalCompletionStroke, // Stroke/outline
      field_1547 = t.completionPanel, // Panel icon
      field_1548 = t.completionPanel, // Hovered panel icon
      field_1540 = new HexIndex[]{
                new(0, 0),
                new(1, 0),
                new(0, 1),
                new(1, -1),
            }, // Spaces used
      field_1551 = Permissions.None,
      CustomPermissionCheck =
        perms => perms.Intersect(versions.Values).Count() > 0
    };
    QApi.AddPartType(cardinalCompletion, (part, pos, editor, renderer) => {
      Vector2 centre = new(/*82.5f*/41.25f, 119.5f);// new Vector2(42f, 49f) + new Vector2(0f,-1f);
      renderer.method_523(t.cardinalCompletionBase, new Vector2(-1, -1), centre, 0);
      renderer.method_528(t.bowlTexture, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_528(t.bowlTexture, new HexIndex(0, 0), Vector2.Zero);
      renderer.method_528(t.bowlTexture, new HexIndex(1, -1), Vector2.Zero);
      renderer.method_528(t.bowlTexture, new HexIndex(0, 1), Vector2.Zero);

      renderer.method_529(t.anyCardinal, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_529(t.anyCardinal, new HexIndex(0, 0), Vector2.Zero);
      renderer.method_529(t.anyCardinal, new HexIndex(1, -1), Vector2.Zero);
      renderer.method_529(t.saltBowlGlyphSpot, new HexIndex(0, 1), Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(cardinalCompletion, false);
    return cardinalCompletion;
  }
  internal static bool GetBerlo(SolutionEditorBase seb, out Part outPart) {
    foreach (var part in seb.method_502().field_3919) {
      if (part.method_1159().field_1528 == "baron") {
        outPart = part;
        return true;
      }
    }
    outPart = null;
    return false;
  }



  internal static Molecule ServinMolec() {
    Molecule servinMolec = new();
    servinMolec.method_1105(new Atom(ExtransmutationsMod.uncommonPrimesAtoms.obscurum), new HexIndex(0, 1));
    servinMolec.method_1105(new Atom(Brimstone.API.VanillaAtoms.salt), new HexIndex(1, 0));
    servinMolec.method_1105(new Atom(ExtransmutationsMod.uncommonPrimesAtoms.pax), new HexIndex(1, -1));
    servinMolec.method_1105(new Atom(ExtransmutationsMod.uncommonPrimesAtoms.lux), new HexIndex(0, -1));
    servinMolec.method_1105(new Atom(Brimstone.API.VanillaAtoms.salt), new HexIndex(-1, 0));
    servinMolec.method_1105(new Atom(ExtransmutationsMod.uncommonPrimesAtoms.bellum), new HexIndex(-1, 1));
    return servinMolec;
  }

  internal static HexIndex[] CARDINAL_POSITIONS = new HexIndex[] { new(1, 0), new(0, 0), new(1, -1) };
  internal static HexIndex SALT_POSITION = new(0, 1);
  public static void Activate(Sim sim, SolutionEditorBase seb, Part part, Textures t) {
    var solution = seb.method_502();
    var puzzle = solution.method_1934();
    var version = GetVersion(puzzle.CustomPermissions);
    var partList = solution.field_3919;

    sim.FindAtomRelative(part, new HexIndex(1, 0)).method_99(out AtomReference c1);
    sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference c2);
    sim.FindAtomRelative(part, new HexIndex(1, -1)).method_99(out AtomReference c3);
    sim.FindAtomRelative(part, new HexIndex(0, 1)).method_99(out AtomReference salt);
    //whether they should be treated as 'catalyst' (not transmuted). this is for berlo
    bool c1Cat = false, c2Cat = false, c3Cat = false, saltCat = false;

    if (version > 0) { // do berlo
      foreach (var e in API.completionWheels) {
        if (FindBerloAtom(sim, part, new HexIndex(1, 0), e.wheelName, e.wheelMolecule).method_99(out var ba)) {
          c1 = ba;
          c1Cat = true;
        }
        if (FindBerloAtom(sim, part, new HexIndex(0, 0), e.wheelName, e.wheelMolecule).method_99(out var ba2)) {
          c2 = ba2;
          c2Cat = true;
        }
        if (FindBerloAtom(sim, part, new HexIndex(1, -1), e.wheelName, e.wheelMolecule).method_99(out var ba3)) {
          c3 = ba3;
          c3Cat = true;
        }
        if (FindBerloAtom(sim, part, new HexIndex(0, 1), e.wheelName, e.wheelMolecule).method_99(out var ba4)) {
          salt = ba4;
          saltCat = true;
        }
      }
    }

    foreach (var recipe in API.completionRecipes) {
      var cardinals = new AtomReference[] { c1, c2, c3 }
        .Where(c => c is not null)
        .Select(c => c.field_2280).ToList();
      if (!API.ConditionsOk(recipe.conditions, puzzle, partList)) { continue; }
      if (!(recipe.saltElement is null || (salt is not null && salt.field_2280 == recipe.saltElement))) { continue; }
      if (recipe.c1 is not null) {
        if (cardinals.Contains(recipe.c1)) {
          cardinals.Remove(recipe.c1);
        }
        else { continue; }
      }
      if (recipe.c2 is not null) {
        if (cardinals.Contains(recipe.c2)) {
          cardinals.Remove(recipe.c2);
        }
        else { continue; }
      }
      if (recipe.c3 is not null) {
        if (cardinals.Contains(recipe.c3)) {
          cardinals.Remove(recipe.c3);
        }
        else { continue; }
      }
      if (cardinals.Count != 0) { continue; }
      //TRANSMUTE!

      if (c1 is not null && !c1Cat) {
        Brimstone.API.ChangeAtom(c1, recipe.output);
        c1.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, c1.field_2280, class_238.field_1989.field_81.field_614, 60f);
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), t.bowlGlow, 30f, Vector2.Zero, 0f));
      }
      if (c2 is not null && !c2Cat) {
        Brimstone.API.ChangeAtom(c2, recipe.output);
        c2.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, c2.field_2280, class_238.field_1989.field_81.field_614, 60f);
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 0))), t.bowlGlow, 30f, Vector2.Zero, 0f));
      }
      if (c3 is not null && !c3Cat) {
        Brimstone.API.ChangeAtom(c3, recipe.output);
        c3.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, c3.field_2280, class_238.field_1989.field_81.field_614, 60f);
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, -1))), t.bowlGlow, 30f, Vector2.Zero, 0f));
      }
      if (salt is not null && !saltCat) {
        Brimstone.API.ChangeAtom(salt, recipe.saltOutput);
        salt.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, salt.field_2280, class_238.field_1989.field_81.field_614, 60f);
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 1))), t.bowlGlow, 30f, Vector2.Zero, 0f));
      }


      class_238.field_1991.field_1844.method_28(seb.method_506());
      seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), t.anyGlowArray, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
      seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 0))), t.anyGlowArray, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
      seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, -1))), t.anyGlowArray, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
      seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 1))), t.saltGlyphArray, 30f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
      t.fancyActivationSound.field_4062 = false;
      t.fancyActivationSound.method_28(seb.method_506());
      break;
    }
  }



  internal static void DefaultRecipes() {
    API.AddCompletionRecipe(new API.CompletionRecipe() {
      conditions = API.NoConditions(),
      saltElement = VA.salt,
      c1 = VA.water,
      c2 = VA.fire,
      c3 = VA.earth,
      output = VA.air,
      saltOutput = VA.air,
    });
    API.AddCompletionRecipe(new API.CompletionRecipe() {
      conditions = API.NoConditions(),
      saltElement = VA.salt,
      c1 = VA.air,
      c2 = VA.fire,
      c3 = VA.earth,
      output = VA.water,
      saltOutput = VA.water,
    });
    API.AddCompletionRecipe(new API.CompletionRecipe() {
      conditions = API.NoConditions(),
      saltElement = VA.salt,
      c1 = VA.air,
      c2 = VA.water,
      c3 = VA.earth,
      output = VA.fire,
      saltOutput = VA.fire,
    });
    API.AddCompletionRecipe(new API.CompletionRecipe() {
      conditions = API.NoConditions(),
      saltElement = VA.salt,
      c1 = VA.air,
      c2 = VA.water,
      c3 = VA.fire,
      output = VA.earth,
      saltOutput = VA.earth,
    });
    // EXTRAORDINARY
    if (uncommonPrimesAtoms.bellum is not null) {
      API.AddCompletionRecipe(new API.CompletionRecipe() {
        conditions = API.ExtraordinaryConditions(),
        saltElement = VA.salt,
        c1 = uncommonPrimesAtoms.pax,
        c2 = uncommonPrimesAtoms.lux,
        c3 = uncommonPrimesAtoms.obscurum,
        output = uncommonPrimesAtoms.bellum,
        saltOutput = uncommonPrimesAtoms.bellum,
      });
      API.AddCompletionRecipe(new API.CompletionRecipe() {
        conditions = API.ExtraordinaryConditions(),
        saltElement = VA.salt,
        c1 = uncommonPrimesAtoms.pax,
        c2 = uncommonPrimesAtoms.lux,
        c3 = uncommonPrimesAtoms.bellum,
        output = uncommonPrimesAtoms.obscurum,
        saltOutput = uncommonPrimesAtoms.obscurum,
      });
      API.AddCompletionRecipe(new API.CompletionRecipe() {
        conditions = API.ExtraordinaryConditions(),
        saltElement = VA.salt,
        c1 = uncommonPrimesAtoms.pax,
        c2 = uncommonPrimesAtoms.obscurum,
        c3 = uncommonPrimesAtoms.bellum,
        output = uncommonPrimesAtoms.lux,
        saltOutput = uncommonPrimesAtoms.lux,
      });
      API.AddCompletionRecipe(new API.CompletionRecipe() {
        conditions = API.ExtraordinaryConditions(),
        saltElement = VA.salt,
        c1 = uncommonPrimesAtoms.lux,
        c2 = uncommonPrimesAtoms.obscurum,
        c3 = uncommonPrimesAtoms.bellum,
        output = uncommonPrimesAtoms.pax,
        saltOutput = uncommonPrimesAtoms.pax,
      });
    }
  }
}
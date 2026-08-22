using System.Collections;
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VA = Brimstone.API.VanillaAtoms;


#nullable enable
public static partial class API {
  internal static List<Wheel> completionWheels = new();
  public static IList<Wheel> CompletionWheels { get => completionWheels; }

  internal static List<CompletionRecipe> completionRecipes = new();
  public static IList<CompletionRecipe> CompletionRecipes { get => completionRecipes; }

  internal static List<InversionRecipe> inversionRecipes = new();
  public static IList<InversionRecipe> InversionRecipes { get => inversionRecipes; }

  internal static List<RevolutionRecipe> revolutionRecipes = new();
  public static IList<RevolutionRecipe> RevolutionRecipes { get => revolutionRecipes; }

  internal static List<DejectionRecipe> dejectionRecipes = new();
  public static IList<DejectionRecipe> DejectionRecipes { get => dejectionRecipes; }

  internal static List<RestorationCardinal> restorationCardinals = new() { };
  public static IList<RestorationCardinal> RestorationCardinals { get => restorationCardinals; }


  public record struct Wheel {
    /// <summary>
    /// PartType.field_1528, the name of the wheel.
    /// </summary>
    public string wheelName;
    /// <summary>
    /// The molecule used in the wheel. If this is null it does its best
    /// by reading .field_1544
    /// </summary>
    public Molecule? wheelMolecule;
  }

  /// <summary>
  /// Condtions that must be met for a recipe to be considered
  /// </summary>
  public record struct RecipeConditions() {
    /// <summary>
    /// If present, Custom Permission required for the recipe.
    /// </summary>
    public string? requiredPerm = null;
    /// <summary>
    /// If present, this glyph must be somewhere in the solution for the recipe.
    /// </summary>
    public string? requiredGlyphName = null;
  }
  internal static bool ConditionsOk(RecipeConditions conditions, Puzzle puzzle, List<Part> partList) {
    if (conditions.requiredPerm is not null
    && !puzzle.CustomPermissions.Contains(conditions.requiredPerm)) {
      return false;
    }
    if (conditions.requiredGlyphName is not null
    && !partList.Any(p => p.method_1159().field_1528 == conditions.requiredGlyphName)) {
      return false;
    }
    return true;
  }
  public static RecipeConditions NoConditions() => new() {
    requiredPerm = null,
    requiredGlyphName = null,
  };
  internal static RecipeConditions ExtraordinaryConditions() => new() {
    requiredPerm = null,
    requiredGlyphName = "extransmutations-extraordinary",
  };

  /// <summary>
  /// Completion Recipe. c1,c2,c3 are the 'cardinal' equivalent that may go
  /// over any of the * bowls, while saltElement is what goes in the salt bowl. <br></br>
  /// 
  /// Transmutes all cardinals into output, and the salt element into saltOutput,
  /// these are the same in normal recipes.
  /// 
  /// Null AtomTypes (except output) are valid and ignored and mean 'no atom'
  /// 
  /// c1,c2,c3 are 'unordered', it doesn't matter which specific AtomType is c1 and which one is c3
  /// </summary>
  public record struct CompletionRecipe {
    public RecipeConditions conditions;
    public AtomType? saltElement;
    public AtomType? c1;
    public AtomType? c2;
    public AtomType? c3;
    public AtomType output;
    public AtomType saltOutput;

    /// <summary>
    /// Generates four completion recipes for a 'simple' (vanilla-like) completion recipe
    /// in which there are four cardinals, and placing three of them into Completion results in
    /// the missing fourth.
    /// </summary> 
    public static IEnumerable<CompletionRecipe> Simple(
     AtomType a1,
     AtomType a2,
     AtomType a3,
     AtomType a4,
     AtomType? maybeSalt = null,
     RecipeConditions? maybeConditions = null) {
      yield return new() {
        conditions = maybeConditions ?? new(),
        saltElement = maybeSalt ?? VA.salt,
        c1 = a1,
        c2 = a2,
        c3 = a3,
        output = a4,
        saltOutput = a4,
      };
      yield return new() {
        conditions = maybeConditions ?? new(),
        saltElement = maybeSalt ?? VA.salt,
        c1 = a1,
        c2 = a2,
        c3 = a4,
        output = a3,
        saltOutput = a3,
      };
      yield return new() {
        conditions = maybeConditions ?? new(),
        saltElement = maybeSalt ?? VA.salt,
        c1 = a1,
        c2 = a4,
        c3 = a3,
        output = a2,
        saltOutput = a2,
      };
      yield return new() {
        conditions = maybeConditions ?? new(),
        saltElement = maybeSalt ?? VA.salt,
        c1 = a4,
        c2 = a2,
        c3 = a3,
        output = a1,
        saltOutput = a1,
      };
    }
  }
  public record struct InversionRecipe {
    public RecipeConditions conditions;
    public AtomType cardinal;
    public AtomType invertsTo;
    public AtomType saltOutput;
  }
  public record struct RevolutionRecipe {
    public RecipeConditions conditions;
    public AtomType cardinal;
    public AtomType transmutesTo;
    public AtomType saltOutput;
  }
  public record struct DejectionRecipe {
    public RecipeConditions conditions;
    public AtomType cardinal;
    public AtomType transmutesTo;
    public AtomType ichorOutput;
    public static DejectionRecipe Default(AtomType cardinal, AtomType to) => new() {
      conditions = NoConditions(),
      cardinal = cardinal,
      transmutesTo = to,
      ichorOutput = ExtransmutationsMod.Ichor,
    };
    internal static DejectionRecipe Extraordinary(AtomType cardinal, AtomType to) => new() {
      conditions = ExtraordinaryConditions(),
      cardinal = cardinal,
      transmutesTo = to,
      ichorOutput = ExtransmutationsMod.Ichor,
    };
  }

  /// <summary> What restoration understands to count as a cardinal </summary>
  public record struct RestorationCardinal {
    public RecipeConditions conditions;
    public AtomType cardinal;
  }

}
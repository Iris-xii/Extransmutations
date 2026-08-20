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
  public static ICollection<Wheel> CompletionWheels {
    get => completionWheels;
    set => completionWheels = (List<Wheel>)value;
  }

  internal static List<CompletionRecipe> completionRecipes = new();
  public static ICollection<CompletionRecipe> CompletionRecipes {
    get => completionRecipes;
    set => completionRecipes = (List<CompletionRecipe>)value;
  }

  internal static List<InversionRecipe> inversionRecipes = new();
  public static ICollection<InversionRecipe> InversionRecipes {
    get => inversionRecipes;
    set => inversionRecipes = (List<InversionRecipe>)value;
  }

  internal static List<RevolutionRecipe> revolutionRecipes = new();
  public static ICollection<RevolutionRecipe> RevolutionRecipes {
    get => revolutionRecipes;
    set => revolutionRecipes = (List<RevolutionRecipe>)value;
  }

  internal static List<DejectionRecipe> dejectionRecipes = new();
  public static ICollection<DejectionRecipe> DejectionRecipes {
    get => dejectionRecipes;
    set => dejectionRecipes = (List<DejectionRecipe>)value;
  }

  internal static List<RestorationCardinal> restorationCardinals = new() { };
  public static ICollection<RestorationCardinal> RestorationCardinals {
    get => restorationCardinals;
    set => restorationCardinals = (List<RestorationCardinal>)value;
  }


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
  public record struct RecipeConditions {
    /// <summary>
    /// If present, Custom Permission required for the recipe.
    /// </summary>
    public string? requiredPerm;
    /// <summary>
    /// If present, this glyph must be somewhere in the solution for the recipe.
    /// </summary>
    public string? requiredGlyphName;
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
  /// </summary>
  public record struct CompletionRecipe {
    public RecipeConditions conditions;
    public AtomType? saltElement;
    public AtomType? c1;
    public AtomType? c2;
    public AtomType? c3;
    public AtomType output;
    public AtomType saltOutput;
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
  public record struct DejectionRecipe { // TODO
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
  public record struct RestorationCardinal {
    public RecipeConditions conditions;
    public AtomType cardinal;
  }

}
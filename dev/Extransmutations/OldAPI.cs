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
  /// <summary>
  /// Allow the Glyph of Completion to use this wheel in its recipes.
  /// </summary>
  public static void AddCompletionWheel(Wheel wheel) => completionWheels.Add(wheel);
  public static void AddCompletionRecipe(CompletionRecipe completionRecipe) => completionRecipes.Add(completionRecipe);
  public static void AddInversionRecipe(InversionRecipe inversionRecipe) => inversionRecipes.Add(inversionRecipe);
  public static void AddRevolutionRecipe(RevolutionRecipe revolutionRecipe) => revolutionRecipes.Add(revolutionRecipe);
  public static void AddDejectionRecipe(DejectionRecipe dejectionRecipe) => dejectionRecipes.Add(dejectionRecipe);
  public static void AddRestorationCardinal(AtomType cardinalLike) => restorationCardinals.Add(new() {conditions = NoConditions(),cardinal = cardinalLike});
  public static void AddRestorationCardinal(AtomType cardinalLike,RecipeConditions cond) => restorationCardinals.Add(new() {conditions = cond,cardinal = cardinalLike});
}
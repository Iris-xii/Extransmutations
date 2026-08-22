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
  // This part of the api still works but you might as well just use the properties directly

  /// <summary>
  /// Allow the Glyph of Completion to use this wheel in its recipes.
  /// </summary>
  [Obsolete] public static void AddCompletionWheel(Wheel wheel) => CompletionWheels.Add(wheel);
  [Obsolete] public static void AddCompletionRecipe(CompletionRecipe completionRecipe) => CompletionRecipes.Add(completionRecipe);
  [Obsolete] public static void AddInversionRecipe(InversionRecipe inversionRecipe) => InversionRecipes.Add(inversionRecipe);
  [Obsolete] public static void AddRevolutionRecipe(RevolutionRecipe revolutionRecipe) => RevolutionRecipes.Add(revolutionRecipe);
  [Obsolete] public static void AddDejectionRecipe(DejectionRecipe dejectionRecipe) => DejectionRecipes.Add(dejectionRecipe);
  public static void AddRestorationCardinal(AtomType cardinalLike) => RestorationCardinals.Add(new() {conditions = NoConditions(),cardinal = cardinalLike});
  public static void AddRestorationCardinal(AtomType cardinalLike,RecipeConditions cond) => RestorationCardinals.Add(new() {conditions = cond,cardinal = cardinalLike});
}
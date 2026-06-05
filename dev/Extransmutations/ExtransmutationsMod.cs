
using Quintessential;
using MonoMod.RuntimeDetour;
using MonoMod.Cil;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using BF = System.Reflection.BindingFlags;

//dotnet build;rm ..\..\Extransmutations.dll;cp .\bin\Debug\net4.5.2\Extransmutations.dll ..\..\Extransmutations.dll
public class ExtransmutationsMod : QuintessentialMod {

  public static AtomType Ichor;
  public static UncommonPrimesAtoms uncommonPrimesAtoms = new();

  public Hook hook_sim_method_1825;
  private ILHook ilhook_orig_method_1832;
  public Hook hook_GameLogic_method_946;

  public override void Load() {
    //hook_sim_method_1825 = new Hook(
    //  typeof(Sim).GetMethod("method_1825", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
    //  OnSimMethod1825); 
    //hook_GameLogic_method_946 = new Hook(
    //  typeof(GameLogic).GetMethod("method_946", BF.Public | BF.Instance),
    //  OnGameLogicMethod945
    //);
    //puzzleinfoscreen_method_1275 = new Hook(
    //  typeof(PuzzleInfoScreen).GetMethod("method_1275", BF.NonPublic | BF.Instance),
    //  OnSolLoad
    //);
  }


  //public Hook puzzleinfoscreen_method_1275;
  //internal static void OnSolLoad(
  //    Action<PuzzleInfoScreen, Solution> orig,
  //    PuzzleInfoScreen self,
  //    Solution param_5012) {
  //  
  //  Puzzle puzzle = param_5012.method_1934();
  //    puzzle.field_2769 = new(true, new class_215() {
  //      field_1899 = puzzle.field_2766,
  //      field_1900 = class_134.method_253("A", string.Empty),
  //      field_1901 = class_134.method_253("BBB EEE RRR QQQ", string.Empty),
  //      field_1902 = "speedbonder",
  //      field_1903 = new(),
  //      field_1904 = new(0, 0),
  //    });
  //  orig(self,param_5012);
  //}
  //internal delegate void origOnGameLogicMethod945(GameLogic self, IScreen param_4617);
  //internal static void OnGameLogicMethod945(
  //    origOnGameLogicMethod945 orig,
  //    GameLogic self,
  //    IScreen param_4617) {
  //  if (param_4617 is PuzzleInfoScreen) {}
  //  orig(self, param_4617);
  //}

  private static void ILMethod1832(ILContext il) {
    try {
      ILCursor c = new(il);
      c.GotoNext(MoveType.After,
        new Func<Mono.Cecil.Cil.Instruction, bool>[] {
        //x => x.MatchCallOrCallvirt(typeof(HexIndex).GetMethod("Rotated")),
        //x => x.Next != null && x.Next.MatchCallOrCallvirt(typeof(HexIndex).GetMethod("op_Addition")),
        x => x.MatchCallOrCallvirt(typeof(Sim).GetMethod("method_1848"))
        }
      );
      c.GotoNext(MoveType.After,
        new Func<Mono.Cecil.Cil.Instruction, bool>[] {
        x => x.MatchCallOrCallvirt(typeof(Maybe<Molecule>).GetMethod("method_1085")),
        }
      );
      c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg, 0);
      c.EmitDelegate(SupressOutputIfFalse);
      //c.EmitDelegate(HookTest);
      //c.Emit(Mono.Cecil.Cil.OpCodes.Nop);
      //Logger.Log($"{il}");
    }
    catch (Exception e) {
      Logger.Log($"{e}");
    }
  }
  private static bool SupressOutputIfFalse(bool mustBeTrue, Sim s) {
    if (!mustBeTrue) { return false; }
    List<HexIndex> okList = new();
    foreach (Part part in s.field_3818.method_502().field_3919) {
      var isInputOutput = part.method_1159().method_310();
      var isOutput = part.method_1159().method_309();
      if (!isInputOutput) { continue; }
      Molecule molecMaybe = part.method_1185(s.field_3818.method_502());
      var hexPos = part.method_1161();
      var partRotation = part.method_1163();
      foreach (var kv in molecMaybe.method_1100()) {
        if (kv.Value.field_2275 != Ichor) { continue; }
        okList.Add(kv.Key.Rotated(partRotation) + hexPos);
      }
    }
    // DEBUG
    //var seb = s.field_3818;
    //foreach (var okHex in okList) {
    //  seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(okHex), Brimstone.API.GetAnimation("textures/parts/calcification_glyph_flash.array", "calcify_glyph", 10), 2f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
    //}
    //
    foreach (Molecule m in s.field_3823) {
      foreach (var kv in m.method_1100()) {
        Atom atom = kv.Value;
        if (atom.field_2275 == Ichor) {
          HexIndex atomHex = kv.Key;
          if (!okList.Contains(atomHex)) {
            return false;
          }
        }
      }
    }
    return true;
  }

  private static bool OnSimMethod1825(On.Sim.orig_method_1825 orig, Sim s) {
    //var inputs = s.field_3818.method_502().method_1934().field_2770;
    List<HexIndex> okList = new();
    foreach (Part part in s.field_3818.method_502().field_3919) {
      var isInputOutput = part.method_1159().method_310();
      var isOutput = part.method_1159().method_309();
      if (!(!isOutput && isInputOutput)) { continue; }
      Molecule molecMaybe = part.method_1185(s.field_3818.method_502());
      var hexPos = part.method_1161();
      var partRotation = part.method_1163();
      foreach (var kv in molecMaybe.method_1100()) {
        if (kv.Value.field_2275 != Ichor) { continue; }
        okList.Add(kv.Key.Rotated(partRotation) + hexPos);
      }
    }
    // DEBUG
    var seb = s.field_3818;
    foreach (var okHex in okList) {
      seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(okHex), Brimstone.API.GetAnimation("textures/parts/calcification_glyph_flash.array", "calcify_glyph", 10), 2f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
    }
    //
    foreach (Molecule m in s.field_3823) {
      foreach (var kv in m.method_1100()) {
        Atom atom = kv.Value;
        if (atom.field_2275 == Ichor) {
          HexIndex atomHex = kv.Key;
          if (!okList.Contains(atomHex)) { return false; }
        }
      }
    }
    return orig(s);
  }


  public override void PostLoad() {
    ilhook_orig_method_1832 = new ILHook(
      typeof(Sim).GetMethod("orig_method_1832", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic),
      ILMethod1832
    );
    if (Brimstone.API.IsModLoaded("Extransmissions")
    && Brimstone.API.GetMod("Extransmissions").method_99(out var QM)
    && QM is Extransmissions.ExtransmissionsMod EM) {
      EM.shouldSuppressOutputs.Add((sim) => !SupressOutputIfFalse(true, sim));
    }
    if (Brimstone.API.IsModLoaded("UncommonPrimes")) {
      try {
        uncommonPrimesAtoms = new UncommonPrimesAtoms() {
          bellum = QApi.ModAtomTypes.Single(at => at.QuintAtomType == "UncommonPrimes:bellum"),
          obscurum = QApi.ModAtomTypes.Single(at => at.QuintAtomType == "UncommonPrimes:obscurum"),
          lux = QApi.ModAtomTypes.Single(at => at.QuintAtomType == "UncommonPrimes:lux"),
          pax = QApi.ModAtomTypes.Single(at => at.QuintAtomType == "UncommonPrimes:pax"),
        };
      }
      catch (InvalidOperationException) { }
    }


    API.AddCompletionWheel(new() {
      wheelName = "uncommon-primes-servin",
      wheelMolecule = GlyphCompletion.ServinMolec()
    });
    API.AddCompletionWheel(new() {
      wheelName = "baron",
      wheelMolecule = null
    });
    DefaultRecipes();
  }

  private void DefaultRecipes() {
    GlyphCompletion.DefaultRecipes();
    GlyphInversion.DefaultRecipes();
    GlyphRevolution.DefaultRecipes();
    GlyphDejection.DefaultRecipes();
    GlyphRestoration.DefaultCardinals();
  }




  public override void LoadPuzzleContent() {
    Ichor = Brimstone.API.CreateNormalAtom(81, "Extransmutations", "Ichor",
      pathToSymbol: "textures/atoms/ichor",
      pathToDiffuse: "textures/atoms/ichor_diffuse",
      pathToShade: "textures/atoms/ichor_shade");
    QApi.AddAtomType(Ichor);

    Textures textures = new();
    var glyphRevolution = GlyphRevolution.LoadPuzzleContent(textures);
    var cardinalInversion = GlyphInversion.LoadPuzzleContent(textures);
    var cardinalCompletion = GlyphCompletion.LoadPuzzleContent(textures);
    var glyphDejection = GlyphDejection.LoadPuzzleContent(textures);
    var glyphRecombination = GlyphRecombination.LoadPuzzleContent(textures);
    var glyphAeration = GlyphAeration.LoadPuzzleContent(textures);
    var glyphLiquidation = GlyphLiquidation.LoadPuzzleContent(textures);
    var glyphCompaction = GlyphCompaction.LoadPuzzleContent(textures);
    var glyphInduction = GlyphInduction.LoadPuzzleContent(textures);
    var glyphExtraordinary = GlyphExtraordinary.LoadPuzzleContent(textures);
    var glyphRestoration = GlyphRestoration.LoadPuzzleContent(textures);

    HashSet<HexIndex> inductionSaltSpots = new();
    Dictionary<HexIndex, int> inductionHooksCount = new();
    Dictionary<Atom, AtomType> previousTypeOfAtom = new();
    Dictionary<Atom, AtomType> currentTypeOfAtom = new();
    bool inductionExists = false;
    QApi.RunAfterCycle((sim, first) => {
      SolutionEditorBase seb = sim.field_3818;
      var solution = seb.method_502();

      var partList = solution.field_3919;
      int cycle = sim.method_1818();

      if (cycle == 0) { // induction hack
        inductionExists = partList.Any((a) => { return a.method_1159() == glyphInduction; });
        Dictionary<Part, int> originalPosition = new();
        for (int i = 0; i < partList.Count(); i++) {
          originalPosition.Add(partList[i], i);
        }
        partList.Sort((a, b) => {
          var aType = a.method_1159();
          var bType = b.method_1159();
          int tieBreaker = 0;
          var aIdx = originalPosition[a];
          var bIdx = originalPosition[b];
          tieBreaker = aIdx.CompareTo(bIdx);
          int inductionOrder = 0;
          if (aType == glyphInduction && bType != glyphInduction) inductionOrder = 1;
          if (aType != glyphInduction && bType == glyphInduction) inductionOrder = -1;
          if (inductionOrder != 0) {
            return inductionOrder;
          }
          else {
            return tieBreaker;
          }
        });
      }
      if (inductionExists) {
        foreach (Molecule molecule in sim.field_3823) {
          foreach (var kv in molecule.method_1100()) {
            Atom a = kv.Value;
            currentTypeOfAtom[a] = a.field_2275;
          }
        }
      }
      //var partSimStates = sim.field_3821;
      //var struct122List = sim.field_3826;
      //var moleculeList = sim.field_3823;
      //var gripperList = sim.HeldGrippers;
      inductionSaltSpots.Clear();
      inductionHooksCount.Clear(); 
      foreach (Part part in partList) {
        var partType = part.method_1159();
        if (partType == glyphInduction) { inductionSaltSpots.Add(GlyphInduction.GetInductionSaltHex(part)); }
        if (partType == glyphInduction) {
          HexIndex hookSpot = GlyphInduction.GetHookHex(part);
          int value = 0;
          inductionHooksCount.TryGetValue(hookSpot, out value);
          value += 1;
          inductionHooksCount[hookSpot] = value;
        } 
      }
      foreach (Part part in partList) {
        var partType = part.method_1159();
        if (partType == glyphRevolution /*&& first*/) { GlyphRevolution.Activate(sim, seb, part, textures); }
        if (partType == cardinalInversion) { GlyphInversion.Activate(sim, seb, part, textures); }
        if (partType == cardinalCompletion) { GlyphCompletion.Activate(sim, seb, part, textures); }
        if (partType == glyphRestoration) { GlyphRestoration.Activate(sim, seb, part, textures); }
        if (partType == glyphDejection) { GlyphDejection.Activate(first, sim, seb, part, textures); }
        if (partType == glyphRecombination) { GlyphRecombination.Activate(sim, seb, part, textures); }
        if (partType == glyphAeration) { GlyphAeration.Activate(first, sim, seb, part, textures); }
        if (partType == glyphLiquidation) { GlyphLiquidation.Activate(sim, seb, part, textures); }
        if (partType == glyphCompaction) { GlyphCompaction.Activate(sim, seb, part, textures); }
        if (partType == glyphInduction) { GlyphInduction.Activate(previousTypeOfAtom, inductionHooksCount, inductionSaltSpots, sim, seb, part, textures); }
      }

      previousTypeOfAtom = currentTypeOfAtom;
      currentTypeOfAtom = new();
    });
  }
  public override void Unload() {
    hook_sim_method_1825 = null;
    ilhook_orig_method_1832 = null;
  }
  internal static void Log(string s) => Logger.Log($"[extransmutations] {s}");


  // BERLO - Ty greenfield 
  internal static Maybe<AtomReference> FindBerloAtom(Sim sim_self, Part part, HexIndex offset, string wheelName = "baron", Molecule wheelMol = null) => FindBerloAtom(sim_self, part.method_1184(offset), wheelName, wheelMol);
  internal static Maybe<AtomReference> FindBerloAtom(Sim sim_self, HexIndex hex,
      string wheelName = "baron", Molecule wheelMol = null) {
    var SEB = sim_self.field_3818;
    var solution = SEB.method_502();
    var partList = solution.field_3919;
    var partSimStates = sim_self.field_3821;

    foreach (var berlo in partList.Where(x => x.method_1159().field_1528 == wheelName)) {
      var partSimState = partSimStates[berlo];
      Molecule wheelAtoms;//berlo.field_1544
      if (wheelMol is null) {
        wheelAtoms = new();
        foreach (var kv in berlo.method_1159().field_1544) {
          wheelAtoms.method_1105(new(kv.Value), kv.Key);
        }
      }
      else {
        wheelAtoms = wheelMol;
      }
      var hexIndex = partSimState.field_2724;
      var rotation = partSimState.field_2726;
      var hexKey = (hex - hexIndex).Rotated(rotation.Negative());
      //SEB.field_3935.Add(new class_228(SEB, (enum_7)1, 
      //class_187.field_1742.method_492(berlo.method_1184(hexKey)), Brimstone.API.GetAnimation("textures/parts/calcification_glyph_flash.array", "calcify_glyph", 10), 2f, Vector2.Zero, /*part.method_1163().ToRadians()*/ 0f));
      if (wheelAtoms.method_1100().TryGetValue(hexKey, out Atom atom)) {
        return new AtomReference(wheelAtoms, hexKey, atom.field_2275, atom, true);
      }
    }
    return struct_18.field_1431;
  }
  // berlo end 
#nullable enable 
  public struct UncommonPrimesAtoms {
    public AtomType? bellum = null;
    public AtomType? obscurum = null;
    public AtomType? lux = null;
    public AtomType? pax = null;
    public UncommonPrimesAtoms() { }
  }
#nullable disable
}

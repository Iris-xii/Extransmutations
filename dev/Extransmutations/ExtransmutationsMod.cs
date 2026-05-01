
using Quintessential;
using MonoMod.RuntimeDetour;
using MonoMod.Cil;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;

public class ExtransmutationsMod : QuintessentialMod {

  public static AtomType IchorMortuum;

  public Hook hook_sim_method_1825;
  private ILHook ilhook_orig_method_1832;

  public override void Load() {
    //hook_sim_method_1825 = new Hook(
    //  typeof(Sim).GetMethod("method_1825", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
    //  OnSimMethod1825); 
  }
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
        if (kv.Value.field_2275 != IchorMortuum) { continue; }
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
        if (atom.field_2275 == IchorMortuum) {
          HexIndex atomHex = kv.Key;
          if (!okList.Contains(atomHex)) { return false; }
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
        if (kv.Value.field_2275 != IchorMortuum) { continue; }
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
        if (atom.field_2275 == IchorMortuum) {
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
  }
  public override void LoadPuzzleContent() {
    IchorMortuum = Brimstone.API.CreateNormalAtom(81, "Extransmutations", "Ichor Mortuum",
      pathToSymbol: "textures/atoms/ichor_mortuum",
      pathToDiffuse: "textures/atoms/ichor_mortuum_diffuse",
      pathToShade: "textures/atoms/ichor_mortuum_shade");
    QApi.AddAtomType(IchorMortuum);

    Textures textures = new();
    var glyphRevolution = GlyphRevolution.LoadPuzzleContent(textures);
    var cardinalInversion = GlyphInversion.LoadPuzzleContent(textures);
    var cardinalCompletion = GlyphCompletion.LoadPuzzleContent(textures);
    var glyphDejection = GlyphDejection.LoadPuzzleContent(textures);

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
        if (partType == glyphRevolution /*&& first*/) { GlyphRevolution.Activate(sim, seb, part, textures); }
        if (partType == cardinalInversion) { GlyphInversion.Activate(sim, seb, part, textures); }
        if (partType == cardinalCompletion) { GlyphCompletion.Activate(sim, seb, part, textures); }
        if (partType == glyphDejection) { GlyphDejection.Activate(first,sim, seb, part, textures); }
      }
    });
  }
  public override void Unload() {
    hook_sim_method_1825 = null;
    ilhook_orig_method_1832 = null;
  }
}

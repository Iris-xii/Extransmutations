using MonoMod.Utils;
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;
using VanillaAtoms = Brimstone.API.VanillaAtoms;

public static class GlyphInduction {

  public static PartType LoadPuzzleContent(Textures t) {
    QApi.AddPuzzlePermission("extransmutations-induction",
 "Glyph of Induction",
 "Extransmutations");
    PartType glyphInduction = new() {
      field_1528 = "extransmutations-induction", // ID
      field_1529 = class_134.method_253("Glyph of Induction", string.Empty), // Name
      field_1530 = class_134.method_253("The Glyph of Induction can attach its hollow hex to another glyph. Whenever a transmutation takes place in the attached hex, the glyph of induction will transmute an atom of salt into fire. It is not permitted to attach multiple induction hooks to the same hex, or to attach it to another glyph of induction.", string.Empty), // Description
      field_1531 = 10, // Cost
      field_1539 = true, // Is a glyph (?)
      field_1549 = t.inductionGlow, // Shadow/glow
      field_1550 = t.inductionStroke, // Stroke/outline
      field_1547 = t.inductionPanel, // Panel icon
      field_1548 = t.inductionPanel, // Hovered panel icon
      field_1540 = new HexIndex[]{
                new(0, 0),
                new(-1, 0),
            }, // Spaces used
      field_1551 = Permissions.None,
      CustomPermissionCheck = perms => perms.Contains("extransmutations-induction"),
    };
    QApi.AddPartType(glyphInduction, (part, pos, editor, renderer) => {
      Vector2 centre = t.inductionBase.method_691();
      var time = editor.method_504();
      class_236 uco = editor.method_1989(part, pos);
      PartSimState pss = editor.method_507().method_481(part);
      renderer.method_523(t.inductionBase, new Vector2(-1, -1), centre, 0f);
      renderer.method_528(t.bowlTexture, new(0, 0), Vector2.Zero);
      renderer.method_529(t.fireGlyph, new(0, 0), Vector2.Zero);
      renderer.method_529(t.markerLighting, new(-1, 0), Vector2.Zero);
      renderer.method_529(t.markerDetails, new(-1, 0), Vector2.Zero);
    });
    QApi.AddPartTypeToPanel(glyphInduction, false);
    return glyphInduction;
  }


  public static HexIndex GetInductionSaltHex(Part part) {
    return part.method_1161() + new HexIndex(0, 0);
  }
  public static HexIndex GetHookHex(Part part) {
    return part.method_1161() + new HexIndex(1, 0).Rotated(part.method_1163());
  }

  public static void Activate(
    Dictionary<Atom, AtomType> previousTypeOfAtom, 
    Dictionary<HexIndex, int> inductionHooksCount,
    HashSet<HexIndex> inductionSaltSpots,
    Sim sim,
    SolutionEditorBase seb,
    Part part,
    Textures t) {
    Dictionary<Part, PartSimState> partSimStates = sim.field_3821;
    PartSimState pss = partSimStates[part];

    //DynamicData dyn_pss = new(pss);
    //object stateOb = dyn_pss.Get("state");
    //InductionState state;
    //if (stateOb is not null) state = (InductionState)stateOb;
    //else state = new(); 

    if (sim.FindAtomRelative(part, new(0, 0)).method_99(out AtomReference salt) &&
    sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference hookedCur)) {
      //foreach (var item in inductionSaltSpots) {
      //  Logger.Log($"{item} >>> {part.method_1161()}");
      //}
      int hooksShared = 0;
      inductionHooksCount.TryGetValue(GetHookHex(part), out hooksShared);
      bool doTransmute =
        salt.field_2280 == VanillaAtoms.salt
        && previousTypeOfAtom.ContainsKey(hookedCur.field_2279)
        && previousTypeOfAtom[hookedCur.field_2279] is not null
        && (!inductionSaltSpots.Contains(GetHookHex(part)))
        && (hooksShared <= 1)
        && previousTypeOfAtom[hookedCur.field_2279] != hookedCur.field_2280;
      //if(state.previousAR is not null && state.previousType is not null) {
      //  Logger.Log($"[extransmutations] "+
      //  $"{(!inductionSaltSpots.Contains(GetHookHex(part)))}"
      //  +$"{(hooksShared <= 1)}"
      //  +$"{state.previousAR.field_2279 == hookedCur.field_2279}"
      //  +$"{state.previousType != hookedCur.field_2280}"
      //  );
      //}
      if (doTransmute) {
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 0))), t.bowlGlow, 30f, Vector2.Zero, 0f));
        Brimstone.API.ChangeAtom(salt, VanillaAtoms.fire);
        salt.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, salt.field_2280, class_238.field_1989.field_81.field_614, 60f); //30f
      }
    }
    //state.previousAR = hookedCur ?? null;
    //state.previousType = hookedCur?.field_2280;
    //dyn_pss.Set("state", state);
  }

  struct InductionState {
    public AtomReference previousAR = null;
    public AtomType previousType = null;
    public InductionState() { }
  }

}
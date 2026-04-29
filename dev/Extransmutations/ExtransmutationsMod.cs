
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;

public class ExtransmutationsMod : QuintessentialMod {
  public override void Load() { }
  public override void PostLoad() { }
  public override void LoadPuzzleContent() {
    Textures textures = new();
    var glyphRevolution = GlyphRevolution.LoadPuzzleContent(textures);
    var cardinalInversion = GlyphInversion.LoadPuzzleContent(textures);
    var cardinalCompletion = GlyphCompletion.LoadPuzzleContent(textures);

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
      }
    });
  }
  public override void Unload() { }
}

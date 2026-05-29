
using Quintessential;

namespace Extransmutations;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;

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
      renderer.method_528(t.bowlProjTexture, new HexIndex(1, 0), Vector2.Zero);
      renderer.method_528(t.bowlProjTexture, new HexIndex(0, 0), Vector2.Zero);
      renderer.method_528(t.bowlProjTexture, new HexIndex(1, -1), Vector2.Zero);
      renderer.method_528(t.bowlProjTexture, new HexIndex(0, 1), Vector2.Zero);

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

  // Ty greenfield 
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

  internal static void BerloReplaceCardinals(
      byte version,
      Sim sim,
      Part part,
      in AtomType[] cardinalsTypes,
      byte[] cardinalsCount,
      string wheelName = "baron",
      Molecule wheelMol = null) {
    if (version <= 0) { return; }
    foreach (var offset in CARDINAL_POSITIONS) {
      var maybeAR = FindBerloAtom(sim, part, offset, wheelName, wheelMol);
      if (maybeAR.method_99(out var AR)) {
        var kind = AR.field_2280;
        for (int i = 0; i < cardinalsCount.Length; i++) {
          if (kind == cardinalsTypes[i]) {
            cardinalsCount[i] += 1;
          }
        }
      }
    }
  }

  private static Molecule ServinMolec() {
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
  public static void Activate(Sim sim, SolutionEditorBase seb, Part part, Textures t, bool doExtraordinary) {
    var solution = seb.method_502();
    var puzzle = solution.method_1934();
    var version = GetVersion(puzzle.CustomPermissions);

    sim.FindAtomRelative(part, new HexIndex(1, 0)).method_99(out AtomReference c1);
    sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference c2);
    sim.FindAtomRelative(part, new HexIndex(1, -1)).method_99(out AtomReference c3);
    bool trueSalt = sim.FindAtomRelative(part, new HexIndex(0, 1)).method_99(out AtomReference salt);
    trueSalt = trueSalt && salt.field_2280 == Brimstone.API.VanillaAtoms.salt;
    bool hasSalt = trueSalt
      || (version > 0 && FindBerloAtom(sim, part, SALT_POSITION).method_1085() && FindBerloAtom(sim, part, SALT_POSITION).method_1087().field_2280 == Brimstone.API.VanillaAtoms.salt)
      || (version > 0 && doExtraordinary && FindBerloAtom(sim, part, SALT_POSITION,"uncommon-primes-servin", ServinMolec()).method_1085() && FindBerloAtom(sim, part, SALT_POSITION,"uncommon-primes-servin", ServinMolec()).method_1087().field_2280 == Brimstone.API.VanillaAtoms.salt);
    { // NORMAL
      AtomReference[] cardinalRefs = { c1, c2, c3 };
      AtomReference[] allAtomRefs = { c1, c2, c3, salt };
      AtomType[] cardinals_types = {
          Brimstone.API.VanillaAtoms.water,
          Brimstone.API.VanillaAtoms.fire,
          Brimstone.API.VanillaAtoms.air,
          Brimstone.API.VanillaAtoms.earth,
      };
      byte[] cardinalsCount = { 0, 0, 0, 0 };
      for (int i = 0; i < 4; i++) {
        foreach (AtomReference ar in cardinalRefs) {
          if (ar is null) continue;
          if (ar.field_2280 == cardinals_types[i]) {
            cardinalsCount[i] += 1;
          }
        }
      }
      BerloReplaceCardinals(version, sim, part, cardinals_types, cardinalsCount);
      AtomType maybeTarget = Brimstone.API.VanillaAtoms.salt; //this crashes if null, for some reason, so dummy salt it is
      if (cardinalsCount.SequenceEqual(new byte[] { 1, 1, 1, 0 })) {
        maybeTarget = Brimstone.API.VanillaAtoms.earth;
      }
      if (cardinalsCount.SequenceEqual(new byte[] { 0, 1, 1, 1 })) {
        maybeTarget = Brimstone.API.VanillaAtoms.water;
      }
      if (cardinalsCount.SequenceEqual(new byte[] { 1, 0, 1, 1 })) {
        maybeTarget = Brimstone.API.VanillaAtoms.fire;
      }
      if (cardinalsCount.SequenceEqual(new byte[] { 1, 1, 0, 1 })) {
        maybeTarget = Brimstone.API.VanillaAtoms.air;
      }
      if (hasSalt && (maybeTarget != Brimstone.API.VanillaAtoms.salt)) { //transmute!
        if (!trueSalt) {
          allAtomRefs = new AtomReference[] { c1, c2, c3 };
        }
        foreach (AtomReference ar in allAtomRefs) {
          if (ar is null) continue;
          ar.field_2277.method_1106(maybeTarget, ar.field_2278);
          ar.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, ar.field_2280, class_238.field_1989.field_81.field_614, 60f);
        }
        //seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), t.calcifyAnimation, 30f, Vector2.Zero, 0f));
        //seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 0))), t.calcifyAnimation, 30f, Vector2.Zero, 0f));
        //seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, -1))), t.calcifyAnimation, 30f, Vector2.Zero, 0f));
        class_238.field_1991.field_1844.method_28(seb.method_506());
      }
    }
    if (doExtraordinary) { // ORDINALS
      AtomReference[] ordinalRefs = { c1, c2, c3 };
      AtomReference[] allAtomRefs = { c1, c2, c3, salt };
      AtomType[] ordinals_types = {
          ExtransmutationsMod.uncommonPrimesAtoms.bellum,
          ExtransmutationsMod.uncommonPrimesAtoms.pax,
          ExtransmutationsMod.uncommonPrimesAtoms.lux,
          ExtransmutationsMod.uncommonPrimesAtoms.obscurum,
      };
      byte[] ordinalsCount = { 0, 0, 0, 0 };
      for (int i = 0; i < 4; i++) {
        foreach (AtomReference ar in ordinalRefs) {
          if (ar is null) continue;
          if (ar.field_2280 == ordinals_types[i]) {
            ordinalsCount[i] += 1;
          }
        }
      }
      BerloReplaceCardinals(version, sim, part, ordinals_types, ordinalsCount, "uncommon-primes-servin", ServinMolec());
      AtomType maybeTarget = Brimstone.API.VanillaAtoms.salt; //this crashes if null, for some reason, so dummy salt it is
      if (ordinalsCount.SequenceEqual(new byte[] { 1, 1, 1, 0 })) {
        maybeTarget = ExtransmutationsMod.uncommonPrimesAtoms.obscurum;
      }
      if (ordinalsCount.SequenceEqual(new byte[] { 0, 1, 1, 1 })) {
        maybeTarget = ExtransmutationsMod.uncommonPrimesAtoms.bellum;
      }
      if (ordinalsCount.SequenceEqual(new byte[] { 1, 0, 1, 1 })) {
        maybeTarget = ExtransmutationsMod.uncommonPrimesAtoms.pax;
      }
      if (ordinalsCount.SequenceEqual(new byte[] { 1, 1, 0, 1 })) {
        maybeTarget = ExtransmutationsMod.uncommonPrimesAtoms.lux;
      }
      if (hasSalt && (maybeTarget != Brimstone.API.VanillaAtoms.salt)) { //transmute!
        if (!trueSalt) {
          allAtomRefs = new AtomReference[] { c1, c2, c3 };
        }
        foreach (AtomReference ar in allAtomRefs) {
          if (ar is null) continue;
          ar.field_2277.method_1106(maybeTarget, ar.field_2278);
          ar.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, ar.field_2280, class_238.field_1989.field_81.field_614, 60f);
        }
        //seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, 0))), t.calcifyAnimation, 30f, Vector2.Zero, 0f));
        //seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(0, 0))), t.calcifyAnimation, 30f, Vector2.Zero, 0f));
        //seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new HexIndex(1, -1))), t.calcifyAnimation, 30f, Vector2.Zero, 0f));
        class_238.field_1991.field_1844.method_28(seb.method_506());
      }
    }

  }
}
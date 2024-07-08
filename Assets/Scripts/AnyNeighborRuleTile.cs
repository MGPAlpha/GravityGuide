using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName="Any Neighbor Rule Tile", menuName="2D/Tiles/Any Neighbor Rule Tile")]
public class AnyNeighborRuleTile : RuleTile<AnyNeighborRuleTile.Neighbor> {
	public class Neighbor : RuleTile.TilingRule.Neighbor {
		public const int NotNull = 3;
		public const int Null = 4;
	}
	public override bool RuleMatch(int neighbor, TileBase tile) {
		switch (neighbor) {
			case Neighbor.NotNull: return tile != null;
			case Neighbor.Null: return tile == null;
		}
		return base.RuleMatch(neighbor, tile);
	}
}
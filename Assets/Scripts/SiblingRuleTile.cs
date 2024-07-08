using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName="Sibling Rule Tile", menuName="2D/Tiles/Sibling Rule Tile")]
public class SiblingRuleTile : RuleTile<SiblingRuleTile.Neighbor> {
    public int siblingGroup;
	public class Neighbor : RuleTile.TilingRule.Neighbor {
		public const int Sibling = 3;
		public const int NotSibling = 4;
	}
	public override bool RuleMatch(int neighbor, TileBase tile) {
		SiblingRuleTile myTile = tile as SiblingRuleTile;
		if (!myTile) {
			RuleOverrideTile overrideTile = tile as RuleOverrideTile;
			if (overrideTile) {
				myTile = overrideTile.m_Tile as SiblingRuleTile;
			}
		}
		switch (neighbor) {
			case Neighbor.Sibling: return myTile && myTile.siblingGroup == siblingGroup;
			case Neighbor.NotSibling: return !myTile || myTile.siblingGroup != siblingGroup;
		}
		return base.RuleMatch(neighbor, tile);
	}
}
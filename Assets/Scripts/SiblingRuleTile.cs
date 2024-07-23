using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName="New Sibling Rule Tile", menuName="2D/Tiles/Sibling Rule Tile")]
public class SiblingRuleTile : RuleTile<SiblingRuleTile.Neighbor> {
    public List<TileBase> siblings;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Sibling = 3;
        public const int NotSibling = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Sibling: return tile == this || siblings.Contains(tile);
            case Neighbor.NotSibling: return tile != this && !siblings.Contains(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }
}
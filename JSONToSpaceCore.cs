using SpaceCore;
using StardewValley.Tools;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffadax
{
    public class JSONToSpaceCore : CustomForgeRecipe
    {
        private class GenericIngredientMatcher(string qualId, int qty) : CustomForgeRecipe.IngredientMatcher
        {
            public string QualifiedId { get; } = qualId;
            public int Quantity { get; } = qty;

            public override bool HasEnoughFor(Item item)
            {
                return item.QualifiedItemId == QualifiedId && item.Stack >= Quantity;
            }

            public override void Consume(ref Item item)
            {
                if (Quantity >= item.Stack)
                    item = null;
                else
                    item.Stack -= Quantity;
            }
        }

        private class WeaponIngredientMatcher(string wqualID) : CustomForgeRecipe.IngredientMatcher
        {
            public string WeaponQualifiedId { get; } = wqualID;
            public override bool HasEnoughFor(Item item)
            {
                if (item is MeleeWeapon)
                {
                    return item.QualifiedItemId == WeaponQualifiedId;
                }
                return false;
            }

            public override void Consume(ref Item item)
            {
                item = null;
            }
        }

        public override IngredientMatcher BaseItem { get; } = new WeaponIngredientMatcher("(W)26");
        public override IngredientMatcher IngredientItem { get; } = new GenericIngredientMatcher("(O)Raffadax.RCP_WineofIshtar", 1);
        public override int CinderShardCost { get; } = 30;
        public override Item CreateResult(Item baseItem, Item ingredItem)
        {
            var w = baseItem.getOne() as MeleeWeapon;
            string newItemId = "Raffadax.RCP_Airgetlam";
            if (newItemId != null)
            {
                w.transform(newItemId);
            }
            return w;
        }
    }
}

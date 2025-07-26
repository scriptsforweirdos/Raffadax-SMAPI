using SpaceCore;
using StardewValley.Tools;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using Microsoft.Xna.Framework.Input;

namespace Raffadax
{
    public class JSONToSpaceCore : CustomForgeRecipe
    {
        public static string WeaponQid { get; set; }
        public static string IngredientQid { get; set; }
        public static int ShardCount { get; set; }
        public static string ResultId { get; set; }
        public static IMonitor ModMonitor;
        
        /// <summary>Constructor class</summary>
        /// <param name="weaponQid">Qualified Item ID of the Weapon ingredient.</param>
        /// <param name="ingredientQid">Qualified Item ID of the Object ingredient.</param>
        /// <param name="shardCount">Cinder Shards required</param>
        /// <param name="modMonitor">Monitor passed from Mod Entry via RecipeLoader.</param>
        public JSONToSpaceCore(string weaponQid, string ingredientQid, int shardCount, IMonitor modMonitor)
        {
            WeaponQid = weaponQid;
            IngredientQid = ingredientQid;
            ShardCount = shardCount;
            ModMonitor = modMonitor;
            //ModMonitor.Log($"Received data {ResultId}: {WeaponQid}, {IngredientQid}", LogLevel.Trace);
        }
        
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

        public override IngredientMatcher BaseItem { get; } = new WeaponIngredientMatcher(WeaponQid);
        public override IngredientMatcher IngredientItem { get; } = new GenericIngredientMatcher(IngredientQid, 1);
        public override int CinderShardCost { get; } = ShardCount;
        public override Item CreateResult(Item baseItem, Item ingredItem)
        {

            var w = baseItem.getOne() as MeleeWeapon;
        
            // Result ID must be looked up via the reversed dictionary we created in RecipeLoader,
            // otherwise it will always return the final result in the data model.
            string newItemId = RecipeLoader.reversed[baseItem.QualifiedItemId + '|' + ingredItem.QualifiedItemId];
            //ModMonitor.Log($"Weapon: {baseItem.QualifiedItemId}, Ingredient: {ingredItem.QualifiedItemId}, newItemId: {RecipeLoader.reversed[baseItem.QualifiedItemId + '|' + ingredItem.QualifiedItemId]}", LogLevel.Trace);
            if (newItemId != null)
            {
                w.transform(newItemId);
            }
            return w;
        }
    }
}

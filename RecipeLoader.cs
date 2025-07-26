using SpaceCore;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffadax
{
    /// <summary>Imports Forge recipe data from data/forgerecipes.json</summary>
    internal class RecipeLoader
    {
        public static IModHelper Helper;
        public static IMonitor ModMonitor;
        internal Dictionary<string, RecipeEntry> lookup;
        // reversed lookup required for finding the resulting IDs given string concat of weapon and object ingredients.
        public static Dictionary<string, string> reversed;

        internal RecipeLoader()
        {
            Helper = ModEntry.Helper;
            ModMonitor = ModEntry.ModMonitor;
            // set up empty dictionaries
            lookup = new Dictionary<string, RecipeEntry>();
            reversed = new Dictionary<string, string>();
            // Populate lookup
            lookup = Helper.Data.ReadJsonFile<Dictionary<string, RecipeEntry>>("data/forgerecipes.json");
            // Populate reversed
            foreach(var key in lookup.Keys)
            {
                string revKey = lookup[key].Weapon + "|" + lookup[key].Ingredient;
                reversed.Add(revKey, key);
            }
            // Add to Forge recipes
            foreach (var key in lookup.Keys)
            {
                CustomForgeRecipe.Recipes.Add(new JSONToSpaceCore(lookup[key].Weapon, lookup[key].Ingredient, lookup[key].CinderShards, ModMonitor));
            }
        }
    }

    class RecipeEntry
    {
        public string? Weapon { get; set; }
        public string? Ingredient { get; set; }
        public int CinderShards { get; set; }

        public RecipeEntry()
        {
            this.Weapon = "(W)0";
            this.Ingredient = "(O)0";
            this.CinderShards = 30;
        }

    }
}

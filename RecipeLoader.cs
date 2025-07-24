using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffadax
{
    internal class RecipeLoader
    {
        public static IModHelper Helper;
        public static IMonitor ModMonitor;
        internal Dictionary<string, RecipeEntry> lookup;

        internal RecipeLoader()
        {
            Helper = ModEntry.Helper;
            ModMonitor = ModEntry.ModMonitor;
            lookup = new Dictionary<string, RecipeEntry>();
            lookup = Helper.Data.ReadJsonFile<Dictionary<string, RecipeEntry>>("data/forgerecipes.json");
            foreach(var key in lookup.Keys)
            {
                ModMonitor.Log($"{key}: {lookup[key].Weapon}, {lookup[key].Ingredient}", LogLevel.Info);
            }
        }
    }

    class RecipeEntry
    {
        public string? Weapon { get; set; }
        public string? Ingredient { get; set; }
        public int CinderShards { get; set; }
        public string? Result { get; set; }

        public RecipeEntry()
        {
            this.Weapon = "(W)0";
            this.Ingredient = "(O)0";
            this.CinderShards = 30;
            this.Result = "0";
        }

    }
}

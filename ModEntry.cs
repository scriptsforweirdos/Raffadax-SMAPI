using System;
using Microsoft.Xna.Framework;
using Raffadax.APIs;
using SpaceCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Enchantments;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Tools;

/// <summary>The mod entry point.</summary>
namespace Raffadax
{
    internal sealed class ModEntry : Mod
    {
        /********
        ** Public Methods
        ********/

        internal static RecipeLoader RecipeLoader { get; set; }
        internal static IMonitor ModMonitor { get; set; }
        internal static IModHelper Helper { get; set; }

        ///<summary>Mod Entry point, called after mod is first loaded.</summary>
        ///<param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModMonitor = Monitor;
            Helper = helper;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            if (!Helper.ModRegistry.IsLoaded("spacechase0.SpaceCore"))
            {
                Monitor.Log("SpaceCore not loaded, Raffadax SMAPI will not load.", LogLevel.Error);
                return;
            }
            //CustomForgeRecipe.Recipes.Add(new JSONToSpaceCore());
        }

        /********
        ** Private Methods
        ********/
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            RecipeLoader = new RecipeLoader();
        }

    }
}
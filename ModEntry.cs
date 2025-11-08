using System;
using Microsoft.Xna.Framework;
using SpaceCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Enchantments;
using StardewValley.ItemTypeDefinitions;
using StardewValley.TerrainFeatures;
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
            helper.Events.GameLoop.SaveCreated += OnSaveCreated;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            if (!Helper.ModRegistry.IsLoaded("spacechase0.SpaceCore"))
            {
                Monitor.Log("SpaceCore not loaded, Raffadax SMAPI will not load.", LogLevel.Error);
                return;
            }
        }

        /********
        ** Private Methods
        ********/
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            RecipeLoader = new RecipeLoader();
        }

        private void OnSaveCreated(object sender, EventArgs e)
        {
            Game1.getFarm().resourceClumps.OnValueRemoved += OnClumpRemoved;
        }

        private void OnSaveLoaded(object sender, EventArgs e)
        {
            Game1.getFarm().resourceClumps.OnValueRemoved += OnClumpRemoved;
        }

        private void OnClumpRemoved(ResourceClump value)
        {
            Monitor.Log($"Removed Clump index {value.parentSheetIndex.Value}", LogLevel.Info);
            if (value.parentSheetIndex.Value == ResourceClump.meteoriteIndex)
            {
                Monitor.Log($"This is a meteorite.", LogLevel.Info);
                if (Game1.random.NextDouble() <= 0.5)
                {
                    Game1.createMultipleObjectDebris("(O)20", (int)value.Tile.X, (int)value.Tile.Y, 10);
                }
            }

        }
    }
}
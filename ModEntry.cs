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

            GameLocation.RegisterTileAction("RaffadaxLadderWarp", LadderWarp);
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

        internal static bool LadderWarp(GameLocation location, string[] arg2, Farmer farmer, Point point)
        {
            if (arg2.Length != 4)
            {
                ModMonitor.Log($"Error in TileAction {arg2} on tile {point} in {Game1.currentLocation.Name}", LogLevel.Error);
                return false;
            }
            if (!ArgUtility.TryGet(arg2, 1, out string? map, out string? error, false))
            {
                ModMonitor.Log($"Error in TileAction {arg2} on tile {point} in {Game1.currentLocation.Name}: Couldn't parse destination map.", LogLevel.Error);
                return false;
            }
            if (Game1.getLocationFromName(map) is not GameLocation destination)
            {
                ModMonitor.Log($"Error in TileAction {arg2} on tile {point} in {Game1.currentLocation.Name}: {map} is not a valid map", LogLevel.Error);
                return false;
            }
            if (!(int.TryParse(arg2[2], out int xCoord) && int.TryParse(arg2[3], out int yCoord)))
            {
                ModMonitor.Log($"Error in TileAction {arg2} on tile {point} in {Game1.currentLocation.Name}: Couldn't parse coordinates.", LogLevel.Error);
                return false;
            }
            ModMonitor.Log("LadderWarp loaded", LogLevel.Info);
            ModMonitor.Log($"Args {arg2}", LogLevel.Info);
            Farmer who = Game1.player;
            who.currentLocation.playSound("stairsdown");
            Game1.displayFarmer = false;
            Game1.player.temporarilyInvincible = true;
            Game1.player.temporaryInvincibilityTimer = -2000;
            Game1.player.freezePause = 1000;
            Game1.warpFarmer(destination.Name, xCoord, yCoord, who.FacingDirection, false);
            Game1.fadeToBlackAlpha = 0.99f;
            Game1.player.temporarilyInvincible = false;
            Game1.player.temporaryInvincibilityTimer = 0;
            Game1.displayFarmer = true;
            return true;
        }
    }
}
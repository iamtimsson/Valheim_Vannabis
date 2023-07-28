using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Vannabis
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    // You actually want this enabled for your final mod, probably set to Major because you have custom items added to the game
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Major)]
    internal class Vannabis : BaseUnityPlugin
    {
        public const string PluginGUID = "com.sicksicksix.Vannabis";
        public const string PluginName = "Vannabis";
        public const string PluginVersion = "0.0.1";

        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void LoadAssets()
        {
            // Step 1: Load the entire unity asset bundle by name
            AssetBundle bundle = AssetUtils.LoadAssetBundleFromResources("vannabis", Assembly.GetExecutingAssembly());

            // Step 2: Load unity asset, you will do this individually for each
            GameObject joint = bundle.LoadAsset<GameObject>("Joint");

            // Step 3: Create a new item configuration to change item
            ItemConfig jointConfig = new ItemConfig
            {
                CraftingStation = "piece_workbench",
                MinStationLevel = 1,
                Description = "Toke up my dudes",
                Requirements = new RequirementConfig[]
                {
                    new RequirementConfig()
                    {
                    Item = "Dandelion",
                    Amount = 2,
                    Recover = true
                    }
                }
            };

            // Step 4: Add the set up asset to the correct manager
            // The second parameter is "fix references" which is only needed if you are using the mocking system like missing pieces does
            ItemManager.Instance.AddItem(new CustomItem(joint, true, jointConfig));

            // Repeat 2-4 for every new asset
            //GameObject joint2 = bundle.LoadAsset<GameObject>("joint2");

            // etc
        }

        private void Awake()
        {
            LoadAssets();

            // load embedded localization Example if you need it
            string englishJson = AssetUtils.LoadTextFromResources("Localization.English.json", Assembly.GetExecutingAssembly());
            Localization.AddJsonFile("English", englishJson);

            // nonembedded resource, so they can edit
            //string englishJson = AssetUtils.LoadText(BepInEx.Paths.ConfigPath + Path.DirectorySeparatorChar + "Localization.English.json");
            //Localization.AddJsonFile("English", englishJson);

            Jotunn.Logger.LogInfo("Vannabis has landed");
        }
    }
}
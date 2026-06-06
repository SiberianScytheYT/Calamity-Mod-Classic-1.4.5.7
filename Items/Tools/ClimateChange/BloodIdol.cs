using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools.ClimateChange
{
    public class BloodIdol : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Relic");
/*
            Tooltip.SetDefault("Summons a blood moon");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.rare = 5;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item66;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.bloodMoon && !Main.dayTime;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            Main.bloodMoon = true;
            CalamityNetcode.SyncWorld();
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("EvilPowder", 10);
            recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

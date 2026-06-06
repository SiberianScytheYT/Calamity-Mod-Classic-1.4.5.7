using CalRD.Items.Materials;
using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools.ClimateChange
{
    public class Moonlight : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moonlight");
/*
            Tooltip.SetDefault("Summons the moon");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 5;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item60;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return Main.dayTime && !CalamityPlayer.areThereAnyDamnBosses;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
				Main.dayTime = false;
				CalamityNetcode.SyncWorld();
			}
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SoulofNight, 7);
            recipe.AddIngredient(ModContent.ItemType<CryoBar>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

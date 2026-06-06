using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools.ClimateChange
{
    public class Cosmolight : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cosmolight");
/*
            Tooltip.SetDefault("Changes night to day and vice versa");
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
            return !CalamityPlayer.areThereAnyDamnBosses;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Main.time = 0.0;
                Main.dayTime = !Main.dayTime;
                if (Main.dayTime)
                {
                    if (++Main.moonPhase >= 8)
                    {
                        Main.moonPhase = 0;
                    }
                }
                CalamityNetcode.SyncWorld();
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Daylight>());
            recipe.AddIngredient(ModContent.ItemType<Moonlight>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

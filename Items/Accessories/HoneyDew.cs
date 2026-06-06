using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class HoneyDew : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Honey Dew");
/*
            Tooltip.SetDefault("5% increased damage reduction, +5 defense, and increased life regen while in the Jungle\n" +
            "Poison and Venom immunity\n" +
            "Honey-like life regen with no speed penalty\n" +
            "Most bee/hornet enemies and projectiles do 75% damage to you");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
            Item.rare = 4;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.beeResist = true;
            if (player.ZoneJungle)
            {
                player.lifeRegen += 1;
                player.statDefense += 5;
                player.endurance += 0.05f;
            }
            player.buffImmune[BuffID.Venom] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            if (!player.honey && player.lifeRegen < 0)
            {
                player.lifeRegen += 2;
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
            }
            player.lifeRegenTime += 1;
            player.lifeRegen += 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<LivingDew>());
            recipe.AddIngredient(ItemID.BottledHoney, 10);
            recipe.AddIngredient(ModContent.ItemType<TrapperBulb>(), 2);
            recipe.AddIngredient(ItemID.ButterflyDust);
            recipe.AddIngredient(ModContent.ItemType<BeetleJuice>(), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

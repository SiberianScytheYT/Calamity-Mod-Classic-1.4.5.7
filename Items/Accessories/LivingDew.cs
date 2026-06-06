using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class LivingDew : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Living Dew");
/*
            Tooltip.SetDefault("5% increased damage reduction, +5 defense, and increased life regen while in the Jungle\n" +
			"Immunity to Poison");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.ZoneJungle)
            {
                player.lifeRegen += 1;
                player.statDefense += 5;
                player.endurance += 0.05f;
            }
            player.buffImmune[BuffID.Poisoned] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Vine, 2);
            recipe.AddIngredient(ModContent.ItemType<MurkyPaste>(), 5);
            recipe.AddIngredient(ItemID.BeeWax, 10);
            recipe.AddIngredient(ItemID.Bezoar);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class StatisBlessing : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Statis' Blessing");
/*
            Tooltip.SetDefault("Increased max minions by 2 and 10% increased minion damage\n" +
                "Increased minion knockback\n" +
                "Minions cause enemies to cry on hit");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.tearMinions = true;
            player.GetKnockback(DamageClass.Summon).Base += 2.5f;
            player.GetDamage(DamageClass.Summon) += 0.1f;
            player.maxMinions += 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PapyrusScarab);
            recipe.AddIngredient(ItemID.PygmyNecklace);
            recipe.AddIngredient(ItemID.SummonerEmblem);
            recipe.AddIngredient(ModContent.ItemType<CoreofCinder>(), 5);
            recipe.AddIngredient(ItemID.HolyWater, 30);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

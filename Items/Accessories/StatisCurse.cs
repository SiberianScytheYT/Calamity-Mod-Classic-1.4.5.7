using CalRD.CalPlayer;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class StatisCurse : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Statis' Curse");
/*
            Tooltip.SetDefault("Increased max minions by 3 and 10% increased minion damage\n" +
                "Increased minion knockback\n" +
                "Grants shadowflame powers to all minions\n" +
                "Minions make enemies cry on hit");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity10BuyPrice;
            Item.rare = 10;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.shadowMinions = true;
            modPlayer.tearMinions = true;
            player.GetKnockback(DamageClass.Summon) += 2.75f;
            player.GetDamage(DamageClass.Summon) += 0.1f;
            player.maxMinions += 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FragmentStardust, 10);
            recipe.AddIngredient(ModContent.ItemType<StatisBlessing>());
            recipe.AddIngredient(ModContent.ItemType<TheFirstShadowflame>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}

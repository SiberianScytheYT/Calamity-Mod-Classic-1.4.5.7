using CalRD.Items.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class SigilofCalamitas : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sigil of Calamitas");
/*
            Tooltip.SetDefault("10% increased magic damage and 10% decreased mana usage\n" +
                "Increases pickup range for mana stars and you restore mana when damaged\n" +
                "+100 max mana and reveals treasure locations if visibility is on");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 8));
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = 8;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicCuffs = true;
            player.manaMagnet = true;
            if (!hideVisual)
                player.findTreasure = true;
            player.statManaMax2 += 100;
            player.GetDamage(DamageClass.Magic) += 0.1f;
            player.manaCost *= 0.9f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SorcererEmblem);
            recipe.AddIngredient(ItemID.CelestialCuffs);
            recipe.AddIngredient(ItemID.CrystalShard, 20);
            recipe.AddIngredient(ModContent.ItemType<CalamityDust>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CoreofChaos>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 2);
            recipe.AddIngredient(ModContent.ItemType<ChaosAmulet>());
            recipe.AddRecipeGroup("AnyEvilWater", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

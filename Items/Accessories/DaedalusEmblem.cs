using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class DaedalusEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Artemis Emblem");
/*
            Tooltip.SetDefault("10% increased ranged damage, 5% increased ranged critical strike chance, and 20% reduced ammo usage\n" +
                "5 increased defense, 2 increased life regen, and 15% increased pick speed");
*/
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
            player.ammoCost80 = true;
            player.lifeRegen += 2;
            player.statDefense += 5;
            player.GetDamage(DamageClass.Ranged) += 0.1f;
            player.GetCritChance(DamageClass.Ranged) += 5;
            player.pickSpeed -= 0.15f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CelestialStone);
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>());
            recipe.AddIngredient(ItemID.RangerEmblem);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

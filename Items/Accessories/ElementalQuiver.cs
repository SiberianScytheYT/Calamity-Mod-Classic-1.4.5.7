using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class ElementalQuiver : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elemental Quiver");
/*
            Tooltip.SetDefault("Ranged projectiles have a chance to split\n" +
                "15% increased ranged damage, 5% increased ranged critical strike chance, and 20% reduced ammo usage\n" +
                "5 increased defense, 2 increased life regen, and 15% increased pick speed\n" +
				"Greatly increases arrow speed and grants a 20% chance to not consume arrows");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.eQuiver = true;
            player.GetDamage(DamageClass.Ranged) += 0.15f;
            player.GetCritChance(DamageClass.Ranged) += 5;
            player.ammoCost80 = true;
            player.lifeRegen += 2;
            player.statDefense += 5;
            player.pickSpeed -= 0.15f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MagicQuiver);
            recipe.AddIngredient(ModContent.ItemType<DaedalusEmblem>());
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}

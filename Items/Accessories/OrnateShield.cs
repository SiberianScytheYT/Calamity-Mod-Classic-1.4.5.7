using CalRD.Items.Materials;
using CalRD.Items.Armor;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	[AutoloadEquip(EquipType.Shield)]
	public class OrnateShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ornate Shield");
/*
            Tooltip.SetDefault("Boosted damage reduction and health while wearing the Daedalus armor");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.defense = 8;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if ((player.armor[0].type == ModContent.ItemType<DaedalusHat>() || player.armor[0].type == ModContent.ItemType<DaedalusHeadgear>() ||
                player.armor[0].type == ModContent.ItemType<DaedalusHelm>() || player.armor[0].type == ModContent.ItemType<DaedalusHelmet>() ||
                player.armor[0].type == ModContent.ItemType<DaedalusVisor>()) &&
                player.armor[1].type == ModContent.ItemType<DaedalusBreastplate>() && player.armor[2].type == ModContent.ItemType<DaedalusLeggings>())
            {
                player.endurance += 0.08f;
                player.statLifeMax2 += 20;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>(), 5);
            recipe.AddIngredient(ItemID.CrystalShard, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

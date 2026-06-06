using CalRD.Items.Materials;
using CalRD.Items.Armor;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class ShieldoftheOcean : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shield of the Ocean");
/*
            Tooltip.SetDefault("Increased defense by 5 when submerged in liquid\n" +
			"Increases movement speed and life regen while wearing the Victide armor");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = CalamityGlobalItem.Rarity2BuyPrice;
            Item.rare = 2;
            Item.defense = 2;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.statDefense += 5;
            }
            if ((player.armor[0].type == ModContent.ItemType<VictideHeadgear>() || player.armor[0].type == ModContent.ItemType<VictideHelm>() ||
                player.armor[0].type == ModContent.ItemType<VictideHelmet>() || player.armor[0].type == ModContent.ItemType<VictideMask>() ||
                player.armor[0].type == ModContent.ItemType<VictideVisage>()) &&
                player.armor[1].type == ModContent.ItemType<VictideBreastplate>() && player.armor[2].type == ModContent.ItemType<VictideLeggings>())
            {
                player.moveSpeed += 0.1f;
                player.lifeRegen += 2;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VictideBar>(), 5);
            recipe.AddIngredient(ItemID.Coral, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

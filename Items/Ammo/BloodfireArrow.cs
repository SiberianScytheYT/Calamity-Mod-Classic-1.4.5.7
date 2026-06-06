using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Ammo
{
    public class BloodfireArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodfire Arrow");
/*
            Tooltip.SetDefault("Heals you a small amount on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 36;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(copper: 80);
            Item.shoot = ModContent.ProjectileType<BloodfireArrowProj>();
            Item.shootSpeed = 10f;
            Item.ammo = AmmoID.Arrow;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient(ModContent.ItemType<BloodstoneCore>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}

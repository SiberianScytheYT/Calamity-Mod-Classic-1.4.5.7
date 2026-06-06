using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Ammo
{
    public class ArcticArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arctic Arrow");
/*
            Tooltip.SetDefault("Freezes enemies for a short time");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 36;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(0, 0, 0, 24);
            Item.rare = 3;
            Item.shoot = ModContent.ProjectileType<ArcticArrowProj>();
            Item.shootSpeed = 13f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(250);
            recipe.AddIngredient(ModContent.ItemType<CryoBar>());
            recipe.AddTile(TileID.IceMachine);
            recipe.Register();
        }
    }
}

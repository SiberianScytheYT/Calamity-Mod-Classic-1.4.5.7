using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Ammo
{
    public class EnhancedNanoRound : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Enhanced Nano Round");
/*
            Tooltip.SetDefault("Confuses enemies and releases a cloud of nanites when enemies die");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 5.5f;
            Item.value = 500;
            Item.rare = 3;
            Item.shoot = ModContent.ProjectileType<EnhancedNanoRoundProj>();
            Item.shootSpeed = 8f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(250);
            recipe.AddIngredient(ItemID.NanoBullet, 250);
            recipe.AddIngredient(ModContent.ItemType<EssenceofEleum>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

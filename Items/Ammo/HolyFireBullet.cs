using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Ammo
{
    public class HolyFireBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Holy Fire Bullet");
/*
            Tooltip.SetDefault("Explosive holy bullets");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 2f;
            Item.value = 2000;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.shoot = ModContent.ProjectileType<HolyFireBulletProj>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(100);
            recipe.AddIngredient(ItemID.ExplodingBullet, 100);
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}

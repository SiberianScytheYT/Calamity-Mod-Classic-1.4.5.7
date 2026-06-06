using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class FrigidflashBolt : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frigidflash Bolt");
/*
            Tooltip.SetDefault("Casts a slow-moving ball of flash-freezing magma");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 13;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item21;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FrigidflashBoltProjectile>();
            Item.shootSpeed = 6.5f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FrostBolt>());
            recipe.AddIngredient(ModContent.ItemType<FlareBolt>());
            recipe.AddIngredient(ModContent.ItemType<EssenceofEleum>(), 2);
            recipe.AddIngredient(ModContent.ItemType<EssenceofChaos>(), 2);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}

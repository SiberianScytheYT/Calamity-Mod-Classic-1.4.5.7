using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Shadethrower : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shadethrower");
/*
            Tooltip.SetDefault("66% chance to not consume gel");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 76;
            Item.height = 30;
            Item.useTime = 10;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.UseSound = SoundID.Item34;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ShadeFire>();
            Item.shootSpeed = 5.5f;
            Item.useAmmo = AmmoID.Gel;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 66)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RottenChunk, 3);
            recipe.AddIngredient(ItemID.DemoniteBar, 7);
            recipe.AddIngredient(ModContent.ItemType<TrueShadowScale>(), 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}

using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class TerraFlameburster : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terra Flameburster");
/*
            Tooltip.SetDefault("80% chance to not consume gel");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 43;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 68;
            Item.height = 22;
            Item.useTime = 3;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.UseSound = SoundID.Item34;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TerraFireGreen>();
            Item.shootSpeed = 7.5f;
            Item.useAmmo = AmmoID.Gel;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 80)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Flamethrower);
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

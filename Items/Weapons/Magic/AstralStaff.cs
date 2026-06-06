using CalRD.Items.Placeables;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class AstralStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Staff");
/*
            Tooltip.SetDefault("Summons a large crystal from the sky that has a large area of effect on impact.");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 270;
            Item.crit += 15;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 26;
            Item.width = 86;
            Item.height = 72;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item105;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AstralCrystal>();
            Item.shootSpeed = 15f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 6);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 spawnPos = new Vector2(player.MountedCenter.X + Main.rand.Next(-200, 201), player.MountedCenter.Y - 600f);
            Vector2 targetPos = Main.MouseWorld + new Vector2(Main.rand.Next(-30, 31), Main.rand.Next(-30, 31));
            Vector2 velocity1 = targetPos - spawnPos;
            velocity1.Normalize();
            velocity1 *= 13f;

            int p = Projectile.NewProjectile(source, spawnPos, velocity1, type, damage, Item.knockBack, player.whoAmI);
            Main.projectile[p].ai[0] = targetPos.Y - 120;

            return false;
        }
    }
}

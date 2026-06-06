using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class GreatbowofTurmoil : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Continental Greatbow");
/*
            Tooltip.SetDefault("Wooden arrows are set alight with fire\n" +
				"Fires 3 arrows at once\n" +
                "Fires 2 additional cursed, hellfire, or ichor arrows");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 31;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 18;
            Item.height = 36;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 17f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 source1 = player.RotatedRelativePoint(player.MountedCenter, true);
            float piOverTen = MathHelper.Pi * 0.1f;
            int arrowAmt = 3;
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            velocity1.Normalize();
            velocity1 *= 40f;
            bool canHit = Collision.CanHit(source1, 0, 0, source1 + velocity1, 0, 0);
            for (int projIndex = 0; projIndex < arrowAmt; projIndex++)
            {
                float offsetAmt = projIndex - (arrowAmt - 1f) / 2f;
                Vector2 offset = velocity1.RotatedBy((double)(piOverTen * offsetAmt), default);
                if (!canHit)
                {
                    offset -= velocity1;
                }
				if (type == ProjectileID.WoodenArrowFriendly)
				{
					type = ProjectileID.FireArrow;
				}
                int num121 = Projectile.NewProjectile(source, source1 + offset, new Vector2(velocity1.X, velocity1.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[num121].noDropItem = true;
            }
            for (int i = 0; i < 2; i++)
            {
                float SpeedX = velocity1.X + (float)Main.rand.Next(-10, 11) * 0.05f;
                float SpeedY = velocity1.Y + (float)Main.rand.Next(-10, 11) * 0.05f;
				type = Utils.SelectRandom(Main.rand, new int[]
				{
					ProjectileID.CursedArrow,
					ProjectileID.HellfireArrow,
					ProjectileID.IchorArrow
				});
                int index = Projectile.NewProjectile(source, position, new Vector2(velocity1.X, velocity1.Y), type, (int)(damage * 0.5f), Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[index].noDropItem = true;
                Main.projectile[index].usesLocalNPCImmunity = true;
                Main.projectile[index].localNPCHitCooldown = 10;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

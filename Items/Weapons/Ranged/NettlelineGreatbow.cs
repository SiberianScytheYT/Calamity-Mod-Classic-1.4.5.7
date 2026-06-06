using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class NettlelineGreatbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nettlevine Greatbow");
/*
            Tooltip.SetDefault("Shoots 4 arrows at once\n" +
                "Fires 2 additional venom or chlorophyte arrows");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 115;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 36;
            Item.height = 64;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num117 = 0.314159274f;
            int num118 = 4;
            Vector2 vector7 = new Vector2(velocity.X, velocity.Y);
            vector7.Normalize();
            vector7 *= 40f;
            bool flag11 = Collision.CanHit(vector2, 0, 0, vector2 + vector7, 0, 0);
            for (int num119 = 0; num119 < num118; num119++)
            {
                float num120 = (float)num119 - ((float)num118 - 1f) / 2f;
                Vector2 value9 = vector7.RotatedBy((double)(num117 * num120), default);
                if (!flag11)
                {
                    value9 -= vector7;
                }
                int num121 = Projectile.NewProjectile(source, vector2.X + value9.X, vector2.Y + value9.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[num121].noDropItem = true;
            }
            for (int i = 0; i < 2; i++)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-40, 41) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.05f;
                switch (Main.rand.Next(2))
                {
                    case 0:
                        type = ProjectileID.VenomArrow;
                        break;
                    case 1:
                        type = ProjectileID.ChlorophyteArrow;
                        break;
                    default:
                        break;
                }
                int index = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[index].noDropItem = true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 12);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}

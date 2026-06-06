using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class StardustStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eidolon Staff");
/*
            Tooltip.SetDefault("The power of an ancient cultist resonates within this staff\n" +
                "Fires a spread of ancient light and has a chance to fire a spinning ice cluster");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 56;
            Item.height = 56;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Starblast>();
            Item.shootSpeed = 12f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            int i = Main.myPlayer;
            float num72 = Item.shootSpeed;
            int num73 = Item.damage;
            float num74 = Item.knockBack;
            float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
            float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
            float num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
            int num130 = 6;
            if (Main.rand.NextBool(3))
            {
                num130++;
            }
            if (Main.rand.NextBool(4))
            {
                num130++;
            }
            if (Main.rand.NextBool(5))
            {
                num130++;
            }
            if (Main.rand.NextBool(3))
            {
                float num132 = num78;
                float num133 = num79;
                num80 = (float)Math.Sqrt((double)(num132 * num132 + num133 * num133));
                num80 = num72 / num80;
                num132 *= num80;
                num133 *= num80;
                float x2 = vector2.X;
                float y2 = vector2.Y;
                Projectile.NewProjectile(source, x2, y2, num132, num133, ModContent.ProjectileType<IceCluster>(), num73, num74, i, 0f, 1f);
            }
            else
            {
                for (int num131 = 0; num131 < num130; num131++)
                {
                    float num132 = num78;
                    float num133 = num79;
                    float num134 = 0.05f * (float)num131;
                    num132 += (float)Main.rand.Next(-155, 156) * num134;
                    num133 += (float)Main.rand.Next(-155, 156) * num134;
                    num80 = (float)Math.Sqrt((double)(num132 * num132 + num133 * num133));
                    num80 = num72 / num80;
                    num132 *= num80;
                    num133 *= num80;
                    float x2 = vector2.X;
                    float y2 = vector2.Y;
                    Projectile.NewProjectile(source, x2, y2, num132, num133, ModContent.ProjectileType<Starblast>(), num73, num74, i, 0f, 0f);
                }
            }
            return false;
        }
    }
}

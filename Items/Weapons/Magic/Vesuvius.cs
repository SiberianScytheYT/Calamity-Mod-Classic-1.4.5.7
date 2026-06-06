using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class Vesuvius : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vesuvius");
/*
            Tooltip.SetDefault("Legendary drop\n" +
                "Asteroids give the Molten buff on enemy hits\n" +
                "Calls down a swarm of molten asteroids\n" +
                "Right click to fire a spread of molten asteroids from the staff\n" +
                "Revengeance drop");
*/
            Item.staff[Item.type] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.damage = 75;
            Item.mana = 6;
            Item.DamageType = DamageClass.Magic;
            Item.useAnimation = Item.useTime = 15;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item88;
            Item.autoReuse = true;
            Item.height = 62;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<AsteroidMolten>();
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        /*public override Vector2? HoldoutOrigin()
        {
            return new Vector2(30, 30);
        }*/

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.altFunctionUse == 2)
				mult *= 1.5f;
		}

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse != 2)
				return 1f;
			//return 0.75f;
            return 1.33f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                int num6 = Main.rand.Next(4, 6);
                for (int index = 0; index < num6; ++index)
                {
                    float SpeedX = velocity.X + (float)Main.rand.Next(-30, 31) * 0.05f;
                    float SpeedY = velocity.Y + (float)Main.rand.Next(-30, 31) * 0.05f;
                    float ai0 = (float)Main.rand.Next(6);
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, ai0, 0.5f + (float)Main.rand.NextDouble() * 0.9f);
                }
                return false;
            }
            else
            {
                float num72 = Item.shootSpeed;
                Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                if (player.gravDir == -1f)
                {
                    num79 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2.Y;
                }
                float num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
                if ((float.IsNaN(num78) && float.IsNaN(num79)) || (num78 == 0f && num79 == 0f))
                {
                    num78 = (float)player.direction;
                    num79 = 0f;
                    num80 = num72;
                }
                else
                {
                    num80 = num72 / num80;
                }

                for (int num113 = 0; num113 < 4; num113++)
                {
                    vector2 = new Vector2(player.position.X + (float)player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                    vector2.X = (vector2.X + player.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                    vector2.Y -= (float)(100 * num113);
                    num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X + (float)Main.rand.Next(-40, 41) * 0.03f;
                    num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                    if (num79 < 0f)
                    {
                        num79 *= -1f;
                    }
                    if (num79 < 20f)
                    {
                        num79 = 20f;
                    }
                    num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
                    num80 = num72 / num80;
                    num78 *= num80;
                    num79 *= num80;
                    float num114 = num78;
                    float num115 = num79 + (float)Main.rand.Next(-40, 41) * 0.02f;
                    float ai0 = (float)Main.rand.Next(6);
                    Projectile.NewProjectile(source, vector2.X, vector2.Y, num114 * 0.75f, num115 * 0.75f, type, damage, Item.knockBack, player.whoAmI, ai0, 0.5f + (float)Main.rand.NextDouble() * 0.9f); //0.3
                }
                return false;
            }
        }
    }
}

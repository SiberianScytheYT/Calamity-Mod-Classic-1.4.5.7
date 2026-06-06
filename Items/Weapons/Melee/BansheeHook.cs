using CalRD.Projectiles.Melee.Spears;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class BansheeHook : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Banshee Hook");
/*
            Tooltip.SetDefault("Swings a banshee hook that fires blades and explodes on hit");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 120;
            Item.damage = 155;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 21;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
            Item.autoReuse = true;
            Item.height = 108;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<BansheeHookProj>();
            Item.shootSpeed = 42f;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Melee/BansheeHookGlow").Value);
		}

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float num82 = (float)Main.mouseX + Main.screenPosition.X - position.X;
            float num83 = (float)Main.mouseY + Main.screenPosition.Y - position.Y;
            if (player.gravDir == -1f)
            {
                num83 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - position.Y;
            }
            float num84 = (float)Math.Sqrt((double)(num82 * num82 + num83 * num83));
            if ((float.IsNaN(num82) && float.IsNaN(num83)) || (num82 == 0f && num83 == 0f))
            {
                num82 = (float)player.direction;
                num83 = 0f;
                num84 = Item.shootSpeed;
            }
            else
            {
                num84 = Item.shootSpeed / num84;
            }
            num82 *= num84;
            num83 *= num84;
            float ai4 = Main.rand.NextFloat() * Item.shootSpeed * 0.75f * (float)player.direction;
            Vector2 velocity1 = new Vector2(num82, num83);
            Projectile.NewProjectile(source, position.X, position.Y, velocity1.X, velocity1.Y, type, damage, Item.knockBack, player.whoAmI, ai4, 0.0f);
            return false;
        }
    }
}

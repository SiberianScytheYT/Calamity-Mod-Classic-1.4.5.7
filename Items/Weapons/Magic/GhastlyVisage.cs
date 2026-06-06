using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Weapons.Magic
{
    public class GhastlyVisage : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ghastly Visage");
/*
            Tooltip.SetDefault("Fires homing ghast energy that explodes");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 92;
            Item.DamageType = DamageClass.Magic;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.mana = 20;
            Item.width = 32;
            Item.height = 36;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shootSpeed = 9f;
            Item.shoot = ModContent.ProjectileType<GhastlyVisageProj>();
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Magic/GhastlyVisageGlow").Value);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<GhastlyVisageProj>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}

using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Magic
{
    public class PurgeGuzzler : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Purge Guzzler");
/*
            Tooltip.SetDefault("Fires three beams of holy energy");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.width = 58;
            Item.height = 44;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/LaserCannon");
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HolyLaser>();
            Item.shootSpeed = 6f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numProj = 2;
            float rotation = MathHelper.ToRadians(4);
            for (int i = 0; i < numProj + 1; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}

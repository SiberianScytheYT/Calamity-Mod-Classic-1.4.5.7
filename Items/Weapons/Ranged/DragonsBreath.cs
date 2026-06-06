using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class DragonsBreath : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dragon's Breath");
/*
            Tooltip.SetDefault("80% chance to not consume ammo\n" +
                "Shoots a spread of exploding fire bullets");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 260;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 28;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6.5f;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 5);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 13; i++)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-20, 21) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-20, 21) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<DragonBurst>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 80)
                return false;
            return true;
        }
    }
}

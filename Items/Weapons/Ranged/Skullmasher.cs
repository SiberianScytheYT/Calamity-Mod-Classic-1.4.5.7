using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Ranged
{
    public class Skullmasher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Skullmasher");
/*
            Tooltip.SetDefault("Sniper shotgun, because why not?\n" +
                "If you crit the target a second swarm of bullets will fire near the target");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 2310;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 76;
            Item.crit += 5;
            Item.height = 30;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PlasmaBlast");
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 5; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-15, 16) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-15, 16) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<AMRShot>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}

using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class ThornBlossom : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Thorn Blossom");
/*
            Tooltip.SetDefault("Every rose has its thorn");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 66;
            Item.height = 68;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BeamingBolt>();
            Item.shootSpeed = 20f;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override Vector2? HoldoutOrigin() => new Vector2(15, 15);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.statLife -= 5;
            if (player.statLife <= 0)
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was violently pricked by a flower."), 1000.0, 0, false);
            }
            for (int index = 0; index < 3; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-120, 121) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-120, 121) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX * 1.5f, SpeedY * 1.5f, ModContent.ProjectileType<NettleRight>(), (int)(damage * 1.5), Item.knockBack, player.whoAmI, 0f, 0f);
            }
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 0.66f, velocity.Y * 0.66f, type, damage, Item.knockBack, player.whoAmI, 1f, 0f);
            return false;
        }
    }
}

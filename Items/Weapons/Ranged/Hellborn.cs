using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Ranged
{
    public class Hellborn : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hellborn");
/*
            Tooltip.SetDefault("Converts all bullets to explosive rounds\n" +
                "Enemies that touch the gun while it's being fired take massive damage");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 50;
            Item.height = 24;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 14f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 3; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-15, 16) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-15, 16) * 0.05f;
                int bullet = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileID.ExplosiveBullet, damage, Item.knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[bullet].usesLocalNPCImmunity = true;
				Main.projectile[bullet].localNPCHitCooldown = 10;
            }
            return false;
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= 15;
        }

        public override void ModifyHitPvp(Player player, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= 15;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 360);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OnFire, 360);
        }
    }
}

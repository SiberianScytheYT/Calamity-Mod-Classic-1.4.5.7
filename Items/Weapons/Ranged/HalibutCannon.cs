using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class HalibutCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Halibut Cannon");
/*
            Tooltip.SetDefault("Becomes more powerful as you progress\n" +
                "(Yes, it's still overpowered)\n" +
                "Revengeance drop");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 108;
            Item.height = 54;
            Item.useTime = 10;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = 10;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.UseSound = SoundID.Item38;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-15, 0);

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float damageMult = 0f +
                    (NPC.downedPlantBoss ? 0.1f : 0f) +
                    (NPC.downedGolemBoss ? 0.1f : 0f) +
                    (NPC.downedAncientCultist ? 0.2f : 0f) +
                    (NPC.downedMoonlord ? 1f : 0f) +
                    (CalamityWorld.downedProvidence ? 0.15f : 0f) +
                    (CalamityWorld.downedPolterghast ? 0.3f : 0f) +
                    (CalamityWorld.downedDoG ? 0.6f : 0f) +
                    (CalamityWorld.downedYharon ? 1f : 0f);
            damage *= damageMult + 1f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int bulletAmt = Main.rand.Next(25, 36);
            for (int index = 0; index < bulletAmt; ++index)
            {
                float SpeedX = velocity.X + Main.rand.Next(-10, 11) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-10, 11) * 0.05f;
                int shot = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[shot].timeLeft = 120;
            }
            return false;
        }
    }
}

using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class DeepWounder : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Deep Wounder");
/*
            Tooltip.SetDefault("Throws an abyssal hatchet that inflicts Armor Crunch and Marked for Death to the enemies it hits\n" +
                "Stealth strikes cause the hatchet to be thrown faster and trail water, inflicting Crush Depth in addition to the other debuffs");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 52;
            Item.damage = 106;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 23;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 48;
            Item.maxStack = 1;
            Item.rare = 7;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.shoot = ModContent.ProjectileType<DeepWounderProjectile>();
            Item.shootSpeed = 14f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                float stealthSpeedMult = 1.5f;
                Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
                velocity1.Normalize();
                velocity1 *= Item.shootSpeed * stealthSpeedMult;

                int p = Projectile.NewProjectile(source, position.X, position.Y, velocity1.X, velocity1.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
                Main.projectile[p].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}

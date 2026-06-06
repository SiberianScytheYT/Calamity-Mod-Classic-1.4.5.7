using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class BlastBarrel : RogueWeapon
    {
        public const int BaseDamage = 32;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blast Barrel");
/*
            Tooltip.SetDefault("Throws a rolling barrel that explodes on wall collision\n" +
                               "Stealth strikes makes the barrel bounce twice before disappearing with varied effects after each bounce\n" +
                               "'Some people used to jump over these'");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = BaseDamage;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 12, 0, 0); //2 gold 40 silver sellprice
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<BlastBarrelProjectile>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            velocity.Y *= 0.7f; //since the barrel is heavy
            Vector2 initialVelocity = new Vector2(velocity.X, velocity.Y);

            //unitY additive is do it doesn't exploe initially
            int p = Projectile.NewProjectile(source, position - Vector2.UnitY * 12f, initialVelocity, type, damage, Item.knockBack, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable())
                Main.projectile[p].Calamity().stealthStrike = true;
            return false;
        }
    }
}

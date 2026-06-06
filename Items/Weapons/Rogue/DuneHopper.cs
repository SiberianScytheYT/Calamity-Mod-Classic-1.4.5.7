using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class DuneHopper : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dune Hopper");
/*
            Tooltip.SetDefault("Throws a spear that bounces a lot\n"
                               +"Stealth strikes throw three high speed spears");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 44;
            Item.damage = 18;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 44;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<DuneHopperProjectile>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int numProj = 2;
                float rotation = MathHelper.ToRadians(3);
                for (int i = 0; i < numProj + 1; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X - 3f, velocity.Y - 3f).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
                    Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<DuneHopperProjectile>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
                }
                return false;
            }
            return true;
        }
    }
}

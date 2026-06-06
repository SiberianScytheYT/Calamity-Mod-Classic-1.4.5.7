using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class SpearofDestiny : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spear of Destiny");
/*
            Tooltip.SetDefault("Throws three spears with the outer two having homing capabilities\n" +
			"Stealth strikes cause all three spears to home in, ignore tiles, and pierce more");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 52;
            Item.damage = 25;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 52;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<SpearofDestinyProjectile>();
            Item.shootSpeed = 20f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int index = 7;
            for (int i = -index; i <= index; i += index)
            {
				int projType = (i != 0 || player.Calamity().StealthStrikeAvailable()) ? type : ModContent.ProjectileType<IchorSpearProj>();
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(i));
                int spear = Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, projType, damage, Item.knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[spear].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            }
            return false;
        }
    }
}

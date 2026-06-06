using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class SpentFuelContainer : RogueWeapon
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spent Fuel Container");
/*
            Tooltip.SetDefault("War Never Changes\n" + //Fallout reference breh, pls don't fall out with me :cri:
							   "Throws a fuel container with trace amounts of plutonium that causes a nuclear explosion\n" +
                               "The explosion does not occur if there are no tiles below it\n" +
                               "Stealth strikes leave a lingering irradiated zone after the explosion dissipates");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 50;
            Item.width = 22;
            Item.height = 24;
            Item.useAnimation = 35;
            Item.useTime = 35;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
            Item.rare = 6;
            Item.UseSound = SoundID.Item106;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 48); //sell price of 9 gold 60 silver
            Item.shoot = ModContent.ProjectileType<SpentFuelContainerProjectile>();
            Item.shootSpeed = 15f;
            Item.Calamity().rogue = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            bool stealthAvailable = player.Calamity().StealthStrikeAvailable();
            int p = Projectile.NewProjectile(source, position, velocity1, ModContent.ProjectileType<SpentFuelContainerProjectile>(), damage, Item.knockBack, player.whoAmI, stealthAvailable ? 1f : 0f);
            if (stealthAvailable)
                Main.projectile[p].Calamity().stealthStrike = true;
            return false;
        }
    }
}

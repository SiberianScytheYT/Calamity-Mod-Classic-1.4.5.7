using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class Quasar : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Quasar");
/*
            Tooltip.SetDefault("Succ\n" +
			"Stealth strikes spawn more explosions");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 52;
            Item.damage = 80; //50
            Item.crit += 12;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 0f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 48;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.shoot = ModContent.ProjectileType<QuasarKnife>();
            Item.shootSpeed = 20f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int knife = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            Main.projectile[knife].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }
    }
}

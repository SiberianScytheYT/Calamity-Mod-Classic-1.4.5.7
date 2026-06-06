using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Rogue
{
    public class SylvanSlasher : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sylvan Slasher");
/*
            Tooltip.SetDefault("Summons a slash attack at the cursor's position\nEnemy hits build stealth and cause sword waves to fire from the player in the opposite direction\nDoes not consume stealth and cannot stealth strike");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 72;
            Item.damage = 87;
            Item.Calamity().rogue = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 5;
            Item.knockBack = 3f;
            Item.autoReuse = false;
            Item.height = 78;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<SylvanSlashAttack>();
            Item.shootSpeed = 24f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}

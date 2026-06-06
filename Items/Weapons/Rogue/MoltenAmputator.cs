using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class MoltenAmputator : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Molten Amputator");
/*
            Tooltip.SetDefault("Throws a scythe that emits molten globs on enemy hits\n" +
                "Stealth strikes spawn molten globs periodically in flight and more on-hit");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 60;
            Item.damage = 169;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.height = 60;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<MoltenAmputatorProj>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int boomer = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[boomer].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}

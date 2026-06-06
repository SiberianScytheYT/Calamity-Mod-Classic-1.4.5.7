using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Rogue
{
    public class UrchinStinger : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Urchin Stinger");
/*
            Tooltip.SetDefault("Stealth strikes stick to enemies while releasing sulfuric bubbles");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 10;
            Item.damage = 17;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.knockBack = 1.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 26;
            Item.maxStack = 999;
            Item.value = 200;
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<UrchinStingerProj>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<UrchinStingerProj>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            if (player.Calamity().StealthStrikeAvailable())
                Main.projectile[proj].Calamity().stealthStrike = true;
            return false;
        }
    }
}

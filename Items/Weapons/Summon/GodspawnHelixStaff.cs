using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class GodspawnHelixStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Godspawn Helix Staff");
/*
            Tooltip.SetDefault("Summons astral probes to protect you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.mana = 10;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1.25f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AstralProbeSummon>();
            Item.DamageType = DamageClass.Summon;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld;
            velocity.X = 0;
            velocity.Y = 0;
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI);
            return false;
        }
    }
}

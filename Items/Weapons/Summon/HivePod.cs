using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class HivePod : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hive Pod");
/*
            Tooltip.SetDefault("Summons an astral hive to protect you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.mana = 10;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.width = 46;
            Item.height = 50;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.UseSound = SoundID.Item78;
            Item.shoot = ModContent.ProjectileType<Hive>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			//CalamityUtils.OnlyOneSentry(player, type);
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
			player.UpdateMaxTurrets();
            return false;
        }
    }
}

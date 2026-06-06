using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
	public class LanternoftheSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Guidelight of Oblivion");
/*
            Tooltip.SetDefault("Shadows dream of endless fire, flames devour and embers swoop\n" +
			"Summons a lantern turret to fight for you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 250;
            Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
            Item.mana = 10;
            Item.width = 42;
            Item.height = 60;
            Item.useTime = Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LanternSoul>();
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
            Item.UseSound = SoundID.Item44;
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

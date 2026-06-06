using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Weapons.Summon
{
    public class EnergyStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Energy Staff");
/*
            Tooltip.SetDefault("Summons a profaned energy turret to fight for you");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 170;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 25;
            Item.width = 66;
            Item.height = 68;
            Item.useTime = Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ProfanedEnergy>();
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			//CalamityUtils.OnlyOneSentry(player, type);
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI, 16f);
			player.UpdateMaxTurrets();
            return false;
        }
    }
}

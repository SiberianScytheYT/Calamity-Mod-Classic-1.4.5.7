using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class CadaverousCarrion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cadaverous Carrion");
/*
            Tooltip.SetDefault("Summons a gross Old Duke head on the ground");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 640;
            Item.mana = 32;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.width = 54;
            Item.height = 56;
            Item.useTime = Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.Calamity().postMoonLordRarity = 13;
			Item.rare = 10;
            Item.UseSound = SoundID.NPCDeath13;
            Item.shoot = ModContent.ProjectileType<OldDukeHeadCorpse>();
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

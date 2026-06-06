using CalRD.Buffs.StatDebuffs;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class AbsoluteZero : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Absolute Zero");
/*
            Tooltip.SetDefault("Ancient blade imbued with the Archmage of Ice's magic\n"
                               +"Shoots dark ice crystals\n"
                               +"The blade creates frost explosions on direct hits");
*/
        }
        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 58;
            Item.height = 58;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = false;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DarkIceZero>();
            Item.shootSpeed = 3f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 600);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 300);

            int p = Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<DarkIceZero>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack * 3f, player.whoAmI);
            Main.projectile[p].Kill();
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Frostburn, 600);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 300);

            int p = Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<DarkIceZero>(), (int)(Item.damage * player.MeleeDamage()), 12f, player.whoAmI);
            Main.projectile[p].Kill();
        }
    }
}

using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class MirrorBlade : ModItem
    {
        private int baseDamage = 295;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mirror Blade");
/*
            Tooltip.SetDefault("The amount of contact damage an enemy does is added to this weapons' damage\n" +
                "You must hit an enemy with the blade to trigger this effect");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 52;
            Item.damage = baseDamage;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 62;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shootSpeed = 9f;
            Item.shoot = ModContent.ProjectileType<MirrorBlast>();
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int conDamage = target.damage + baseDamage;
            if (conDamage < baseDamage)
            {
                conDamage = baseDamage;
            }
            if (conDamage > 750)
            {
                conDamage = 750;
            }
            Item.damage = conDamage;
        }
    }
}

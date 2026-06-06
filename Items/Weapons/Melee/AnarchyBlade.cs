using CalRD.Buffs.DamageOverTime;
using CalRD.CalPlayer;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.NPCs;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class AnarchyBlade : ModItem
    {
        private static int BaseDamage = 110;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Anarchy Blade");
/*
            Tooltip.SetDefault("The lower your life the more damage this blade does\n" +
                "Your hits will generate a large explosion\n" +
                "If you're below 50% life your hits have a chance to instantly kill regular enemies");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 94;
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 19;
            Item.useTime = 19;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 100;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CoreofChaos>(), 3);
            recipe.AddIngredient(ItemID.BrokenHeroSword);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, (int)CalamityDusts.Brimstone);
            }
        }

        // Gains 10% of missing health as base damage.
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            int lifeAmount = player.statLifeMax2 - player.statLife;
            damage.Base += lifeAmount * 0.1f; // * player.MeleeDamage();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<BrimstoneBoom>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, Main.myPlayer);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);

            if (player.statLife < (player.statLifeMax2 * 0.5f) && Main.rand.NextBool(5))
            {
                if (!CalamityPlayer.areThereAnyDamnBosses && CalamityGlobalNPC.ShouldAffectNPC(target))
                {
                    target.life = 0;
                    target.HitEffect(0, 10.0);
                    target.active = false;
                    target.NPCLoot();
                }
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<BrimstoneBoom>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, Main.myPlayer);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
        }
    }
}

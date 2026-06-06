using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatBuffs;
using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TrueTyrantYharimsUltisword : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("True Tyrant's Ultisword");
/*
            Tooltip.SetDefault("Fires blazing, hyper, and sunlight blades\n" +
                "Gives the player the tyrant's fury buff on enemy hits\n" +
                "This buff increases melee damage by 30% and melee crit chance by 10%");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 102;
            Item.damage = 185;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useTurn = true;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 102;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<BlazingPhantomBlade>();
            Item.shootSpeed = 12f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    type = ModContent.ProjectileType<BlazingPhantomBlade>();
                    break;
                case 1:
                    type = ModContent.ProjectileType<HyperBlade>();
                    break;
                case 2:
                    type = ModContent.ProjectileType<SunlightBlade>();
                    break;
                default:
                    break;
            }
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, Main.myPlayer);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TyrantYharimsUltisword>());
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>());
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 15);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 106);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.AddBuff(ModContent.BuffType<TyrantsFury>(), 300);
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(BuffID.Venom, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            player.AddBuff(ModContent.BuffType<TyrantsFury>(), 300);
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(BuffID.Venom, 300);
        }
    }
}

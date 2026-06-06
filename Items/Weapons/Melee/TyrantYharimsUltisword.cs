using CalRD.Buffs.StatBuffs;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TyrantYharimsUltisword : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tyrant Yharim's Ultisword");
/*
            Tooltip.SetDefault("Fires homing blazing blades\n" +
                "Gives the player the tyrant's fury buff on enemy hits\n" +
                "This buff increases melee damage by 30% and melee crit chance by 10%");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 88;
            Item.damage = 64;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 26;
            Item.useTurn = true;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 88;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<BlazingPhantomBlade>();
            Item.shootSpeed = 12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TrueCausticEdge>());
            recipe.AddIngredient(ItemID.BrokenHeroSword);
            recipe.AddIngredient(ItemID.FlaskofVenom, 5);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 15);
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
            player.AddBuff(ModContent.BuffType<TyrantsFury>(), 180);
            target.AddBuff(BuffID.OnFire, 180);
            target.AddBuff(BuffID.Venom, 180);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            player.AddBuff(ModContent.BuffType<TyrantsFury>(), 180);
            target.AddBuff(BuffID.OnFire, 180);
            target.AddBuff(BuffID.Venom, 180);
        }
    }
}

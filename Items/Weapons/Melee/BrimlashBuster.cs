using CalRD.Dusts;
using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class BrimlashBuster : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimlash Buster");
/*
            Tooltip.SetDefault("50% chance to do triple damage on enemy hits\n" +
                "Fires a brimstone bolt that explodes into more bolts on death");
*/
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 72;
            Item.damage = 126;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.shoot = ModContent.ProjectileType<BrimlashProj>();
            Item.shootSpeed = 18f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Brimlash>());
            recipe.AddIngredient(ModContent.ItemType<CoreofChaos>(), 3);
            recipe.AddIngredient(ItemID.FragmentSolar, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, (int)CalamityDusts.Brimstone);
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			float damageMult = 0f;
            if (player.Calamity().brimlashBusterBoost)
				damageMult = 2f;
			damage.Base *= damageMult;
		}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
			player.Calamity().brimlashBusterBoost = Main.rand.NextBool(3);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
			player.Calamity().brimlashBusterBoost = Main.rand.NextBool(3);
        }
    }
}

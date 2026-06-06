using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class CorruptedCrusherBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Corrupted Crusher Blade");
/*
            Tooltip.SetDefault("Inflicts cursed inferno and critical hits lower enemy defense");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 39;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 66;
            Item.height = 80;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.75f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(7))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 120);
            if (target.defense > 0 && hit.Crit && !CalamityPlayer.areThereAnyDamnBosses)
            {
				target.defense = target.defDefense - 5;
			}
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.CursedInferno, 120);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EbonianGel>(), 15);
            recipe.AddIngredient(ItemID.EbonstoneBlock, 50);
            recipe.AddIngredient(ItemID.ShadowScale, 5);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 4);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}

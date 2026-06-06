using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Tools
{
    public class AstralHamaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Hamaxe");
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.crit += 25;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 60;
            Item.height = 70;
            Item.useTime = 8;
            Item.useAnimation = 20;
            Item.useTurn = true;
            Item.axe = 30;
            Item.hammer = 150;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.tileBoost += 3;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust d = CalamityGlobalItem.MeleeDustHelper(player, Main.rand.NextBool(2) ? ModContent.DustType<AstralOrange>() : ModContent.DustType<AstralBlue>(), 0.48f, 50, 78, -0.1f, 0.1f);
            if (d != null)
            {
                d.customData = 0.02f;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 8);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}

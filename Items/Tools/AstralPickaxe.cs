using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Tools
{
    public class AstralPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Pickaxe");
        }

        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.crit += 25;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 50;
            Item.height = 60;
            Item.useTime = 5;
            Item.useAnimation = 10;
            Item.useTurn = true;
            Item.pick = 220;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.tileBoost += 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 7);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust d = CalamityGlobalItem.MeleeDustHelper(player, Main.rand.NextBool(2) ? ModContent.DustType<AstralOrange>() : ModContent.DustType<AstralBlue>(), 0.56f, 40, 65, -0.13f, 0.13f);
            if (d != null)
            {
                d.customData = 0.02f;
            }
        }
    }
}

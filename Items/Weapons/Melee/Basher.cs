using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Basher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Basher");
/*
            Tooltip.SetDefault("Inflicts irradiated on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 42;
            Item.damage = 17;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = Item.useTime = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Acidwood>(), 30);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 120);
        }
    }
}

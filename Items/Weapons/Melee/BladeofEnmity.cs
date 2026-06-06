using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class BladeofEnmity : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blade of Enmity");
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 64;
			Item.scale = 1.5f;
            Item.damage = 158;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>(), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 600);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 600);
        }
    }
}

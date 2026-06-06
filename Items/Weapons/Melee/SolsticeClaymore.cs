using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class SolsticeClaymore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Solstice Claymore");
/*
            Tooltip.SetDefault("Changes projectile color based on the time of year\n" +
                               "Inflicts daybroken during the day and nightwither during the night");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 86;
            Item.damage = 520;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 16;
            Item.useTurn = true;
            Item.knockBack = 6.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 86;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<SolsticeBeam>();
            Item.shootSpeed = 16f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BeamSword);
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 20);
            recipe.AddIngredient(ItemID.FragmentSolar, 5);
            recipe.AddIngredient(ItemID.FragmentVortex, 5);
            recipe.AddIngredient(ItemID.FragmentStardust, 5);
            recipe.AddIngredient(ItemID.FragmentNebula, 5);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int dustType = Main.dayTime ?
            Utils.SelectRandom(Main.rand, new int[]
            {
            6,
            259,
            158
            }) :
            Utils.SelectRandom(Main.rand, new int[]
            {
            173,
            27,
            234
            });
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustType);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.dayTime)
            {
                target.AddBuff(BuffID.Daybreak, 300);
            }
            else
            {
                target.AddBuff(ModContent.BuffType<Nightwither>(), 300);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (!Main.dayTime)
            {
                target.AddBuff(ModContent.BuffType<Nightwither>(), 300);
            }
        }
    }
}

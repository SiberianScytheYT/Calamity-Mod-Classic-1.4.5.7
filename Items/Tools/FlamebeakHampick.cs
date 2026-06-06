using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
    public class FlamebeakHampick : ModItem
    {
        private static int PickPower = 210;
        private static int HammerPower = 130;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Seismic Hampick");
/*
            Tooltip.SetDefault("Capable of mining Lihzahrd Bricks\n"
                               +"Left click to use as a pickaxe\n"
                               +"Right click to use as a hammer");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 58;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 52;
            Item.height = 50;
            Item.useTime = 6;
            Item.useAnimation = 15;
            Item.useTurn = true;
            Item.pick = PickPower;
            Item.hammer = HammerPower;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3.5f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.tileBoost += 2;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.pick = 0;
				Item.hammer = HammerPower;
            }
            else
            {
                Item.pick = PickPower;
				Item.hammer = 0;
            }
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, Main.rand.NextBool(3) ? 16 : 127);
            }
            if (Main.rand.NextBool(5))
            {
				int smoke = Gore.NewGore(Entity.GetSource_FromThis(), new Vector2(hitbox.X, hitbox.Y), default, Main.rand.Next(375, 378), 0.75f);
				Main.gore[smoke].behindTiles = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}

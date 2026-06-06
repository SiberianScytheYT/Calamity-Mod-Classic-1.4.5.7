using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class CalamitousEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Calamitous Essence");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 34;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 24);
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, TextureAssets.Item[Item.type].Value);
		}

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float brightness = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.1f * brightness, 0.1f * brightness, 0.1f * brightness);
        }
    }
}
